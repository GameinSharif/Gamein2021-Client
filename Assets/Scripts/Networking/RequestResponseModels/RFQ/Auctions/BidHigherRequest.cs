using System;
using UnityEngine;

[Serializable]
public class BidHigherRequest : RequestObject
{
    public int bidingAmount;

    public BidHigherRequest(RequestTypeConstant requestTypeConstant, int bidingAmount) : base(requestTypeConstant)
    {
        this.bidingAmount = bidingAmount;
    }
}
