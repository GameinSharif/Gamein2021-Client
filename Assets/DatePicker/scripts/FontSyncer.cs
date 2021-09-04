using System;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class FontSyncer : MonoBehaviour
{
    void Start()
    {
        //TMP_FontAsset localizeFont = LocalizationManager.Instance.EnglishFontAsset;
        TMP_FontAsset localizeFont = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>().EnglishFontAsset;

        TMP_Text[] children = GetComponentsInChildren<TMP_Text>();
        foreach (var child in children)
        {
            child.font = localizeFont;
        }
    }
}
