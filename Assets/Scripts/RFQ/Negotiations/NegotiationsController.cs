using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;
using UnityEngine.UI;

public class NegotiationsController : MonoBehaviour
{
    public static NegotiationsController Instance;

    public GameObject negotiationItemPrefab;

    private List<NegotiationItemController> _supplyNegotiationItemControllers = new List<NegotiationItemController>();
    private List<NegotiationItemController> _demandNegotiationItemControllers = new List<NegotiationItemController>();
    
    public RectTransform supplyNegotiationsScrollPanel;
    public RectTransform demandNegotiationsScrollPanel;

    private PoolingSystem<Utils.Negotiation> _supplyNegotiationsPool;
    private PoolingSystem<Utils.Negotiation> _demandNegotiationsPool;

    void Awake()
    {
        Instance = this;

        _supplyNegotiationsPool = new PoolingSystem<Utils.Negotiation>(supplyNegotiationsScrollPanel, negotiationItemPrefab,
            InitializeSupplyNegotiation, 10);
        _demandNegotiationsPool = new PoolingSystem<Utils.Negotiation>(demandNegotiationsScrollPanel, negotiationItemPrefab,
            InitializeDemandNegotiation, 10);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetNegotiationsResponseEvent += OnGetNegotiationsResponseReceived;
        EventManager.Instance.OnEditNegotiationCostPerUnitResponseEvent += OnEditNegotiationCostPerUnitResponseReceived;
        EventManager.Instance.OnRejectNegotiationResponseEvent += OnRejectNegotiationResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetNegotiationsResponseEvent -= OnGetNegotiationsResponseReceived;
        EventManager.Instance.OnEditNegotiationCostPerUnitResponseEvent -= OnEditNegotiationCostPerUnitResponseReceived;
        EventManager.Instance.OnRejectNegotiationResponseEvent -= OnRejectNegotiationResponse;
    }

    public void OnGetNegotiationsResponseReceived(GetNegotiationsResponse getNegotiationsResponse)
    {
        _supplyNegotiationItemControllers.Clear();
        _demandNegotiationItemControllers.Clear();
        
        _supplyNegotiationsPool.RemoveAll();
        _demandNegotiationsPool.RemoveAll();
        
        
        List<Utils.Negotiation> supplyNegotiations = new List<Utils.Negotiation>();
        List<Utils.Negotiation> demandNegotiations = new List<Utils.Negotiation>();

        int teamId = PlayerPrefs.GetInt("TeamId");
        foreach(Utils.Negotiation negotiation in getNegotiationsResponse.negotiations)
        {
            if (negotiation.supplierId == teamId)
            {
                supplyNegotiations.Add(negotiation);
            }
            else if (negotiation.demanderId == teamId)
            {
                demandNegotiations.Add( negotiation);
            }
        }

        supplyNegotiations.Reverse();
        demandNegotiations.Reverse();

        for (int i=0;i < supplyNegotiations.Count; i++)
        {
            _supplyNegotiationsPool.Add(supplyNegotiations[i]);
        }
        for (int i = 0; i < demandNegotiations.Count; i++)
        {
            _demandNegotiationsPool.Add(demandNegotiations[i]);
        }
        
        RebuildSupplyNegotiationsLayout();
        RebuildDemandNegotiationsLayout();
    }

    private void OnEditNegotiationCostPerUnitResponseReceived(EditNegotiationCostPerUnitResponse editNegotiationCostPerUnitResponse)
    {
        if (editNegotiationCostPerUnitResponse.negotiation != null)
        {
            foreach (NegotiationItemController controller in _supplyNegotiationItemControllers)
            {
                if (controller.Negotiation.id != editNegotiationCostPerUnitResponse.negotiation.id) continue;
                
                controller.UpdateEditedNegotiation(editNegotiationCostPerUnitResponse.negotiation);
                return;
            }
            foreach (NegotiationItemController controller in _demandNegotiationItemControllers)
            {
                if (controller.Negotiation.id != editNegotiationCostPerUnitResponse.negotiation.id) continue;
                
                controller.UpdateEditedNegotiation(editNegotiationCostPerUnitResponse.negotiation);
                return;
            }
            NotificationsController.Instance.AddNewNotification("notification_edit_negotiation",
                GameDataManager.Instance.GetProductName(editNegotiationCostPerUnitResponse.negotiation.productId));
        }
        else
        {
            if (editNegotiationCostPerUnitResponse.message == "Supplier error")
            {
                DialogManager.Instance.ShowErrorDialog("negotiation_supplier_error");
                return;
            }
            if (editNegotiationCostPerUnitResponse.message == "Demander error")
            {
                DialogManager.Instance.ShowErrorDialog("negotiation_demander_error");
                return;
            }
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    private void OnRejectNegotiationResponse(RejectNegotiationResponse response)
    {
        if (response.negotiation == null || response.negotiation.state != Utils.NegotiationState.CLOSED) return;
        
        int teamId = PlayerPrefs.GetInt("TeamId");
        
        if (response.negotiation.supplierId == teamId)
        {
            for (int i = 0; i < _supplyNegotiationItemControllers.Count; i++)
            {
                var controller = _supplyNegotiationItemControllers[i];
                if (controller.Negotiation.id != response.negotiation.id) continue;
                
                _supplyNegotiationsPool.Remove(controller.gameObject);
                _supplyNegotiationItemControllers.Remove(controller);
                RebuildSupplyNegotiationsLayout();
            }
        }
        else if (response.negotiation.demanderId == teamId)
        {
            for (int i = 0; i < _demandNegotiationItemControllers.Count; i++)
            {
                var controller = _demandNegotiationItemControllers[i];
                if (controller.Negotiation.id != response.negotiation.id) continue;
                
                _demandNegotiationsPool.Remove(controller.gameObject);
                _demandNegotiationItemControllers.Remove(controller);
                RebuildDemandNegotiationsLayout();
            }
        }
    }

    private void InitializeSupplyNegotiation(GameObject theGameObject, int index, Utils.Negotiation negotiation)
    {
        var controller = theGameObject.GetComponent<NegotiationItemController>();
        controller.Initialize(negotiation, true);
        
        _supplyNegotiationItemControllers.Add(controller);
    }
    private void InitializeDemandNegotiation(GameObject theGameObject, int index, Utils.Negotiation negotiation)
    {
        var controller = theGameObject.GetComponent<NegotiationItemController>();
        controller.Initialize(negotiation, false);
        
        _demandNegotiationItemControllers.Add(controller);
    }

    public void RebuildSupplyNegotiationsLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(supplyNegotiationsScrollPanel);
    }
    
    public void RebuildDemandNegotiationsLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(demandNegotiationsScrollPanel);
    }

    public void AddNegotiationToList(Utils.Negotiation negotiation)
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        if (negotiation.supplierId == teamId)
        {
            _supplyNegotiationsPool.Add(0, negotiation);
            RebuildSupplyNegotiationsLayout();
        }
        else if (negotiation.demanderId == teamId)
        {
            _demandNegotiationsPool.Add(0, negotiation);
            RebuildDemandNegotiationsLayout();
        }
    }
}
