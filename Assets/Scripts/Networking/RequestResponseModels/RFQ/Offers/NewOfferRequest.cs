using System;
using UnityEngine;

[Serializable]
public class NewOfferRequest : RequestObject
{
    public Utils.Offer offer;

    public NewOfferRequest(RequestTypeConstant requestTypeConstant, Utils.Offer offer) : base(requestTypeConstant)
    {
        this.offer = offer;
    }
}