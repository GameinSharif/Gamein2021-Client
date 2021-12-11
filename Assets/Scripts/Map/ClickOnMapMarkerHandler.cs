using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOnMapMarkerHandler : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        //TODO conditions for all map marker types
        GetComponentInParent<EachAuctionController>().OnFactoryButtonClick();
        GetComponentInParent<EachDcController>().OnDcMarkerClicked();
    }
}
