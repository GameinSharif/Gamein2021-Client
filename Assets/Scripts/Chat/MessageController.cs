using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    
    public RTLTextMeshPro text;

    public void SetText(string value)
    {
        text.text = value;
    }
}
