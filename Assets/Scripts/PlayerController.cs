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

    public Renderer modelPlayer;

    bool toggle = false;

    private GameObject ball;
    private Camera mainCamera;
    private bool isDamaged = false, prevState = false; 

    [Header("Particles")] 
    public GameObject slamParticle;
    public ParticleSystem hitParticle;
    void Start()
    {   
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {       

        if(prevState == isGrounded){
            prevState = true;
            if(isGrounded == true && prevState == true){
                FindObjectOfType<AudioManager>().PlaySound("Land");
                GameObject particle = Instantiate(slamParticle, new Vector3(transform.position.x,-1f,transform.position.z), slamParticle.transform.rotation);
                Destroy(particle, 1f);
                prevState = false;
            }
        }


        //animator.SetBool("isGameStarted", true);
        move.z = forwardSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
        //animator.SetBool("isGrounded", isGrounded);
        if (isGrounded && velocity.y < 0)
            velocity.y = -1f;

        if (isGrounded)
        {

            if (SwipeManager.swipeUp){
                Jump();
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;  
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
        FindObjectOfType<AudioManager>().PlaySound("Jump");
        //animator.SetTrigger("jump");
        controller.center = Vector3.zero;
        controller.height = 2;
   
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Wall" )
        {
            Damage();
            GameObject wall = hit.gameObject;
            Destroy(wall);
        }

        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Meteor" )
        {
            Destroy(collision.gameObject);
            Damage();
        }
    }

    public void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Meteor")
        {   
            FindObjectOfType<AudioManager>().StopSound("Meteor");
            if(SwipeManager.tap && !isDamaged ){
                Destroy(collision.gameObject);
                HitBall();
                mainCamera.GetComponent<ScreenShake>().additionalStrength = 4f;
                mainCamera.GetComponent<ScreenShake>().start = true;
            }
        }
    }
    

    public void HitBall(){
        FindObjectOfType<AudioManager>().PlaySound("HitBall");
        hitParticle.Play();
        if(ball == null ){
            ball = Instantiate(preFabBall, exit.position, exit.rotation);
        }
        ball.GetComponent<Rigidbody>().velocity = exit.forward * 3500f * Time.deltaTime;
    }

    public void Damage(){

        if(!isDamaged){
            FindObjectOfType<AudioManager>().PlaySound("Damage");
            PlayerManager.instance.life --;
            mainCamera.GetComponent<ScreenShake>().additionalStrength = 1f;
            mainCamera.GetComponent<ScreenShake>().start = true;
            StartCoroutine(Blink());
        }
    }
   public IEnumerator Blink()
   {
      isDamaged = true;
      modelPlayer.enabled = false;
      yield return new WaitForSeconds (0.12f);
      modelPlayer.enabled = true;
      yield return new WaitForSeconds (0.24f);
      modelPlayer.enabled = false;
      yield return new WaitForSeconds (0.36f);
      modelPlayer.enabled = true;
      yield return new WaitForSeconds (0.24f);
      modelPlayer.enabled = false;
      yield return new WaitForSeconds (0.36f);
      modelPlayer.enabled = true;
      isDamaged = false;
   }

}
