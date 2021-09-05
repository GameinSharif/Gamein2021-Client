using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Examples;
using System;

public class MapManager : MonoBehaviour
{
    private AbstractMap _abstractMap;
    private QuadTreeCameraMovement _quadTreeCameraMovement;
    private float[] _possibleZoomAmounts = { 2, 4, 6, 8, 10 };
    private int _currnetZoomAmountIndex = 0;

    public List<GameObject> IsMapTypeSelectedGameObjects;
    public float PanSpeed = 2f;
    public float ZoomSpeed = 0f;

    private void Awake()
    {
        _abstractMap = FindObjectOfType<AbstractMap>();
        _quadTreeCameraMovement = FindObjectOfType<QuadTreeCameraMovement>();       
    }

    private void Start()
    {
        int lastMapTypeIndex = PlayerPrefs.GetInt("LastMapTypeIndex", 0);
        SetMapImagerySourceType(lastMapTypeIndex);

        InitializeMapZoomAmount();

        _quadTreeCameraMovement.SetPanSpeed(PanSpeed);
        _quadTreeCameraMovement.SetZoomSpeed(ZoomSpeed);
    }

    public void SetMapImagerySourceType(int index)
    {
        _abstractMap.ImageLayer.SetLayerSource((ImagerySourceType)index);
        PlayerPrefs.SetInt("LastMapTypeIndex", index);

        foreach (GameObject gameObject in IsMapTypeSelectedGameObjects)
        {
            gameObject.SetActive(false);
        }
        IsMapTypeSelectedGameObjects[index].SetActive(true);
    }

    private void InitializeMapZoomAmount()
    {
        int mapZoomIndex = PlayerPrefs.GetInt("MapZoomIndex", 0);
        _currnetZoomAmountIndex = mapZoomIndex;
        _abstractMap.SetZoom(_possibleZoomAmounts[_currnetZoomAmountIndex]);
    }

    private void SetMapZoom(int index)
    {
        _currnetZoomAmountIndex = index;
        _abstractMap.UpdateMap(_possibleZoomAmounts[_currnetZoomAmountIndex]);
        PlayerPrefs.SetInt("MapZoomIndex", index);
    }

    public void ZoomIn()
    {
        if (_currnetZoomAmountIndex < 4)
        {
            SetMapZoom(_currnetZoomAmountIndex + 1);
        }
    }

    public void ZoomOut()
    {
        if (_currnetZoomAmountIndex > 0)
        {
            SetMapZoom(_currnetZoomAmountIndex - 1);
        }
    }
}
