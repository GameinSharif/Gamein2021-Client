using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Examples;
using System;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    private AbstractMap _abstractMap;
    private QuadTreeCameraMovement _quadTreeCameraMovement;
    private List<MapUtils.OnMapMarker> _onMapMarkers = new List<MapUtils.OnMapMarker>();
    private List<MapUtils.OnMapLine> _onMapLines = new List<MapUtils.OnMapLine>();

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
    private readonly float _onMapMarkerVerticalDistanceFromMap = 2;

    public List<GameObject> IsMapTypeSelectedGameObjects;
    [Space]
    public GameObject MapAgenetMarkerPrefab;
    public List<MapUtils.MapAgentMarker> MapAgentMarkers;
    [Space]
    public GameObject MapLinePrefab;
    public List<MapUtils.MapLine> MapLines;

    private void Awake()
    {
        Instance = this;
        _abstractMap = FindObjectOfType<AbstractMap>();
        _quadTreeCameraMovement = FindObjectOfType<QuadTreeCameraMovement>();
    }

    private void Start()
    {
        InitializeMap();
        InitializeGameDataOnMap();

        //SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.Manufacturer, new Vector2(0, 0), 0);
        //SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.Manufacturer, new Vector2(35, 50), 1);

        //SetMapLine(MapUtils.MapLine.LineType.StorageToShop, _onMapMarkers[0], _onMapMarkers[1]);

        //ChangeMapAgentType(_onMapMarkers[0], MapUtils.MapAgentMarker.AgentType.Storage);

        SnapToLocation(new Vector2(46, 2));

        _quadTreeCameraMovement.SetPanSpeed(_panSpeed);
        _quadTreeCameraMovement.SetZoomSpeed(_zoomSpeed);
    }

    private void InitializeGameDataOnMap()
    {
        for (int i=0; i < GameDataManager.Instance.GameinCustomers.Count; i++)
        {
            Utils.GameinCustomer gameinCustomer = GameDataManager.Instance.GameinCustomers[i];
            SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.GameinCustomer, new Vector2d(gameinCustomer.latitude, gameinCustomer.longitude), i, gameinCustomer.name);
        }
        
        foreach (Utils.Auction auction in GameDataManager.Instance.Auctions)
        {
            MapUtils.MapAgentMarker.AgentType agentType;
            bool isTheirs = auction.highestBidTeamId == PlayerPrefs.GetInt("TeamId");
            bool isDifferentCountry = GameDataManager.Instance.GetFactoryById(auction.factoryId).country.ToString() !=
                                      PlayerPrefs.GetString("Country");
            bool isOver = auction.auctionBidStatus == Utils.AuctionBidStatus.Over;
            if (isDifferentCountry || !isTheirs && isOver)
            {
                agentType = MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory;
            } else if (auction.highestBidTeamId == null)
            {
                agentType = MapUtils.MapAgentMarker.AgentType.NoOwnerFactory;
            } else if (isTheirs)
            {
                agentType = MapUtils.MapAgentMarker.AgentType.MyFactory;
            }
            else
            {
                agentType = MapUtils.MapAgentMarker.AgentType.OtherFactory;
            }
            Utils.Factory factory = GameDataManager.Instance.GetFactoryById(auction.factoryId);
            SetMapAgentMarker(agentType, new Vector2d(factory.latitude, factory.longitude), auction.id, factory.name);
        }

        //TODO do the same thing for other map markers
    }

    public void UpdateOnMapObjects()
    {
        UpdateMarkersLocation();
        UpdateLinesLocation();
    }

    #region Initialize

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

    #endregion

    #region Zoom

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

    #endregion

    #region Markers

    public void SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType agentType, Vector2d location, int index, string name)
    {
        foreach (MapUtils.MapAgentMarker mapAgentMarker in MapAgentMarkers)
        {
            if (mapAgentMarker.MapAgentType == agentType)
            {
                var instance = Instantiate(MapAgenetMarkerPrefab);
                instance.GetComponent<MaterialSetter>().Initialize(mapAgentMarker, name);
                
                instance.transform.localPosition = _abstractMap.GeoToWorldPosition(location) + new Vector3(0, _onMapMarkerVerticalDistanceFromMap, 0);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                MapUtils.OnMapMarker onMapMarker = new MapUtils.OnMapMarker(location, instance, mapAgentMarker, index);
                _onMapMarkers.Add(onMapMarker);
                if (agentType == MapUtils.MapAgentMarker.AgentType.MyFactory ||
                    agentType == MapUtils.MapAgentMarker.AgentType.OtherFactory ||
                    agentType == MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory ||
                    agentType == MapUtils.MapAgentMarker.AgentType.NoOwnerFactory)
                {
                    Utils.Auction auction = GameDataManager.Instance.GetAuctionById(index);
                    instance.GetComponent<EachAuctionController>().SetAuctionValues(auction, onMapMarker);
                }
            }
        }
    }

    public void ChangeMapAgentType(MapUtils.OnMapMarker onMapMarker, MapUtils.MapAgentMarker.AgentType newAgentType)
    {
        foreach (MapUtils.MapAgentMarker mapAgentMarker in MapAgentMarkers)
        {
            if (mapAgentMarker.MapAgentType == newAgentType)
            {
                onMapMarker.MapAgentMarker = mapAgentMarker;
                onMapMarker.SpawnedObject.GetComponent<MaterialSetter>().SetMaterial(mapAgentMarker.MarkerMaterial);
            }
        }
    }

    public void UpdateAllOnMapMarkers()
    {
        foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
        {
            UpdateOnMapMarker(onMapMarker);
        }
    }

    public void UpdateOnMapMarker(MapUtils.OnMapMarker onMapMarker)
    {
        int auctionId = onMapMarker.Index;
        Utils.Auction newAuction = GameDataManager.Instance.GetAuctionById(auctionId);
        int teamId = PlayerPrefs.GetInt("TeamId"); 
        bool hasHighest = GameDataManager.Instance.Auctions.Exists(a => a.highestBidTeamId == teamId);
        bool isTheirs = newAuction.highestBidTeamId == PlayerPrefs.GetInt("TeamId");
        bool isDifferentCountry = GameDataManager.Instance.GetFactoryById(newAuction.factoryId).country.ToString() !=
                                  PlayerPrefs.GetString("Country");
        bool isOver = newAuction.auctionBidStatus == Utils.AuctionBidStatus.Over;
        if (isOver)
        {
            ChangeMapAgentType(onMapMarker,
                isTheirs
                    ? MapUtils.MapAgentMarker.AgentType.MyFactory
                    : MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory);
        } else if (isTheirs)
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.MyFactory);
        } else if (isDifferentCountry) 
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory);
        }
        else if (newAuction.highestBidTeamId == null)
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.NoOwnerFactory);
        }
        else
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.OtherFactory);
        }
        onMapMarker.SpawnedObject.GetComponent<EachAuctionController>().SetAuctionValues(newAuction, onMapMarker);
    }

    public MapUtils.OnMapMarker GetOnMapMarkerById(int id)
    {
        return _onMapMarkers.First(marker => marker.Index == id);
    }
    
    public void UpdateMarkersLocation()
    {
        foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
        {
            var spawnedObject = onMapMarker.SpawnedObject;
            var location = onMapMarker.Location;
            spawnedObject.transform.localPosition = _abstractMap.GeoToWorldPosition(location) + new Vector3(0, _onMapMarkerVerticalDistanceFromMap, 0);
        }
    }

    #endregion

    #region SnapToLocation

    public void SnapToLocation(Vector2 location)
    {
        Vector2d vector2d = new Vector2d(location.x, location.y);
        _abstractMap.SetCenterLatitudeLongitude(vector2d);
    }

    #endregion

    #region Lines
    
    public void SetMapLine(MapUtils.MapLine.LineType lineType, MapUtils.OnMapMarker start, MapUtils.OnMapMarker end)
    {
        foreach (MapUtils.MapLine mapLine in MapLines)
        {
            if (mapLine.MapLineType == lineType)
            {
                var instance = Instantiate(MapLinePrefab);

                LineRenderer lineRenderer = instance.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, start.SpawnedObject.transform.position);
                lineRenderer.SetPosition(1, end.SpawnedObject.transform.position);

                _onMapLines.Add(new MapUtils.OnMapLine(start, end, lineRenderer));
            }
        }
    }

    public void UpdateLinesLocation()
    {
        foreach (MapUtils.OnMapLine onMapLine in _onMapLines)
        {
            LineRenderer lineRenderer = onMapLine.LineRenderer;
            lineRenderer.SetPosition(0, onMapLine.Start.SpawnedObject.transform.position);
            lineRenderer.SetPosition(1, onMapLine.End.SpawnedObject.transform.position);
        }
    }

    #endregion

}
