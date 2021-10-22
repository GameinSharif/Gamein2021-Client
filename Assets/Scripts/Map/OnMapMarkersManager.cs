
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnMapMarkersManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.OnBidForAuctionResponseEvent += OnBidForAuctionResponseReceive;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBidForAuctionResponseEvent -= OnBidForAuctionResponseReceive;
    }
    
    public void OnBidForAuctionResponseReceive(BidForAuctionResponse bidForAuctionResponse)
    {
        if (bidForAuctionResponse.result == "success")
        {
            int teamId = PlayerPrefs.GetInt("TeamId");
            if (bidForAuctionResponse.auction.highestBidTeamId == teamId)
            {
                //TODO show feedback for successfully biding higher
            }
            GameDataManager.Instance.UpdateAuctionElement(bidForAuctionResponse.auction);
            MapManager.Instance.UpdateAllOnMapMarkers();
        }
        else
        {
            //TODO show feedback for the unsuccessful bid
        }
    }
    
}