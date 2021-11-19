using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class OnMapMarkerCanvasSetter : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.allCameras[1];
        canvas.sortingOrder = 1;
    }
}
