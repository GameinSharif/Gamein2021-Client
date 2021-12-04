using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class BidForAuctionManager : MonoBehaviour
{
    public static BidForAuctionManager Instance;

    public RTLTextMeshPro CurrentRoundText;
    public RTLTextMeshPro RemainedTimeText;

    private void Awake()
    {
        Instance = this;
    }

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
            GameDataManager.Instance.UpdateAuctionElement(bidForAuctionResponse.auction);
        }
        else
        {
            if (MapManager.IsInMap)
            {
                DialogManager.Instance.ShowErrorDialog();
            }
        }
    }

    public IEnumerator AuctionTimer()
    {
        while (GameDataManager.Instance.AuctionCurrentRound < GameDataManager.Instance.GameConstants.AuctionRoundsStartTime.Count)
        {
            int remainedTimeInSeconds = (int) GameDataManager.Instance.GameConstants.AuctionRoundsStartTime[GameDataManager.Instance.AuctionCurrentRound].ToDateTime().AddSeconds(GameDataManager.Instance.GameConstants.AuctionRoundDurationSeconds).Subtract(DateTime.Now).TotalSeconds;
            
            CurrentRoundText.text = (GameDataManager.Instance.AuctionCurrentRound + 1) + "/" + GameDataManager.Instance.GameConstants.AuctionRoundsStartTime.Count;
            
            while (remainedTimeInSeconds > 0)
            {
                RemainedTimeText.text = remainedTimeInSeconds.ToString();

                yield return new WaitForSeconds(1);

                remainedTimeInSeconds--;
            }

            yield return null;
        }
    }
    
}