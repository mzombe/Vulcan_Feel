using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStop : MonoBehaviour
{
    public AudioSource audio;

    void Update()
    {
        if(Time.timeScale == 0) {
            audio.Stop();
        }else{
            audio.UnPause();
        }
    }
}
