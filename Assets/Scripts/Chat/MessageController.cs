using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    //TODO move colors to UI constants or use global constants 
    private static readonly Color FromMeColor = new Color(83.0f/255, 190.0f/255, 255.0f/255, 1.0f);
    private static readonly Color FromThemColor = new Color(235.0f/255, 235.0f/255, 235.0f/255, 1.0f);
    
    public RTLTextMeshPro text;
    public Image backgroundImage;
    public HorizontalLayoutGroup mainLayoutGroup;
    
    private bool fromMe;
    public bool IsFromMe
    {
        get => fromMe;
        set
        {
            fromMe = value;
            if (fromMe)
            {
                backgroundImage.color = FromMeColor;
                mainLayoutGroup.childAlignment = TextAnchor.UpperRight;
            }
            else
            {
                backgroundImage.color = FromThemColor;
                mainLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            }
        }
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}
