using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class BGM : MonoBehaviour
{
    public static bool IsMute
    {
        set => PlayerPrefs.SetInt("BGM_Mute", value ? 1 : 0);
        get => PlayerPrefs.GetInt("BGM_Mute") == 1;
    }
    
    public static BGM instance;
    public List<ClipData> clips = new List<ClipData>();
    public int startIndex = -1;


    AudioSource source;
    ClipData currentData;

    private void Awake()
    {
        instance = this;
    }

    public void Setup()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        
        if (GameDataManager.Instance.IsAuctionOver())
        {
            Play(BGMs.DEFAULT);
        }
        else
        {
            Play(BGMs.AUCTION);
        }
    }

    private void MuteChange(bool obj)
    {
        source.volume = IsMute ? 0 : currentData.Volume;
    }

    public void Play(BGMs bgm)
    {
        var b = clips.FirstOrDefault(a => a.type == bgm);
        if (b == null)
        {
            Debug.LogError($"No BGM found with Type {bgm}");
            return;
        }

        if (source.clip is null)
        {
            source.clip = b.audioClip;
            source.pitch = b.Pitch;
            currentData = b;
            source.Play();
            source.DOFade(IsMute ? 0 : b.Volume,1f).From(0);
        }
        else
        {
            source.DOFade(0, 1f).onComplete += () =>
            {
                source.clip = b.audioClip;
                //source.volume = IsMute ? 0 : b.Volume;
                source.pitch = b.Pitch;
                currentData = b;
                source.Play();
                source.DOFade(IsMute ? 0 : b.Volume,1f);
            };   
        }
    }

    public void MuteChange()
    {
        IsMute = !IsMute;
        source.volume = IsMute ? 0 : currentData.Volume;
    }

    public enum BGMs
    {
        AUCTION,
        DEFAULT
    }

    [System.Serializable]
    public class ClipData
    {
        public BGMs type;
        public AudioClip audioClip;
        public float Volume => volume / 100;

        [Range(0, 100)] [SerializeField] float volume;

        public float Pitch => 1;
    }
}