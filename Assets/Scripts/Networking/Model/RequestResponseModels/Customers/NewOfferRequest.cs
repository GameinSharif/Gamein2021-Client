using System;
using UnityEngine;

[Serializable]
public class NewOfferRequest : RequestObject
{
    public NewOfferTransitModel offer;

    public NewOfferRequest(RequestTypeConstant requestTypeConstant, NewOfferTransitModel offer) : base(requestTypeConstant)
    {
        this.offer = offer;
    }
}