using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Examples;
using System;
using System.Linq;
using RTLTMPro;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public static Vector2 SnapToLocaltionOnOpenMap;
    public static bool IsInMap = false;

    private AbstractMap _abstractMap;
    private QuadTreeCameraMovement _quadTreeCameraMovement;
    private List<MapUtils.OnMapMarker> _onMapMarkers = new List<MapUtils.OnMapMarker>();
    private List<MapUtils.OnMapLine> _onMapLines = new List<MapUtils.OnMapLine>();

    private readonly float[] _possibleZoomAmounts = { 6, 8, 9, 10 };
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

    public Camera Camera;
    public List<GameObject> IsMapTypeSelectedGameObjects;
    [Space]
    public GameObject MapAgenetMarkerPrefab;
    public GameObject OnMapMarkersParent;
    public List<MapUtils.MapAgentMarker> MapAgentMarkers;
    [Space]
    public GameObject MapLinePrefab;
    public GameObject MapLinesParent;
    public List<MapUtils.MapLine> MapLines;
    [Space]
    public GameObject SnapToMyTeamLocationButtonGameObject;
    [Space]
    public GameObject OtherCountryFactoriesLegend;
    [Space]
    public RTLTextMeshPro CashForAuction;

    private List<GameObject> _linesSpawnedObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        _abstractMap = FindObjectOfType<AbstractMap>();
        _quadTreeCameraMovement = FindObjectOfType<QuadTreeCameraMovement>();

        var canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    private void Start()
    {
        InitializeMap();
        InitializeGameDataOnMap();

        if (SnapToLocaltionOnOpenMap != Vector2.zero)
        {
            SnapToLocation(SnapToLocaltionOnOpenMap);
        }
        else
        {
            SnapToMyTeamLocation();
        }

        if (!GameDataManager.Instance.IsAuctionOver())
        {
            StartCoroutine(BidForAuctionManager.Instance.AuctionTimer());
        }

        _quadTreeCameraMovement.SetPanSpeed(_panSpeed);
        _quadTreeCameraMovement.SetZoomSpeed(_zoomSpeed);

        MainMenuManager.IsLoadingMap = false;
        IsInMap = true;
    }

    //Also called by a button in MapScene
    public void SnapToMyTeamLocation()
    {
        SnapToLocation(GameDataManager.Instance.GetMyTeamLocaionOnMap());
    }

    public void InitializeGameDataOnMap()
    {
        InitializeMapMarkers();
        InitializeLines();
    }

    private void InitializeLines()
    {
        for (int i=0;i < TransportManager.Instance.Transports.Count; i++)
        {
            Utils.Transport transport = TransportManager.Instance.Transports[i];
            if (transport.transportState == Utils.TransportState.IN_WAY)
            {
                MapUtils.OnMapMarker sourceNode = GetOnMapMarkerByTypeAndId(transport.sourceType, transport.sourceId);
                MapUtils.OnMapMarker destinationNode = GetOnMapMarkerByTypeAndId(transport.destinationType, transport.destinationId);
                MapUtils.MapLine.LineType lineType = GetMapLineType(sourceNode, destinationNode);

                SetMapLine(lineType, sourceNode, destinationNode);
            }
        }
    }

    private void InitializeMapMarkers()
    {
        for (int i = 0; i < GameDataManager.Instance.GameinCustomers.Count; i++)
        {
            Utils.GameinCustomer gameinCustomer = GameDataManager.Instance.GameinCustomers[i];
            SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.GameinCustomer, new Vector2d(gameinCustomer.latitude, gameinCustomer.longitude), gameinCustomer.id, gameinCustomer.name);
        }

        for (int i = 0; i < GameDataManager.Instance.DCs.Count; i++)
        {
            Utils.DC dc = GameDataManager.Instance.DCs[i];
            string dcName = dc.name;

            MapUtils.MapAgentMarker.AgentType agentType;
            if (dc.ownerId == null)
            {
                agentType = MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter;
            }
            else if (dc.ownerId == PlayerPrefs.GetInt("TeamId"))
            {
                agentType = MapUtils.MapAgentMarker.AgentType.MyDistributionCenter;
                dcName = PlayerPrefs.GetString("TeamName");
            }
            else
            {
                agentType = MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter;
                dcName = GameDataManager.Instance.GetTeamById(dc.ownerId.Value).teamName;
            }

            SetMapAgentMarker(agentType, new Vector2d(dc.latitude, dc.longitude), dc.id, dcName);
        }
        
        for (int i=0; i < GameDataManager.Instance.GameinSuppliers.Count; i++)
        {
            Utils.Supplier gameinSupplier = GameDataManager.Instance.GameinSuppliers[i];
            SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.Supplier, new Vector2d(gameinSupplier.latitude, gameinSupplier.longitude), gameinSupplier.id, gameinSupplier.name);
        }

        if (GameDataManager.Instance.IsAuctionOver())
        {
            OtherCountryFactoriesLegend.SetActive(true);
            SnapToMyTeamLocationButtonGameObject.SetActive(true);

            int teamId = PlayerPrefs.GetInt("TeamId");
            for (int i = 0; i < GameDataManager.Instance.Teams.Count; i++)
            {
                Utils.Team team = GameDataManager.Instance.Teams[i];
                if (team.factoryId == 0)
                {
                    continue;
                }
                Utils.Factory factory = GameDataManager.Instance.GetFactoryById(team.factoryId);
                if (team.id == teamId)
                {
                    SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.MyFactory, new Vector2d(factory.latitude, factory.longitude), factory.id, team.teamName);
                }
                else
                {
                    SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.OtherFactory, new Vector2d(factory.latitude, factory.longitude), factory.id, team.teamName);
                }
            }
        }
        else
        {
            OtherCountryFactoriesLegend.SetActive(false);
            SnapToMyTeamLocationButtonGameObject.SetActive(false);

            Enum.TryParse(PlayerPrefs.GetString("Country"), out Utils.Country country);
            for (int i = 0; i < GameDataManager.Instance.Factories.Count; i++)
            {
                Utils.Factory factory = GameDataManager.Instance.Factories[i];
                if (factory.country == country)
                {
                    SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.NoOwnerFactory, new Vector2d(factory.latitude, factory.longitude), factory.id, factory.name);

                }
                else
                {
                    SetMapAgentMarker(MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory, new Vector2d(factory.latitude, factory.longitude), factory.id, factory.name);
                }
            }

            UpdateAllAuctions();
            
        }
        InitializeAllDCs();
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
        int lastMapTypeIndex = PlayerPrefs.GetInt("LastMapTypeIndex", 1);
        _currnetMapTypeIndex = lastMapTypeIndex;
        _abstractMap.ImageLayer.SetProperties((ImagerySourceType)_currnetMapTypeIndex, _useRetina, _useCompression, _useMipMap);
        SetMapTypesActiveStatus();

        int mapZoomIndex = PlayerPrefs.GetInt("MapZoomIndex", 3);
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

        if (index == 0)
        {
            foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
            {
                switch (onMapMarker.MapAgentMarker.MapAgentType)
                {
                    case MapUtils.MapAgentMarker.AgentType.OtherFactory:
                    case MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory:
                    case MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter:
                    case MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter:
                        onMapMarker.SpawnedObject.SetActive(false);
                        break;
                }
            }
        }
        else
        {
            foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
            {
                onMapMarker.SpawnedObject.SetActive(true);
            }
        }
    }

    public void ZoomIn()
    {
        if (_currnetZoomAmountIndex < _possibleZoomAmounts.Length -1)
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
                var instance = Instantiate(MapAgenetMarkerPrefab, OnMapMarkersParent.transform);
                instance.GetComponent<MaterialSetter>().Initialize(mapAgentMarker, name);
                
                instance.transform.localPosition = _abstractMap.GeoToWorldPosition(location) + new Vector3(0, _onMapMarkerVerticalDistanceFromMap, 0);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                MapUtils.OnMapMarker onMapMarker = new MapUtils.OnMapMarker(location, instance, mapAgentMarker, index);
                _onMapMarkers.Add(onMapMarker);
            }
        }
    }

    public void ChangeMapAgentType(MapUtils.OnMapMarker onMapMarker, MapUtils.MapAgentMarker.AgentType newAgentType, string name = null)
    {
        foreach (MapUtils.MapAgentMarker mapAgentMarker in MapAgentMarkers)
        {
            if (mapAgentMarker.MapAgentType == newAgentType)
            {
                onMapMarker.MapAgentMarker = mapAgentMarker;
                onMapMarker.SpawnedObject.GetComponent<MaterialSetter>().Initialize(mapAgentMarker, name);
            }
        }
    }

    public void UpdateAllAuctions()
    {
        foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
        {
            if (onMapMarker.MapAgentMarker.MapAgentType.ToString().Contains("Factory"))
            {
                UpdateFactory(onMapMarker);
            }
        }
    }

    public void UpdateFactory(MapUtils.OnMapMarker onMapMarker)
    {
        int factoryId = onMapMarker.Index;
        Utils.Auction auction = GameDataManager.Instance.GetAuctionByFactoryId(factoryId);
        if (auction == null) //This factory has no bid yet.
        {
            onMapMarker.SpawnedObject.GetComponent<EachAuctionController>().SetAuctionValues(auction, onMapMarker);

            return;
        }

        bool isForThisTeam = auction.highestBidTeamId == PlayerPrefs.GetInt("TeamId");
        bool isDifferentCountry = GameDataManager.Instance.GetFactoryById(auction.factoryId).country.ToString() != PlayerPrefs.GetString("Country");
        bool isOver = auction.auctionBidStatus == Utils.AuctionBidStatus.Over;
        if (isOver)
        {
            ChangeMapAgentType(onMapMarker,
                isForThisTeam
                    ? MapUtils.MapAgentMarker.AgentType.MyFactory
                    : MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory);
        } 
        else if (isForThisTeam)
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.MyFactory);
        } 
        else if (isDifferentCountry) 
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory);
        }
        else
        {
            ChangeMapAgentType(onMapMarker, MapUtils.MapAgentMarker.AgentType.OtherFactory);
        }

        onMapMarker.SpawnedObject.GetComponent<EachAuctionController>().SetAuctionValues(auction, onMapMarker);
    }

    public MapUtils.OnMapMarker GetOnMapMarkerById(int id)
    {
        return _onMapMarkers.First(marker => marker.Index == id);
    }

    public MapUtils.OnMapMarker GetOnMapMarkerByTypeAndId(Utils.TransportNodeType transportNodeType, int transportNodeId)
    {
        switch (transportNodeType)
        {
            case Utils.TransportNodeType.SUPPLIER:
                foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
                {
                    if (onMapMarker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.Supplier && onMapMarker.Index == transportNodeId)
                    {
                        return onMapMarker;
                    }
                }
                break;
            case Utils.TransportNodeType.GAMEIN_CUSTOMER:
                foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
                {
                    if (onMapMarker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.GameinCustomer && onMapMarker.Index == transportNodeId)
                    {
                        return onMapMarker;
                    }
                }
                break;
            case Utils.TransportNodeType.DC:
                foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
                {
                    if (onMapMarker.MapAgentMarker.MapAgentType.ToString().Contains("DistributionCenter") && onMapMarker.Index == transportNodeId)
                    {
                        return onMapMarker;
                    }
                }
                break;
            case Utils.TransportNodeType.FACTORY:
                foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
                {
                    if (onMapMarker.MapAgentMarker.MapAgentType.ToString().Contains("Factory") && onMapMarker.Index == transportNodeId)
                    {
                        return onMapMarker;
                    }
                }
                break;
        }

        return null;
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

    private void InitializeAllDCs()
    {
        foreach (MapUtils.OnMapMarker onMapMarker in _onMapMarkers)
        {
            switch (onMapMarker.MapAgentMarker.MapAgentType)
            {
                case MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter:
                case MapUtils.MapAgentMarker.AgentType.MyDistributionCenter:
                case MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter:
                    
                    Utils.DC dcDto = GameDataManager.Instance.DCs.Find(dto => dto.id == onMapMarker.Index);
                    onMapMarker.SpawnedObject.GetComponent<EachDcController>().SetValues(dcDto, onMapMarker.MapAgentMarker.MapAgentType);
                    break;
            }
        }
    }

    public void UpdateDcMarker(Utils.DC dc, bool isSold)
    {
        MapUtils.OnMapMarker onMapMarker = _onMapMarkers.Find(marker =>
            marker.Index == dc.id &&
            (marker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter ||
             marker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.MyDistributionCenter ||
             marker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter));

        string dcName = dc.name;
        MapUtils.MapAgentMarker.AgentType agentType;
        if (dc.ownerId == null)
        {
            agentType = MapUtils.MapAgentMarker.AgentType.NoOwnerDistributionCenter;
        } 
        else if (dc.ownerId == PlayerPrefs.GetInt("TeamId"))
        {
            agentType = MapUtils.MapAgentMarker.AgentType.MyDistributionCenter;
            dcName = PlayerPrefs.GetString("TeamName");
        }
        else
        {
            agentType = MapUtils.MapAgentMarker.AgentType.OtherDistributionCenter;
            dcName = GameDataManager.Instance.GetTeamById(dc.ownerId.Value).teamName;
        }

        ChangeMapAgentType(onMapMarker, agentType, dcName);
        
        onMapMarker.SpawnedObject.GetComponent<EachDcController>().SetValues(dc, onMapMarker.MapAgentMarker.MapAgentType);
    }

    #endregion

    #region SnapToLocation

    public void SnapToLocation(Vector2 location)
    {
        Vector2d vector2d = new Vector2d(location.x, location.y);
        _abstractMap.SetCenterLatitudeLongitude(vector2d);
        try
        {
            _abstractMap.UpdateMap();
        }
        catch (Exception)
        {
            //just ignore this
        }
    }

    #endregion

    #region Lines
    
    public void SetMapLine(MapUtils.MapLine.LineType lineType, MapUtils.OnMapMarker start, MapUtils.OnMapMarker end)
    {
        foreach (MapUtils.MapLine mapLine in MapLines)
        {
            if (mapLine.MapLineType == lineType)
            {
                var instance = GetLine();
                instance.GetComponent<LineMaterialSetter>().SetMaterial(mapLine.LineMaterial);

                LineRenderer lineRenderer = instance.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, start.SpawnedObject.transform.position);
                lineRenderer.SetPosition(1, end.SpawnedObject.transform.position);

                _onMapLines.Add(new MapUtils.OnMapLine(start, end, lineRenderer));
            }
        }
    }

    private GameObject GetLine()
    {
        foreach (GameObject gameObject in _linesSpawnedObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        return Instantiate(MapLinePrefab, MapLinesParent.transform);
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

    public void UpdateLine(Utils.Transport transport)
    {
        MapUtils.OnMapMarker sourceNode = GetOnMapMarkerByTypeAndId(transport.sourceType, transport.sourceId);
        MapUtils.OnMapMarker destinationNode = GetOnMapMarkerByTypeAndId(transport.destinationType, transport.destinationId);
        MapUtils.MapLine.LineType lineType = GetMapLineType(sourceNode, destinationNode);

        foreach (MapUtils.OnMapLine onMapLine in _onMapLines)
        {
            if (onMapLine.Start == sourceNode && onMapLine.End == destinationNode)
            {
                switch (transport.transportState)
                {
                    case Utils.TransportState.IN_WAY:
                        ChangeLineType(onMapLine, lineType);
                        return;
                    case Utils.TransportState.SUCCESSFUL:
                    case Utils.TransportState.CRUSHED:
                        ChangeLineType(onMapLine, MapUtils.MapLine.LineType.SupplyChain);
                        return;
                    case Utils.TransportState.PENDING:
                        //Do nothing
                        break;
                }
            }
        }

        SetMapLine(lineType, sourceNode, destinationNode);
    }

    public void ChangeLineType(MapUtils.OnMapLine onMapLine, MapUtils.MapLine.LineType newLineType)
    {
        foreach (MapUtils.MapLine mapLine in MapLines)
        {
            if (mapLine.MapLineType == newLineType)
            {
                onMapLine.LineRenderer.gameObject.GetComponent<LineMaterialSetter>().SetMaterial(mapLine.LineMaterial);
            }
        }
    }

    public MapUtils.MapLine.LineType GetMapLineType(MapUtils.OnMapMarker sourceNode, MapUtils.OnMapMarker destinationNode)
    {
        //TODO
        return MapUtils.MapLine.LineType.FactoryToFactory;
    }

    #endregion
}
