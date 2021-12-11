using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;
using UnityEngine.UI;

public class OffersController : MonoBehaviour
{
    public static OffersController Instance;

    private List<MyOfferItemController> _myTeamOfferItemControllers = new List<MyOfferItemController>();
    private List<OtherOfferItemController> _otherTeamsOfferItemControllers = new List<OtherOfferItemController>();
    private List<OtherOfferItemController> _acceptedOfferItemControllers = new List<OtherOfferItemController>();
    
    public GameObject otherOfferItemPrefab;
    public GameObject myOfferItemPrefab;

    public RectTransform otherOffersScrollPanel;
    public RectTransform myOffersScrollPanel;
    public RectTransform acceptedOffersScrollPanel;

    private PoolingSystem<Utils.Offer> _otherOffersPool;
    private PoolingSystem<Utils.Offer> _myOffersPool;
    private PoolingSystem<Utils.Offer> _acceptedOffersPool;

    public GameObject otherOffersTitle;
    public GameObject otherOffersScrollView;
    public GameObject acceptedOffersTitle;
    public GameObject acceptedOffersScrollView;

    public TMP_InputField searchBar;

    void Awake()
    {
        Instance = this;
        
        _otherOffersPool = new PoolingSystem<Utils.Offer>(otherOffersScrollPanel, otherOfferItemPrefab, InitializeOtherOfferItem);
        _myOffersPool = new PoolingSystem<Utils.Offer>(myOffersScrollPanel, myOfferItemPrefab, InitializeMyOfferItem);
        _acceptedOffersPool = new PoolingSystem<Utils.Offer>(acceptedOffersScrollPanel, otherOfferItemPrefab, InitializeAcceptedOfferItem);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetOffersResponseEvent += OnGetOffersResponseReceived;
        EventManager.Instance.OnTerminateOfferResponseEvent += OnTerminateOfferResponse;
        EventManager.Instance.OnAcceptOfferResponseEvent += OnAcceptOfferResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetOffersResponseEvent -= OnGetOffersResponseReceived;
        EventManager.Instance.OnTerminateOfferResponseEvent -= OnTerminateOfferResponse;
        EventManager.Instance.OnAcceptOfferResponseEvent -= OnAcceptOfferResponse;
    }

    private void OnGetOffersResponseReceived(GetOffersResponse getOffersResponse)
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
        
        _myOffersPool.RemoveAll();
        _otherOffersPool.RemoveAll();
        _acceptedOffersPool.RemoveAll();
        
        for (int i = 0; i < myTeamOffers.Count; i++)
        {
            if (myTeamOffers[i].offerStatus == Utils.OfferStatus.TERMINATED ||
                myTeamOffers[i].offerStatus == Utils.OfferStatus.PASSED_DEADLINE) continue;
            _myOffersPool.Add(myTeamOffers[i]);
        }

        for (int i = 0; i < otherTeamsOffers.Count; i++)
        {
            if (otherTeamsOffers[i].offerStatus == Utils.OfferStatus.TERMINATED ||
                otherTeamsOffers[i].offerStatus == Utils.OfferStatus.PASSED_DEADLINE) continue;
            _otherOffersPool.Add(otherTeamsOffers[i]);
        }

        for (int i = 0; i < acceptedOffers.Count; i++)
        {
            _acceptedOffersPool.Add(acceptedOffers[i]);
        }
        
