using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject preFabBall;
    public Transform exit;

    private CharacterController controller;
    private Vector3 move;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1;//0:left, 1:middle, 2:right
    public float laneDistance = 2.5f;//The distance between tow lanes

    public bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public float gravity = -12f;
    public float jumpHeight = 2;
    private Vector3 velocity;

    //public Animator animator;
    private bool isSliding = false, triggerMeteor = false;

    public float slideDuration = 1.5f;

    bool toggle = false;

    private GameObject ball;
    private Camera mainCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Time.timeScale = 1.2f;
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {

        //Increase Speed
        if (toggle)
        {
            toggle = false;
            if (forwardSpeed < maxSpeed)
                forwardSpeed += 0.1f * Time.fixedDeltaTime;
        }
        else
        {
            toggle = true;
            if (Time.timeScale < 2f)
                Time.timeScale += 0.005f * Time.fixedDeltaTime;
        }
    }

    void Update()
    {
        if(SwipeManager.tap && triggerMeteor && !SwipeManager.swipeLeft && !SwipeManager.swipeRight && !SwipeManager.swipeDown && !SwipeManager.swipeUp){
            HitBall();
            mainCamera.GetComponent<ScreenShake>().additionalStrength = 4f;
            mainCamera.GetComponent<ScreenShake>().start = true;
        }
        

        //animator.SetBool("isGameStarted", true);
        move.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        //animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && velocity.y < 0)
            velocity.y = -1f;

        if (isGrounded)
        {
            if (SwipeManager.swipeUp)
                Jump();

            if (SwipeManager.swipeDown && !isSliding)
                StartCoroutine(Slide());
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            if (SwipeManager.swipeDown && !isSliding)
            {
                StartCoroutine(Slide());
                velocity.y = -10;
            }                

        }
        controller.Move(velocity * Time.deltaTime);

        //Gather the inputs on which lane we should be
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (desiredLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * laneDistance;

        //transform.position = targetPosition;
        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        controller.Move(move * Time.deltaTime);
    }

    private void Jump()
    {   
        StopCoroutine(Slide());
        //animator.SetBool("isSliding", false);
        //animator.SetTrigger("jump");
        controller.center = Vector3.zero;
        controller.height = 2;
        isSliding = false;
   
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // if(hit.transform.tag == "Obstacle")
        // {
        //     FindObjectOfType<AudioManager>().PlaySound("GameOver");
        // }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        //animator.SetBool("isSliding", true);
        yield return new WaitForSeconds(0.25f/ Time.timeScale);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds((slideDuration - 0.25f)/Time.timeScale);

        //animator.SetBool("isSliding", false);

        controller.center = Vector3.zero;
        controller.height = 2;

        isSliding = false;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Meteor" )
        {
            triggerMeteor = true;
            StartCoroutine(TriggerFalse());
        }
    }

    private IEnumerator TriggerFalse()
    {
        yield return new WaitForSeconds(0.2f);
        triggerMeteor = false;
    }

    public void HitBall(){
        if(ball == null){
            ball = Instantiate(preFabBall, exit.position, exit.rotation);
        }    
        ball.GetComponent<Rigidbody>().velocity = exit.forward * 5000f * Time.deltaTime;
    }
}
