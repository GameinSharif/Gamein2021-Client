
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BidForAuctionManager : MonoBehaviour
{
    public GameObject bidResultPopUp;
    public GameObject successfulResult;
    public GameObject unsuccessfulResult;
    
    private void OnEnable()
    {
        EventManager.Instance.OnBidForAuctionResponseEvent += OnBidForAuctionResponseReceive;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnBidForAuctionResponseEvent -= OnBidForAuctionResponseReceive;
    }

    private void OnBidForAuctionResponseReceive(BidForAuctionResponse bidForAuctionResponse)
    {
        if (bidForAuctionResponse.result == "success")
        {
            int teamId = PlayerPrefs.GetInt("TeamId");
            if (bidForAuctionResponse.auction.highestBidTeamId == teamId)
            {
                if (SceneManager.GetActiveScene().name == "MapScene")
                {
                    successfulResult.SetActive(true);
                    unsuccessfulResult.SetActive(false);
                    bidResultPopUp.SetActive(true);
                }
            }
            GameDataManager.Instance.UpdateAuctionElement(bidForAuctionResponse.auction);
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "MapScene")
            {
                successfulResult.SetActive(false);
                unsuccessfulResult.SetActive(true);
                bidResultPopUp.SetActive(true);
            }
        }
    }
    
}