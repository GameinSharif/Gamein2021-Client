
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
            int playerId = PlayerPrefs.GetInt("PlayerId");
            if (bidForAuctionResponse.auction.highestBidTeamId == playerId) //TODO teamId not playerId
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