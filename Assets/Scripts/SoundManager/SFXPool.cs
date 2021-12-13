using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPool : MonoBehaviour
{
    private static int AudioSAMounts = 5;
    private static List<AudioSource> availableSources = new List<AudioSource>();
    private static List<Data> busySources = new List<Data>();
    
    
    private static SFXPool _inst;

    private static SFXPool Setup()
    {
        GameObject g = new GameObject("SFX Pool");
        _inst = g.AddComponent<SFXPool>();
        for (int i = 0; i < AudioSAMounts; i++)
        {
            availableSources.Add(g.AddComponent<AudioSource>());
        }
        DontDestroyOnLoad(g);
        return _inst;
    }
   
    public static void Play(SFXManager.ClipData data)
    {
        if (_inst == null)
        {
            _inst = Setup();
        }
        if (availableSources.Count <= 0)
            return;
        var s = availableSources[0];
        s.volume =SFXManager.IsMute?0: data.Volume;
        s.pitch = data.Pitch;
        s.clip = data.audioClip;
        s.loop = false;
        s.Play();
        availableSources.Remove(s);
        busySources.Add(new Data()
        {
            source=s,
            time=data.audioClip.length
        });
    }
    private void Update()
    {
        foreach (var item in busySources.ToArray())
        {
            item.time -= Time.deltaTime;
            if (item.time <= 0)
            {
                availableSources.Add(item.source);
                busySources.Remove(item);
            }
        }
    }
    class Data
    {
        public AudioSource source;
        public float time;
    }

}
