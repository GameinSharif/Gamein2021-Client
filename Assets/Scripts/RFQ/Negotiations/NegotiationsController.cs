using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class NegotiationsController : MonoBehaviour
{
    public static NegotiationsController Instance;

    [HideInInspector] public List<Utils.Negotiation> SupplyNegotiations;
    [HideInInspector] public List<Utils.Negotiation> DemandNegotiations;

    public GameObject NegotiationItemPrefab;

    public GameObject SupplyNegotiationsScrollViewParent;
    public GameObject DemandNegotiationsScrollViewParent;

    private List<GameObject> _spawnedGameObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetNegotiationsResponseEvent += OnGetNegotiationsResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetNegotiationsResponseEvent -= OnGetNegotiationsResponseReceived;
    }

    public void OnGetNegotiationsResponseReceived(GetNegotiationsResponse getNegotiationsResponse)
    {
        DeactiveAllChildrenInScrollPanel();
        SupplyNegotiations = new List<Utils.Negotiation>();
        DemandNegotiations = new List<Utils.Negotiation>();

        int teamId = PlayerPrefs.GetInt("TeamId");
        foreach(Utils.Negotiation negotiation in getNegotiationsResponse.negotiations)
        {
            if (negotiation.supplierId == teamId)
            {
                SupplyNegotiations.Add(negotiation);
            }
            else if (negotiation.demanderId == teamId)
            {
                DemandNegotiations.Add( negotiation);
            }
        }

        SupplyNegotiations.Reverse();
        DemandNegotiations.Reverse();

        for (int i=0;i < SupplyNegotiations.Count; i++)
        {
            AddSupplyNegotiationToList(SupplyNegotiations[i], i + 1);
        }
        for (int i = 0; i < DemandNegotiations.Count; i++)
        {
            AddDemandNegotiationToList(DemandNegotiations[i], i + 1);
        }
    }

    private void AddSupplyNegotiationToList(Utils.Negotiation negotiation, int index)
    {
        GameObject createdItem = GetItem(SupplyNegotiationsScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        NegotiationItemController controller = createdItem.GetComponent<NegotiationItemController>();
        controller.SetSupplyNegotiationInfo(index, negotiation);

        createdItem.SetActive(true);
    }

    private void AddDemandNegotiationToList(Utils.Negotiation negotiation, int index)
    {
        GameObject createdItem = GetItem(DemandNegotiationsScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        NegotiationItemController controller = createdItem.GetComponent<NegotiationItemController>();
        controller.SetDemandNegotiationInfo(index, negotiation);

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