        RebuildListLayout(myOffersScrollPanel);
        RebuildListLayout(otherOffersScrollPanel);
        RebuildListLayout(acceptedOffersScrollPanel);
    }

    private void OnTerminateOfferResponse(TerminateOfferResponse response)
    {
        for (int i = 0; i < _myTeamOfferItemControllers.Count; i++)
        {
            var controller = _myTeamOfferItemControllers[i];
            if (controller.Offer.id != response.terminatedOfferId) continue;
            
            string productName = GameDataManager.Instance.GetProductName(controller.Offer.productId);
            string translatedProductName =
                LocalizationManager.GetLocalizedValue("product_" + productName,
                    LocalizationManager.GetCurrentLanguage());
            NotificationsController.Instance.AddNewNotification("notification_terminate_offer",translatedProductName);

            _myOffersPool.Remove(controller.gameObject);
            _myTeamOfferItemControllers.Remove(controller);
            RebuildListLayout(myOffersScrollPanel);
            return;
        }
    }

    private void OnAcceptOfferResponse(AcceptOfferResponse response)
    {
        if (response.acceptedOffer == null)
        {
            if (response.message == "The Offer Placer Team doesn't have enough money!")
            {
                DialogManager.Instance.ShowErrorDialog("offer_accept_demander_error");
                return;
            }
            DialogManager.Instance.ShowErrorDialog();
            return;
        }
        
        if (response.acceptedOffer.teamId != PlayerPrefs.GetInt("TeamId"))
        {
            for (int i = 0; i < _otherTeamsOfferItemControllers.Count; i++)
            {
                var controller = _otherTeamsOfferItemControllers[i];
                if (controller.Offer.id != response.acceptedOffer.id) continue;
            
                _acceptedOffersPool.Add(response.acceptedOffer);
            
                _otherOffersPool.Remove(controller.gameObject);
                _otherTeamsOfferItemControllers.Remove(controller);
                RebuildListLayout(otherOffersScrollPanel);
                break;
            }
            
            MainHeaderManager.Instance.Money += (int)(response.acceptedOffer.volume * response.acceptedOffer.costPerUnit);
        }
        else
        {
            for (int i = 0; i < _myTeamOfferItemControllers.Count; i++)
            {
                var controller = _myTeamOfferItemControllers[i];
                if (controller.Offer.id != response.acceptedOffer.id) continue;
                
                string productName = GameDataManager.Instance.GetProductName(response.acceptedOffer.productId);
                string translatedProductName =
                    LocalizationManager.GetLocalizedValue("product_" + productName,
                        LocalizationManager.GetCurrentLanguage());
                NotificationsController.Instance.AddNewNotification("notification_offer_accepted",translatedProductName);
                
                controller.SetAsAccepted(response.acceptedOffer);
                break;
            }
            MainHeaderManager.Instance.Money -= (int)(response.acceptedOffer.volume * response.acceptedOffer.costPerUnit);
        }
    }

    private void RebuildListLayout(RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
    public void OnRefreshButtonClicked()
    {
        RequestManager.Instance.SendRequest(new GetOffersRequest(RequestTypeConstant.GET_OFFERS));
    }

    private void InitializeOtherOfferItem(GameObject theGameObject, int index, Utils.Offer offer)
    {
        var controller = theGameObject.GetComponent<OtherOfferItemController>();
        controller.Initialize(offer, false);
        _otherTeamsOfferItemControllers.Add(controller);
        
        RebuildListLayout(otherOffersScrollPanel);
    }

    private void InitializeAcceptedOfferItem(GameObject theGameObject, int index, Utils.Offer offer)
    {
        var controller = theGameObject.GetComponent<OtherOfferItemController>();
        controller.Initialize(offer, true);
        _acceptedOfferItemControllers.Add(controller);
    }

    private void InitializeMyOfferItem(GameObject theGameObject, int index, Utils.Offer offer)
    {
        var controller = theGameObject.GetComponent<MyOfferItemController>();
        controller.Initialize(offer);
        _myTeamOfferItemControllers.Add(controller);
    }

    public void AcceptedOffersToggleOnValueChanged(bool value)
    {
        acceptedOffersTitle.SetActive(value);
        otherOffersTitle.SetActive(!value);
        acceptedOffersScrollView.SetActive(value);
        otherOffersScrollView.SetActive(!value);
    }

    public void AddMyOfferToList(Utils.Offer offer)
    {
        _myOffersPool.Add(0, offer);
        RebuildListLayout(myOffersScrollPanel);
    }

    public void OnSearchBarValueChanged(string query)
    {
        query ??= "";

        query = LocalizationManager.GetCurrentLanguage() == LocalizationManager.LocalizedLanguage.Farsi &&
                StringUtils.IsAlphaNumeric(query)
            ? StringUtils.Reverse(query)
            : query;
        
        foreach (var controller in _otherTeamsOfferItemControllers)
        {
            controller.gameObject.SetActive(
                controller.team.text.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                controller.product.GetLocalizedString().value.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0
            );
        }
        
        RebuildListLayout(otherOffersScrollPanel);
    }
}
