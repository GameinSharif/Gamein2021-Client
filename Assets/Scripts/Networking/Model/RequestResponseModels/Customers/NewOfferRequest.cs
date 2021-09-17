using System;
using UnityEngine;

[Serializable]
public class NewOfferRequest : RequestObject
{
    public NewOfferTransitModel offer;

    public NewOfferRequest(RequestTypeConstant requestTypeConstant, NewOfferTransitModel offer)
    {
        this.playerId = PlayerPrefs.GetInt("PlayerId", 0);
        this.requestTypeConstant = Convert.ToInt32(requestTypeConstant);
        this.offer = offer;
    }
}