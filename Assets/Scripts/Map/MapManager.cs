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
    private int _currnetMapTypeIndex = 0;

    public List<GameObject> IsMapTypeSelectedGameObjects;
    [Space]
    [Header("InitialValues")]
    public float PanSpeed = 2f;
    public float ZoomSpeed = 0f;
    public bool UseRetina = false;
    public bool UseCompression = true;
    public bool UseMipMap = true;

    private void Awake()
    {
        _abstractMap = FindObjectOfType<AbstractMap>();
        _quadTreeCameraMovement = FindObjectOfType<QuadTreeCameraMovement>();       
    }

    private void Start()
    {
        InitializeMap();

        _quadTreeCameraMovement.SetPanSpeed(PanSpeed);
        _quadTreeCameraMovement.SetZoomSpeed(ZoomSpeed);
    }

    private void InitializeMap()
    {
        int lastMapTypeIndex = PlayerPrefs.GetInt("LastMapTypeIndex", 0);
        _currnetMapTypeIndex = lastMapTypeIndex;
        _abstractMap.ImageLayer.SetProperties((ImagerySourceType)_currnetMapTypeIndex, UseRetina, UseCompression, UseMipMap);
        SetMapTypesActiveStatus();

        int mapZoomIndex = PlayerPrefs.GetInt("MapZoomIndex", 0);
        _currnetZoomAmountIndex = mapZoomIndex;
        _abstractMap.SetZoom(_possibleZoomAmounts[_currnetZoomAmountIndex]);
    }

    //Called On Map Types Button Click
    public void SetMapImagerySourceType(int index)
    {
        if (_currnetMapTypeIndex == index)
        {
            return;
        }

        _currnetMapTypeIndex = index;
        _abstractMap.ImageLayer.SetLayerSource((ImagerySourceType)_currnetMapTypeIndex);
        PlayerPrefs.SetInt("LastMapTypeIndex", _currnetMapTypeIndex);
        SetMapTypesActiveStatus();
    }

    private void SetMapTypesActiveStatus()
    {
        foreach (GameObject gameObject in IsMapTypeSelectedGameObjects)
        {
            gameObject.SetActive(false);
        }
        IsMapTypeSelectedGameObjects[_currnetMapTypeIndex].SetActive(true);
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
