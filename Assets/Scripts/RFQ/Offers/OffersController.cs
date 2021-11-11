using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class OffersController : MonoBehaviour
{
    public static OffersController Instance;

    public GameObject offerItemPrefab;

    public GameObject MyTeamOffersScrollViewParent;
    public GameObject OtherTeamsOffersScrollViewParent;
    public GameObject AcceptedOffersScrollViewParent;

    private List<OfferItemController> _myTeamOfferItemControllers = new List<OfferItemController>();
    private List<OfferItemController> _otherTeamsOfferItemControllers = new List<OfferItemController>();
    private List<OfferItemController> _acceptedOfferItemControllers = new List<OfferItemController>();
    private List<GameObject> _spawnedGameObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetOffersResponseEvent += OnGetOffersResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetOffersResponseEvent -= OnGetOffersResponseReceived;
    }

    public void OnGetOffersResponseReceived(GetOffersResponse getOffersResponse)
    {
        List<Utils.Offer> myTeamOffers = getOffersResponse.myTeamOffers;
        List<Utils.Offer> otherTeamsOffers = getOffersResponse.otherTeamsOffers;
        List<Utils.Offer> acceptedOffers = getOffersResponse.acceptedOffersByMyTeam;

        myTeamOffers.Reverse();
        otherTeamsOffers.Reverse();
        acceptedOffers.Reverse();

        _myTeamOfferItemControllers.Clear();
        _otherTeamsOfferItemControllers.Clear();
        _acceptedOfferItemControllers.Clear();
        
        DeactiveAllChildrenInScrollPanel();

        for (int i = 0; i < myTeamOffers.Count; i++)
        {
            AddMyOfferToList(myTeamOffers[i], i + 1);
        }
        for (int i = 0; i < otherTeamsOffers.Count; i++)
        {
            AddOtherOfferToList(otherTeamsOffers[i], i + 1);
        }

        for (int i = 0; i < acceptedOffers.Count; i++)
        {
            AddAcceptedOfferToList(acceptedOffers[i], i + 1);
        }
    }

    public void AddMyOfferToList(Utils.Offer offer)
    {
        AddMyOfferToList(offer, _myTeamOfferItemControllers.Count + 1);
    }

    private void AddMyOfferToList(Utils.Offer offer, int index)
    {
        GameObject createdItem = GetItem(MyTeamOffersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        OfferItemController controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(index, offer);

        _myTeamOfferItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private void AddOtherOfferToList(Utils.Offer offer, int index)
    {
        GameObject createdItem = GetItem(OtherTeamsOffersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        OfferItemController controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(index, offer);

        _otherTeamsOfferItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private void AddAcceptedOfferToList(Utils.Offer offer, int index)
    {
        GameObject createdItem = GetItem(AcceptedOffersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);
        
        OfferItemController controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(index, offer);
        
        _acceptedOfferItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    public void AddAcceptedOfferToList(Utils.Offer offer)
    {
        AddAcceptedOfferToList(offer, _acceptedOfferItemControllers.Count + 1);
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

    public void OnRefreshButtonClicked()
    {
        RequestManager.Instance.SendRequest(new GetOffersRequest(RequestTypeConstant.GET_OFFERS));
    }
}
