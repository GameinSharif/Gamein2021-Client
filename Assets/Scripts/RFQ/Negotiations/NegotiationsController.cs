using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class NegotiationsController : MonoBehaviour
{
    public GameObject negotiationOfferItemPrefab;
    public Transform scrollPanel;
    
    public GameObject negotiationSelectedOfferItemPrefab;
    public Transform offerPanel;

    public GameObject sendNewOfferPopUp;
    public TMP_InputField offerPopUpInputField1;
    public TMP_InputField offerPopUpInputField2;
    public TMP_InputField offerPopUpInputField3;
    public TMP_Dropdown offerPopUpDropdown;
    public DatePicker date1;
    public DatePicker date2;
    public DatePicker date3;
    public RTLTextMeshPro[] dates;

    
    private void Awake()
    {
        DestroySelectedOfferInPanel();
        DestroyAllChildrenInScrollPanel();
        TestFunc();
        SetOfferPopUpActive(false);
    }

    public void TestFunc()
    {
        OfferViewModel offer = new OfferViewModel("test", "T3", 2000, 5, 10000, "09/11/2021",
            "09/12/2021", "20/11/2021", Frequency.MONTHLY, State.INPROGRESS);
        AddToList(offer);
        
        OfferViewModel offer2 = new OfferViewModel("bhkbkk", "T32", 500, 5, 10000, "09/11/2021",
            "09/12/2021", "20/11/2021", Frequency.ONCE, State.INPROGRESS);
        AddToList(offer2);
        
        OfferViewModel offer3 = new OfferViewModel("kbllbjlb", "T", 2, 5, 10000, "09/11/2021",
            "09/12/2021", "20/11/2021", Frequency.MONTHLY, State.DEAL);
        AddToList(offer3);
        
        OfferViewModel offer4 = new OfferViewModel("kbllbjlb", "T", 2, 5, 10000, "09/11/2021",
            "09/12/2021", "20/11/2021", Frequency.MONTHLY, State.DEAL);
        AddToList(offer4);
        
        OfferViewModel offer5 = new OfferViewModel("kbllbjlb", "T", 2, 5, 10000, "09/11/2021",
            "09/12/2021", "20/11/2021", Frequency.MONTHLY, State.DEAL);
        AddToList(offer5);
        
        OfferViewModel offer6 = new OfferViewModel("kbllbjlb", "T", 2, 5, 10000, "09/11/2021",
            "09/12/2021", "20/11/2021", Frequency.MONTHLY, State.DEAL);
        AddToList(offer6);
    }
    
    public void AddToList(OfferViewModel offer)
    {
        var createdItem = Instantiate(negotiationOfferItemPrefab, scrollPanel);
        var controller = createdItem.GetComponent<NegotiationOfferItemController>();
        controller.SetInfo(scrollPanel.transform.childCount, offer);
        RectTransform createdItemRectTransform = createdItem.GetComponent<RectTransform>();
        float height = -123.3697f;
        createdItemRectTransform.anchoredPosition = new Vector2(0, (float) scrollPanel.transform.childCount * height);
        createdItem.gameObject.SetActive(true);
    }

    private void DestroyAllChildrenInScrollPanel()
    {
        foreach (Transform child in scrollPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void ShowSelectedOffer(OfferViewModel offer)
    {
        DestroySelectedOfferInPanel();
        var createdItem = Instantiate(negotiationSelectedOfferItemPrefab, offerPanel);
        var controller = createdItem.GetComponent<NegotiationOfferItemController>();
        controller.SetInfo(offer);
    }

    private void DestroySelectedOfferInPanel()
    {
        foreach (Transform child in offerPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void OnSendOfferButtonClicked()
    {
        SetOfferPopUpActive(true);
    }

    public void OnOfferPopUpCloseClicked()
    {
        SetOfferPopUpActive(false);
    }

    private void SetOfferPopUpActive(bool value)
    {
        sendNewOfferPopUp.SetActive(value);
    }
    
    public void OnPlaceOfferButtonClicked()
    {
        //TODO
    }
}
