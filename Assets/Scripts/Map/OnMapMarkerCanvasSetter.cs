using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapMarkerCanvasSetter : MonoBehaviour
{
    public Canvas canvas;

    private void Start()
    {
        canvas.worldCamera = Camera.allCameras[1];
        canvas.sortingOrder = 1;
    }
}
