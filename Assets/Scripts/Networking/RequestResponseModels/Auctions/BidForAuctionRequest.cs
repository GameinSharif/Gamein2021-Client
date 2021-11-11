using System;
using UnityEngine;

[Serializable]
public class BidForAuctionRequest : RequestObject
{
    public int factoryId;
    public int raiseAmount;

    public BidForAuctionRequest(RequestTypeConstant requestTypeConstant, int factoryId, int raiseAmount) : base(requestTypeConstant)
    {
        this.factoryId = factoryId;
        this.raiseAmount = raiseAmount;
    }
}
