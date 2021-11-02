using System.Collections.Generic;
using System;

[Serializable]
public class GetAllAuctionsResponse : ResponseObject
{
    public List<Utils.Auction> auctions;
}