using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBall : MonoBehaviour
{
    private GameObject Player;
    
    void Start(){
        StartCoroutine(Destroy());
        FindObjectOfType<AudioManager>().PlaySound("LaunchBall");
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall") {
            FindObjectOfType<AudioManager>().PlaySound("HitWall");
            PlayerManager.instance.ScoreAddPlus(50f);
            GameObject wall = collision.gameObject;
            wall.GetComponent<MeshDestroy>().DestroyMesh();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
