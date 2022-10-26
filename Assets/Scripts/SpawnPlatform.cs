using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();
    public List<Transform> currentPlatforms = new List<Transform>();
    public int offset;

    private RandomSpawn spawnWalls;
    private Transform player;
    private Transform currentPlatformsPoint;
    private int platformIndex;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnWalls = GetComponent<RandomSpawn>();
        for (int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0f,-1f,i * 100f),transform.rotation).transform;
            currentPlatforms.Add(p);
            offset += 100;
        }    

        currentPlatformsPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
    }

    void Update()
    {
        float distance = player.position.z - currentPlatformsPoint.position.z;

        if(distance >= 5){
            Recycle(currentPlatforms[platformIndex].gameObject);
            platformIndex++;

            if(platformIndex > currentPlatforms.Count - 1){
                platformIndex = 0;
            }

            currentPlatformsPoint = currentPlatforms[platformIndex].GetComponent<Platform>().point;
        }
    }

    public void Recycle(GameObject platform){
        platform.transform.position = new Vector3(0,-1f,offset);
        offset += 100;

        for (int i = 0; i < RandomSpecific(); i++)
        {
            spawnWalls.GenerateWall(100f, 200f);
        }
    }

    public int RandomSpecific(){    
     
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
