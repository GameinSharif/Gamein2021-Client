using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    
    public static bool IsMute
    {
        set => PlayerPrefs.SetInt("SFX_Mute", value ? 1 : 0);
        get => PlayerPrefs.GetInt("SFX_Mute") == 1;
    }

    public List<ClipData> clips = new List<ClipData>();

    private void Awake()
    {
        Instance = this;
    }

    public void MuteChange()
    {
        IsMute = !IsMute;
    }

    public void Play(SfxID id)
    {
        var c = clips.FirstOrDefault(a => a.id == id);
        if (c == null)
        {
            Debug.LogError($"No SFX Found With ID {id}");
            return;
        }

        SFXPool.Play(c);
    }

    [System.Serializable]
    public class ClipData
    {
        public SfxID id;
        public AudioClip audioClip;
        public float Volume => volume / 100;

        [Range(0, 100)] [SerializeField] float volume;

        public float Pitch => 1;
    }
    
    public enum SfxID
    {
        CLICK,
        NOTIFICATION,
        NEWS_NOTIFICATION,
        RANDOMIZE_COUNTRY
    }
}