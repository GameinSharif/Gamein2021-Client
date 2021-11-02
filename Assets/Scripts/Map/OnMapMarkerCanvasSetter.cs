using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapMarkerCanvasSetter : MonoBehaviour
{
    public Canvas canvas;

    private void OnMouseDown()
    {
        Debug.Log(1);
    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.allCameras[0];
        canvas.sortingOrder = 1;
    }
}
