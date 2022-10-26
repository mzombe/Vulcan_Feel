using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject preFabWall;

    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < RandomSpecificInt(); i++)
        {
            GenerateWall(50f,100f);
        }
    }

    public void GenerateWall(float min, float max){
        Vector3 RandomSpawnPosition = new Vector3(RandomSpecific(),0f,Random.Range(Player.transform.position.z + min, Player.transform.position.z + max));
        Instantiate(preFabWall, RandomSpawnPosition, preFabWall.transform.rotation);
    }

    public float RandomSpecific(){    
     
     float val = Random.Range(1,3);
     
     if(val == 1)
         {
            return 0f;
         }
     else if(val == 2)
         {
             return -2.5f;
         }
     else if(val == 3)
         {
             return 2.5f;
         }
    return 0f;

    }

    public int RandomSpecificInt(){    
     
     float val = Random.Range(1,3);
     
     if(val == 1)
         {
            return 2;
         }
     else if(val == 2)
         {
             return 3;
         }
     else if(val == 3)
         {
             return 4;
         }
    return 1;
    
    }
}