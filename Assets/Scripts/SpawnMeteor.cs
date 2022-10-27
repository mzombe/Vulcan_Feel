using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteor : MonoBehaviour
{
    public GameObject preFab;
    public bool stopSpawning = false;
    public bool hasObj = false;

    private GameObject instanciate;

    void Start()
    {
        if(!stopSpawning && !hasObj) {
             instanciate = Instantiate(preFab, transform.position, transform.rotation);
        }
    }

   
    void Update()
    {
        if(instanciate == null && !stopSpawning && !hasObj){
            StartCoroutine(WaitSpawn());
            hasObj = true;
        }else{
            hasObj = false;
        }
    }

    IEnumerator WaitSpawn(){
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        if(instanciate == null)instanciate = Instantiate(preFab, transform.position, transform.rotation);
    }

}
