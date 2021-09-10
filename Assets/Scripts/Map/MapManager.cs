using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Examples;
using System;

public class MapManager : MonoBehaviour
{
    private AbstractMap _abstractMap;
    private QuadTreeCameraMovement _quadTreeCameraMovement;
    private List<OnMapMarker> _onMapMarkers = new List<OnMapMarker>();

    private readonly float[] _possibleZoomAmounts = { 2, 4, 6, 8, 10 };
    private int _currnetZoomAmountIndex = 0;
    private int _currnetMapTypeIndex = 0;

    private readonly float _panSpeed = 2f;
    private readonly float _zoomSpeed = 0f;
    private readonly bool _useRetina = false;
    private readonly bool _useCompression = true;
    private readonly bool _useMipMap = true;
    private readonly ElevationLayerType _elevationLayerType = ElevationLayerType.FlatTerrain;
    private readonly float _spawnScale = 10f;

    public List<GameObject> IsMapTypeSelectedGameObjects;
    [Space]
    public GameObject MapAgenetMarkerPrefab;
    public List<MapAgentMarker> MapAgentMarkers;

    public struct OnMapMarker
    {
        public Vector2d Location;
        public GameObject SpawnedObject;

        public OnMapMarker(Vector2d location, GameObject spawnedObject)
        {
            Location = location;
            SpawnedObject = spawnedObject;
        }
    }

    private void Awake()
    {
        _abstractMap = FindObjectOfType<AbstractMap>();
        _quadTreeCameraMovement = FindObjectOfType<QuadTreeCameraMovement>();       
    }

    private void Start()
    {
        InitializeMap();

        //SetMapAgentMarker(MapAgentMarker.AgentType.Manufacturer, new Vector2(0, 0));
        //SnapToLocation(new Vector2(35,50));

        _quadTreeCameraMovement.SetPanSpeed(_panSpeed);
        _quadTreeCameraMovement.SetZoomSpeed(_zoomSpeed);
    }

    private void InitializeMap()
    {
        int lastMapTypeIndex = PlayerPrefs.GetInt("LastMapTypeIndex", 0);
        _currnetMapTypeIndex = lastMapTypeIndex;
        _abstractMap.ImageLayer.SetProperties((ImagerySourceType)_currnetMapTypeIndex, _useRetina, _useCompression, _useMipMap);
        SetMapTypesActiveStatus();

        int mapZoomIndex = PlayerPrefs.GetInt("MapZoomIndex", 0);
        _currnetZoomAmountIndex = mapZoomIndex;
        _abstractMap.SetZoom(_possibleZoomAmounts[_currnetZoomAmountIndex]);

        _abstractMap.Terrain.SetElevationType(_elevationLayerType);
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

    public void SetMapAgentMarker(MapAgentMarker.AgentType agentType, Vector2 location)
    {
        Vector2d vector2d = new Vector2d(location.x, location.y);

        foreach (MapAgentMarker mapAgentMarker in MapAgentMarkers)
        {
            if (mapAgentMarker.MapAgentType == agentType)
            {
                var instance = Instantiate(MapAgenetMarkerPrefab);
                instance.GetComponent<MaterialSetter>().SetMaterial(mapAgentMarker.MarkerMaterial);
                
                instance.transform.localPosition = _abstractMap.GeoToWorldPosition(vector2d);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _onMapMarkers.Add(new OnMapMarker(vector2d, instance));
            }
        }
    }

    public void UpdateMarkersLocation()
    {
        foreach (OnMapMarker onMapMarker in _onMapMarkers)
        {
            var spawnedObject = onMapMarker.SpawnedObject;
            var location = onMapMarker.Location;
            spawnedObject.transform.localPosition = _abstractMap.GeoToWorldPosition(location);
        }
    }

    public void SnapToLocation(Vector2 location)
    {
        Vector2d vector2d = new Vector2d(location.x, location.y);
        _abstractMap.SetCenterLatitudeLongitude(vector2d);
    }
}
