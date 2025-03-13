using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController 
{
    static AudioController instance = new AudioController();
    public static AudioController Instance => instance;

    Dictionary<string, Stack<AudioSource>> pool=new Dictionary<string, Stack<AudioSource>>();
  
    public void Play(string path,Vector3 point,bool loop=false,float volume=1,float spactialBlend=1) { 
        
        if(string.IsNullOrEmpty(path)) return;

        AudioSource audio = null;
        if (pool.ContainsKey(path) && pool[path].Count>0)
        {
            audio = pool[path].Pop();
            audio.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = new GameObject("audio");
            audio = go.AddComponent<AudioSource>();
        }

        audio.transform.position = point;
        audio.clip=ResourcesManager.Instance.Load<AudioClip>(path);
        audio.loop=loop;    
        audio.volume=volume;
        audio.spatialBlend=spactialBlend;
        audio.Play();
    
    }


    public void Stop(string path,AudioSource audioSource)
    {
        if (pool.ContainsKey(path)==false)
        {
            pool[path] = new Stack<AudioSource>();
        }
        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        pool[path].Push(audioSource);
    }
}
