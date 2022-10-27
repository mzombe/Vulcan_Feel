using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBall : MonoBehaviour
{
    private GameObject Player;
    
    void Start(){
        StartCoroutine(Destroy());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall") {
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
