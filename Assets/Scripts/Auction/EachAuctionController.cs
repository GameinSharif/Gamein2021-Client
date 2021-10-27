using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEditor;
using UnityEngine.UI;

public class EachAuctionController : MonoBehaviour
{
    public GameObject auctionGameObject;
    public GameObject bidHigherGameObject;
    public GameObject hasHighestBidGameObject;
    public RTLTextMeshPro highestBidAmount;
    public Button factoryButton;
    private Utils.Auction _auction;

    public void OnFactoryButtonClicked()
    {
        auctionGameObject.SetActive(!auctionGameObject.activeSelf);
    }
    
    public void OnCloseButtonClicked()
    {
        auctionGameObject.SetActive(false);
    }

    public void SetAuctionValues(Utils.Auction auction, MapUtils.OnMapMarker onMapMarker)
    {
        _auction = auction;
        if (auction.auctionBidStatus == Utils.AuctionBidStatus.Over ||  onMapMarker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory) 
        {
            factoryButton.interactable = false;
            return;
        }

        if (onMapMarker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.NoOwnerFactory)
        {
            highestBidAmount.text = "minimum amount" + " $"; //TODO get minimum from constants
        }
        else
        {
            highestBidAmount.text = auction.highestBid + " $";
        }
        
        if (hasHighestBid())
        {
            bidHigherGameObject.SetActive(false);
            hasHighestBidGameObject.SetActive(true);
        }
        else
        {
            bidHigherGameObject.SetActive(true);
            hasHighestBidGameObject.SetActive(false);
        }
        
        //TODO set all needed values for auction game object
    }

    public bool hasHighestBid()
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        List<Utils.Auction> auctions = GameDataManager.Instance.Auctions;

        return auctions.Exists(a => a.highestBidTeamId == teamId);
    }
    
    
    public void OnBidButtonClicked()
    {
        int factoryId = _auction.factoryId;
        BidForAuctionRequest bidHigherRequest = new BidForAuctionRequest(RequestTypeConstant.BID_FOR_AUCTION, factoryId);
        RequestManager.Instance.SendRequest(bidHigherRequest);
    }
}
