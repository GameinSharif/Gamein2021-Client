using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEditor;

public class EachAuctionController : MonoBehaviour
{
    public GameObject auctionGameObject;
    public GameObject bidHigherGameObject;
    public GameObject hasHighestBidGameObject;
    public RTLTextMeshPro highestBidAmount;
    public Utils.Auction auction;
    

    public void SetAuctionPanelActive()
    {
        auctionGameObject.SetActive(true);
    }
    
    public void OnCloseButtonClicked()
    {
        auctionGameObject.SetActive(false);
    }

    public void SetAuctionValues(Utils.Auction auction, MapUtils.OnMapMarker onMapMarker)
    {
        this.auction = auction;

        if (auction.auctionBidStatus == Utils.AuctionBidStatus.Over)
        {
            //TODO deactivate the on map marker onclick function
            return;
        }
        
        highestBidAmount.text = auction.highestBid + " $";
        
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
        int factoryId = auction.factoryId;
        BidForAuctionRequest bidHigherRequest = new BidForAuctionRequest(RequestTypeConstant.BID_FOR_AUCTION, factoryId);
        RequestManager.Instance.SendRequest(bidHigherRequest);
    }
}
