using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class NegotiationsController : MonoBehaviour
{
    public static NegotiationsController Instance;

    public GameObject NegotiationItemPrefab;

    public GameObject SupplyNegotiationsScrollViewParent;
    public GameObject DemandNegotiationsScrollViewParent;

    private List<NegotiationItemController> _supplyNegotiationItemControllers = new List<NegotiationItemController>();
    private List<NegotiationItemController> _demandNegotiationItemControllers = new List<NegotiationItemController>();
    private List<GameObject> _spawnedGameObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetNegotiationsResponseEvent += OnGetNegotiationsResponseReceived;
        EventManager.Instance.OnEditNegotiationCostPerUnitResponseEvent += OnEditNegotiationCostPerUnitResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetNegotiationsResponseEvent -= OnGetNegotiationsResponseReceived;
        EventManager.Instance.OnEditNegotiationCostPerUnitResponseEvent -= OnEditNegotiationCostPerUnitResponseReceived;
    }

    public void OnGetNegotiationsResponseReceived(GetNegotiationsResponse getNegotiationsResponse)
    {
        _supplyNegotiationItemControllers.Clear();
        _demandNegotiationItemControllers.Clear();
        DeactiveAllChildrenInScrollPanel();

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
            AddSupplyNegotiationToList(supplyNegotiations[i], i + 1);
        }
        for (int i = 0; i < demandNegotiations.Count; i++)
        {
            AddDemandNegotiationToList(demandNegotiations[i], i + 1);
        }
    }

    private void OnEditNegotiationCostPerUnitResponseReceived(EditNegotiationCostPerUnitResponse editNegotiationCostPerUnitResponse)
    {
        if (editNegotiationCostPerUnitResponse.negotiation != null)
        {
            foreach (NegotiationItemController negotiationItemController in _supplyNegotiationItemControllers)
            {
                negotiationItemController.OnEditNegotiationCostPerUnitResponseReceived(editNegotiationCostPerUnitResponse.negotiation);
            }
            foreach (NegotiationItemController negotiationItemController in _demandNegotiationItemControllers)
            {
                negotiationItemController.OnEditNegotiationCostPerUnitResponseReceived(editNegotiationCostPerUnitResponse.negotiation);
            }
        }
        else
        {
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void AddNegotiationToList(Utils.Negotiation negotiation)
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        if (negotiation.supplierId == teamId)
        {
            AddSupplyNegotiationToList(negotiation, _supplyNegotiationItemControllers.Count);
        }
        else if (negotiation.demanderId == teamId)
        {
            AddDemandNegotiationToList(negotiation, _demandNegotiationItemControllers.Count);
        }
    }

    private void AddSupplyNegotiationToList(Utils.Negotiation negotiation, int index)
    {
        GameObject createdItem = GetItem(SupplyNegotiationsScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        NegotiationItemController controller = createdItem.GetComponent<NegotiationItemController>();
        controller.SetSupplyNegotiationInfo(index, negotiation);

        _supplyNegotiationItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private void AddDemandNegotiationToList(Utils.Negotiation negotiation, int index)
    {
        GameObject createdItem = GetItem(DemandNegotiationsScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        NegotiationItemController controller = createdItem.GetComponent<NegotiationItemController>();
        controller.SetDemandNegotiationInfo(index, negotiation);

        _demandNegotiationItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem = Instantiate(NegotiationItemPrefab, parent.transform);
        _spawnedGameObjects.Add(newItem);
        return newItem;
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
}
