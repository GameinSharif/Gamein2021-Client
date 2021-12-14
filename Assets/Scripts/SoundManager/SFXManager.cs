using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    
    public static bool IsMute
    {
        set => PlayerPrefs.SetInt("SFX_Mute", value ? 1 : 0);
        get => PlayerPrefs.GetInt("SFX_Mute", 0) == 1;
    }

    public List<ClipData> clips = new List<ClipData>();

    public Image muteButtonImage;
    private Color mutedColor = new Color(0.316f, 0.316f, 0.316f);
    
    private void Awake()
    {
        Instance = this;
        muteButtonImage.color = IsMute ? mutedColor : Color.white;
    }

    public void MuteChange()
    {
        IsMute = !IsMute;
        muteButtonImage.color = IsMute ? mutedColor : Color.white;
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