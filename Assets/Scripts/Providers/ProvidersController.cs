using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class ProvidersController : MonoBehaviour
{
    public GameObject providerItemPrefab;
    public Transform scrollPanel;
    
    public GameObject placeAsProviderPopUp;
    public TMP_InputField providerPopUpInputField1;
    public RTLTextMeshPro providerPopUpTextField1;
    public RTLTextMeshPro providerPopUpTextField2;
    public RTLTextMeshPro providerPopUpTextField3;

    public GameObject newOfferPopUp;
    public TMP_InputField offerPopUpInputField1;
    public TMP_InputField offerPopUpInputField2;
    public TMP_InputField offerPopUpInputField3;
    public TMP_Dropdown offerPopUpDropdown;
    public DatePicker date1;
    public DatePicker date2;
    public DatePicker date3;

    private void Awake()
    {
        DestroyAllChildrenInScrollPanel();
        SetOfferPopUpActive(false);
        SetProviderPopUpActive(false);
        testFunc();
    }

    private void testFunc()
    {
        Provider provider = new Provider("abck0", "T2", 500, 2.5,
            1.2, 5);
        AddToList(provider);
        
        Provider provider2 = new Provider("lefwN", "T2", 500, 2.5,
            1.2, 5);
        AddToList(provider2);
        
        Provider provider3 = new Provider("KNCjascnkivehq", "T2", 500, 2.5,
            1.2, 5);
        AddToList(provider3);
    }
    
    public void AddToList(Provider provider)
    {
        var createdItem = Instantiate(providerItemPrefab, scrollPanel);
        var controller = createdItem.GetComponent<ProviderItemController>();
        controller.SetInfo(scrollPanel.transform.childCount, provider);
        RectTransform createdItemRectTransform = createdItem.GetComponent<RectTransform>();
        float height = -97.5916f;
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

    public void OnPlaceYourselfAsProviderClicked()
    {
        SetProviderPopUpActive(true);
    }

    public void OnProviderPopUpCloseClicked()
    {
        SetProviderPopUpActive(false);
    }

    private void SetProviderPopUpActive(bool value)
    {
        placeAsProviderPopUp.SetActive(value);
    }

    public void OnPlaceProviderButtonClicked()
    {
        //TODO
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
        newOfferPopUp.SetActive(value);
    }
    
    public void OnPlaceOfferButtonClicked()
    {
        //TODO
    }
}
