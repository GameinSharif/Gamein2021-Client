using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEditor;

public class AuctionController : MonoBehaviour
{
    public GameObject auction;
    public GameObject bidHigherGameObject;
    public GameObject hasHighestBidGameObject;
    public RTLTextMeshPro highestBidAmount;
    
    public void SetAuctionPanelActive()
    {
        auction.SetActive(true);
    }
    
    public void OnCloseButtonClicked()
    {
        auction.SetActive(false);
    }

    public void SetAuctionValues(int highestBid, MapUtils.MapAgentMarker.AgentType agentType)
    {
        highestBidAmount.text = highestBid + " $";
        
        if (agentType == MapUtils.MapAgentMarker.AgentType.NoOwnerFactory)
        {
            bidHigherGameObject.SetActive(true);
            hasHighestBidGameObject.SetActive(false);
        } else if (agentType == MapUtils.MapAgentMarker.AgentType.OtherFactory)
        {
            bidHigherGameObject.SetActive(false);
            hasHighestBidGameObject.SetActive(true);
        }
    }
    
    public void OnBidButtonClicked()
    {
        int bidingAmount = Convert.ToInt32(highestBidAmount.text);
        BidHigherRequest bidHigherRequest = new BidHigherRequest(RequestTypeConstant.BID_HIGHER, bidingAmount);
        RequestManager.Instance.SendRequest(bidHigherRequest);
    }
}
