using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed;


    void Update()
    {
        transform.position -= new Vector3(0f,0f,Time.deltaTime * speed);
    }

    
}
