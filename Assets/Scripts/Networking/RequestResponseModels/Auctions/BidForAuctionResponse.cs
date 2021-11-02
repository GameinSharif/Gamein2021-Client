using System;

[Serializable]
public class BidForAuctionResponse : ResponseObject
{
    public Utils.Auction auction;
    public string result;
}