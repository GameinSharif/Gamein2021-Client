using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class OffersController : MonoBehaviour
{
    public static OffersController Instance;

    [HideInInspector] public List<Utils.Offer> MyTeamOffers;
    [HideInInspector] public List<Utils.Offer> OtherTeamsOffers;

    public GameObject offerItemPrefab;

    public GameObject MyTeamOffersScrollViewParent;
    public GameObject OtherTeamsOffersScrollViewParent;

    private List<GameObject> _spawnedGameObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetOffersResponseEvent += OnGetOffersResponseReceived;
        EventManager.Instance.OnTerminateOfferResponseEvent += OnTerminateOfferResponseRecieved;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetOffersResponseEvent -= OnGetOffersResponseReceived;
        EventManager.Instance.OnTerminateOfferResponseEvent -= OnTerminateOfferResponseRecieved;
    }

    public void OnGetOffersResponseReceived(GetOffersResponse getOffersResponse)
    {
        MyTeamOffers = getOffersResponse.myTeamOffers;
        OtherTeamsOffers = getOffersResponse.otherTeamsOffers;

        MyTeamOffers.Reverse();
        OtherTeamsOffers.Reverse();

        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < MyTeamOffers.Count; i++)
        {
            AddMyOfferToList(MyTeamOffers[i], i + 1);
        }
        for (int i = 0; i < OtherTeamsOffers.Count; i++)
        {
            AddOtherOfferToList(OtherTeamsOffers[i], i + 1);
        }
    }

    public void AddMyOfferToList(Utils.Offer offer)
    {
        MyTeamOffers.Add(offer);
        AddMyOfferToList(offer, MyTeamOffers.Count);
    }

    private void AddMyOfferToList(Utils.Offer offer, int index)
    {
        GameObject createdItem = GetItem(MyTeamOffersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        OfferItemController controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(index, offer);

        createdItem.SetActive(true);
    }

    private void AddOtherOfferToList(Utils.Offer offer, int index)
    {
        GameObject createdItem = GetItem(OtherTeamsOffersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        OfferItemController controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(index, offer);

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

        GameObject newItem = Instantiate(offerItemPrefab, parent.transform);
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

    private void OnTerminateOfferResponseRecieved(TerminateOfferResponse terminateOfferResponse)
    {
        foreach (Utils.Offer offer in MyTeamOffers)
        {
            if (offer.id == terminateOfferResponse.terminatedOfferId)
            {
                offer.offerStatus = Utils.OfferStatus.TERMINATED;
            }
        }
    }

    public void OnRefreshButtonClicked()
    {
        RequestManager.Instance.SendRequest(new GetOffersRequest(RequestTypeConstant.GET_OFFERS));
    }
}
