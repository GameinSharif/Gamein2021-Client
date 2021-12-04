using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransportsController : MonoBehaviour
{
    public static TransportsController Instance;

    public GameObject transportItemPrefab;

    public List<Sprite> vehicleSprites;
    public List<Sprite> markerSprites;

    public RectTransform comingScrollPanel;
    public RectTransform goingScrollPanel;
    public RectTransform doneScrollPanel;
    public RectTransform crashScrollPanel;

    private PoolingSystem<Tuple<Utils.Transport, bool, bool>> _comingPool;
    private PoolingSystem<Tuple<Utils.Transport, bool, bool>> _goingPool;
    private PoolingSystem<Tuple<Utils.Transport, bool, bool>> _donePool;
    private PoolingSystem<Tuple<Utils.Transport, bool, bool>> _crashPool;
    
    private List<TransportItemController> _comingControllers = new List<TransportItemController>();
    private List<TransportItemController> _goingControllers = new List<TransportItemController>();

    private void Awake()
    {
        Instance = this;

        _comingPool = new PoolingSystem<Tuple<Utils.Transport, bool, bool>>(
            comingScrollPanel,
            transportItemPrefab,
            InitializeTransportItem
        );

        _goingPool = new PoolingSystem<Tuple<Utils.Transport, bool, bool>>(
            goingScrollPanel,
            transportItemPrefab,
            InitializeTransportItem
        );

        _donePool = new PoolingSystem<Tuple<Utils.Transport, bool, bool>>(
            doneScrollPanel,
            transportItemPrefab,
            InitializeTransportItem
        );

        _crashPool = new PoolingSystem<Tuple<Utils.Transport, bool, bool>>(
            crashScrollPanel,
            transportItemPrefab,
            InitializeTransportItem
        );
    }

    public Sprite GetSpriteByAgentType(MapUtils.MapAgentMarker.AgentType agentType)
    {
        return markerSprites[(int) agentType];
    }

    public Sprite GetVehicleSprite(Utils.VehicleType transportVehicleType)
    {
        return vehicleSprites[(int) transportVehicleType];
    }

    //resets everything
    public void Initialize()
    {
        _comingPool.RemoveAll();
        _goingPool.RemoveAll();
        _crashPool.RemoveAll();
        _donePool.RemoveAll();
        
        _comingControllers.Clear();
        _goingControllers.Clear();

        var transportListCopy = RemoveDuplicates(TransportManager.Instance.Transports);
        
        foreach (var transport in transportListCopy)
        {
            if (transport.transportState == Utils.TransportState.PENDING) continue;

            var isGoing = IsGoing(transport);
            var isInWay = transport.transportState == Utils.TransportState.IN_WAY;

            PoolingSystem<Tuple<Utils.Transport, bool, bool>> pool = transport.transportState switch
            {
                Utils.TransportState.IN_WAY => isGoing ? _goingPool : _comingPool,
                Utils.TransportState.CRUSHED => _crashPool,
                Utils.TransportState.SUCCESSFUL => _donePool,
            };

            pool.Add(new Tuple<Utils.Transport, bool, bool>(transport, isGoing, isInWay));
        }
        
        RebuildListLayout(comingScrollPanel);
        RebuildListLayout(goingScrollPanel);
        RebuildListLayout(doneScrollPanel);
        RebuildListLayout(crashScrollPanel);
    }

    private List<Utils.Transport> RemoveDuplicates(List<Utils.Transport> mainList)
    {
        var newList = new List<Utils.Transport>(mainList.Count);

        for (int i = 0; i < mainList.Count; i++)
        {
            bool isUnique = true;
            for (int j = 0; j < newList.Count; j++)
            {
                if (mainList[i].id == newList[j].id)
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                newList.Add(mainList[i]);
            }
        }
        
        return newList;
    }

    private bool IsGoing(Utils.Transport transport)
    {
        if (transport.sourceType == Utils.TransportNodeType.FACTORY ||
            transport.sourceType == Utils.TransportNodeType.DC)
        {
            return StorageManager.Instance.GetStorageByBuildingIdAndType(transport.sourceId, transport.sourceType == Utils.TransportNodeType.DC) != null;
        }

        return false;
    }

    private void InitializeTransportItem(GameObject theGameObject, int index, Tuple<Utils.Transport, bool, bool> transportData)
    {
        var (transport, isGoing, shouldKeepController) = transportData;
        
        var controller = theGameObject.GetComponent<TransportItemController>();
        controller.Initialize(transport, isGoing);

        if (shouldKeepController)
        {
            if (isGoing)
            {
                _goingControllers.Add(controller);
            }
            else
            {
                _comingControllers.Add(controller);
            }
        }
    }

    public void AddInWay(Utils.Transport transport)
    {
        var isGoing = IsGoing(transport);
        var pool = isGoing ? _goingPool : _comingPool;
        pool.Add(new Tuple<Utils.Transport, bool, bool>(transport, isGoing, true));
        RebuildListLayout(isGoing ? goingScrollPanel : comingScrollPanel);
    }

    public void AddCrashed(Utils.Transport transport)
    {
        var isGoing = IsGoing(transport);

        RemoveInWay(transport, isGoing);
        
        _crashPool.Add(new Tuple<Utils.Transport, bool, bool>(transport, isGoing, false));
        RebuildListLayout(crashScrollPanel);
    }

    public void AddDone(Utils.Transport transport)
    {
        var isGoing = IsGoing(transport);

        RemoveInWay(transport, isGoing);
        
        _donePool.Add(new Tuple<Utils.Transport, bool, bool>(transport, isGoing, false));
        RebuildListLayout(doneScrollPanel);
    }

    private void RemoveInWay(Utils.Transport transport, bool isGoing)
    {
        if (isGoing)
        {
            for (int i = 0; i < _goingControllers.Count; i++)
            {
                var controller = _goingControllers[i];
                if (controller.Transport.id != transport.id) continue;

                _goingPool.Remove(controller.gameObject);
                _goingControllers.Remove(controller);

                RebuildListLayout(goingScrollPanel);
                break;
            }
        }
        else
        {
            for (int i = 0; i < _comingControllers.Count; i++)
            {
                var controller = _comingControllers[i];
                if (controller.Transport.id != transport.id) continue;

                _comingPool.Remove(controller.gameObject);
                _comingControllers.Remove(controller);

                RebuildListLayout(comingScrollPanel);
                break;
            }
        }
    }

    private void RebuildListLayout(RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}