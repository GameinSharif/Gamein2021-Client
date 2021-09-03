using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using System;

public class MapManager : MonoBehaviour
{
    private AbstractMap abstractMap;

    public List<GameObject> IsMapTypeSelectedGameObjects;

    private void Awake()
    {
        abstractMap = FindObjectOfType<AbstractMap>();

        int index = PlayerPrefs.GetInt("LastMapTypeIndex", 0);
        SetMapImagerySourceType(index);
    }

    public void SetMapImagerySourceType(int index)
    {
        abstractMap.ImageLayer.SetLayerSource((ImagerySourceType)index);
        PlayerPrefs.SetInt("LastMapTypeIndex", index);

        foreach (GameObject gameObject in IsMapTypeSelectedGameObjects)
        {
            gameObject.SetActive(false);
        }
        IsMapTypeSelectedGameObjects[index].SetActive(true);
    }
}
