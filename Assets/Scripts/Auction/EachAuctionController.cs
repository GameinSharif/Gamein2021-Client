using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class EachAuctionController : MonoBehaviour
{
    public GameObject auctionGameObject;
    public GameObject bidHigherGameObject;
    public GameObject hasHighestBidGameObject;
    public RTLTextMeshPro highestBidAmount;
    public Localize highestBidAmountLocalize;
    public Localize minRaiseAmount;
    public TMP_InputField raiseAmountInputFiled;

    private bool _isClickable = false;
    private Utils.Auction _auction;
    private MapUtils.OnMapMarker _onMapMarker;
    private bool _isSendingRequest = false;

    public void OnFactoryButtonClick()
    {
        if (_isClickable)
        {
            auctionGameObject.SetActive(!auctionGameObject.activeSelf);
        }
    }

    public void SetAuctionValues(Utils.Auction auction, MapUtils.OnMapMarker onMapMarker)
    {
        _isSendingRequest = false;
        if (onMapMarker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.DifferentCountryFactory || (auction != null && auction.auctionBidStatus == Utils.AuctionBidStatus.Over))
        {
            _isClickable = false;
            auctionGameObject.SetActive(false);

            return;
        }

        _isClickable = true;
        _auction = auction;
        _onMapMarker = onMapMarker;

        if (onMapMarker.MapAgentMarker.MapAgentType == MapUtils.MapAgentMarker.AgentType.NoOwnerFactory)
        {
            highestBidAmountLocalize.SetKey("auction_no_bid");
        }
        else
        {
            highestBidAmountLocalize.SetKey("");
            highestBidAmount.text = auction.highestBid.ToString();
        }     

        if (HasHighestBid())
        {
            bidHigherGameObject.SetActive(false);
            hasHighestBidGameObject.SetActive(true);
        }
        else
        {
            hasHighestBidGameObject.SetActive(false);

            if (auction != null)
            {
                minRaiseAmount.SetKey("auction_min_raise", auction.lastRaiseAmount.ToString());
            }
            else
            {
                minRaiseAmount.SetKey("auction_min_raise", GameDataManager.Instance.GameConstants.AuctionStartValue.ToString());
            }

            bidHigherGameObject.SetActive(true);
        }

        //TODO set all needed values for auction game object
    }

    public bool HasHighestBid()
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        List<Utils.Auction> auctions = GameDataManager.Instance.Auctions;

        return auctions.Exists(a => a.highestBidTeamId == teamId);
    }
    
    
    public void OnBidButtonClicked()
    {
        if (_isSendingRequest)
        {
            return;
        }
        _isSendingRequest = true;

        string raise = raiseAmountInputFiled.text;
        if (string.IsNullOrEmpty(raise))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        int raiseAmount = int.Parse(raise);
        if ((_auction == null && raiseAmount < GameDataManager.Instance.GameConstants.AuctionStartValue) || (_auction != null && raiseAmount < _auction.lastRaiseAmount))
        {
            DialogManager.Instance.ShowErrorDialog("auction_not_enough_money_error");
            return;
        }

        int factoryId = _onMapMarker.Index;
        BidForAuctionRequest bidHigherRequest = new BidForAuctionRequest(RequestTypeConstant.BID_FOR_AUCTION, factoryId, raiseAmount);
        RequestManager.Instance.SendRequest(bidHigherRequest);
    }
}
