using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
        }
        
    }

    private void Update() {
        if(Time.timeScale == 0){
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].source.Stop();
            }
        }
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                if(Time.timeScale > 0)s.source.Play();
        }
    }

    public void StopSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                s.source.Stop();
        }
    }
}
