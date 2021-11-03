using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnMapMarkerHandler : MonoBehaviour
{
    private void OnMouseDown()
    {
        //TODO conditions for all map marker types
        GetComponentInParent<EachAuctionController>().OnFactoryButtonClick();
        GetComponentInParent<EachDcController>().OnDcMarkerClicked();
    }
}
