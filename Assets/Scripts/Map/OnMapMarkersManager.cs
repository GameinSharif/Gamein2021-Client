
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnMapMarkersManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.OnBidForAuctionResponseEvent += OnChangeMapMarkerResponseReceive;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBidForAuctionResponseEvent -= OnChangeMapMarkerResponseReceive;
    }
    
    public void OnChangeMapMarkerResponseReceive(BidForAuctionResponse changeMapMarkerResponse)
    {
        if (changeMapMarkerResponse.result == "Successful")
        {
            int playerId = PlayerPrefs.GetInt("PlayerId");
            if (changeMapMarkerResponse.bidderPlayerId == playerId)
            {
                //TODO show feedback for successfully biding higher
            }
            //TODO set the new values for the changed on map marker
        }
        else
        {
            //TODO show feedback for the unsuccessful bid
        }
    }
    
}