using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class CustomersController : MonoBehaviour
{
    public GameObject offerItemPrefab;
    public Transform scrollPanel;

    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;
    public TMP_Dropdown dropdown;

    public GameObject newOfferPopUp;

    public DatePicker date1;
    public DatePicker date2;
    public DatePicker date3;

    public RTLTextMeshPro[] dates;

    void Awake()
    {
        // DestroyAllChildrenInScrollPanel();
        SetActivePopup(false);
    }

    public void AddToList(Offer offer)
    {
        var createdItem = Instantiate(offerItemPrefab, scrollPanel);
        var controller = createdItem.GetComponent<OfferItemController>();
        controller.SetInfo(scrollPanel.transform.childCount + 1, offer);
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
        //TODO send offer to server
        Debug.Log("Place offer clicked");
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
}
