using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class CustomersController : MonoBehaviour
{
    public GameObject offerItemPrefab;
    public Transform scrollPanel;

    public TMP_InputField volumeInput;
    public TMP_InputField costPerUnitInput;
    public TMP_InputField inputField3;
    public TMP_Dropdown dropdown;

    public GameObject newOfferPopUp;

    public DatePicker eea;
    public DatePicker lea;
    public DatePicker deadline;

    public RTLTextMeshPro[] dates;

    void Awake()
    {
        // DestroyAllChildrenInScrollPanel();
        SetActivePopup(false);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetOffersResponseEvent += OnGetOffersResponseReceived;
        EventManager.Instance.OnNewOfferResponseEvent += OnNewOfferResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetOffersResponseEvent -= OnGetOffersResponseReceived;
        EventManager.Instance.OnNewOfferResponseEvent -= OnNewOfferResponseReceived;
    }

    public void AddToList(OfferViewModel offerViewModel)
    {
        var createdItem = Instantiate(offerItemPrefab, scrollPanel);
        var controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(scrollPanel.transform.childCount + 1, offerViewModel);
    }

    private void DestroyAllChildrenInScrollPanel()
    {
        foreach (Transform child in scrollPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnPlaceOfferClicked()
    {
        int volume = Convert.ToInt32(volumeInput.text);
        int costPerUnit = Convert.ToInt32(costPerUnitInput.text);

        if (volume < 0 || costPerUnit < 0)
        {
            //TODO show error
            Debug.Log("Negative input");
            return;
        }

        
        var offer = new NewOfferTransitModel(
            type: dropdown.options[dropdown.value].text,
            volume: volume,
            costPerUnit: costPerUnit,
            earliestExpectedArrival: new CustomDateTime(eea.Value),
            latestExpectedArrival: new CustomDateTime(lea.Value),
            offerDeadline: new CustomDateTime(deadline.Value)
        );
        var request = new NewOfferRequest(RequestTypeConstant.NEW_OFFER, offer);
        
        RequestManager.Instance.SendRequest(request);
    }

    public void OnNewOfferButtonClicked()
    {
        SetActivePopup(true);
    }

    public void OnPopupCloseClicked()
    {
        SetActivePopup(false);
    }

    private void SetActivePopup(bool value)
    {
        newOfferPopUp.SetActive(value);
    }

    public void OnRefreshButtonClicked()
    {
        RequestManager.Instance.SendRequest(new GetOffersRequest(RequestTypeConstant.GET_OFFERS));
    }

    public void OnGetOffersResponseReceived(GetOffersResponse response)
    {
        //TODO refresh the list
    }

    public void OnNewOfferResponseReceived(NewOfferResponse response)
    {
        //TODO show visual feedback
    }
}
