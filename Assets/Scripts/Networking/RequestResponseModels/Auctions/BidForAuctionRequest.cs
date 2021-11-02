using System;
using UnityEngine;

[Serializable]
public class BidForAuctionRequest : RequestObject
{
    public int factoryId;

    public BidForAuctionRequest(RequestTypeConstant requestTypeConstant, int factoryId) : base(requestTypeConstant)
    {
        this.factoryId = factoryId;
    }
}
