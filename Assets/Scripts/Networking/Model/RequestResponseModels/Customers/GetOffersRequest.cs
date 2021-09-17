using System;
using UnityEngine;

[Serializable]
public class GetOffersRequest : RequestObject
{
    public GetOffersRequest(RequestTypeConstant requestTypeConstant)
    {
        this.playerId = PlayerPrefs.GetInt("PlayerId", 0);
        this.requestTypeConstant = Convert.ToInt32(requestTypeConstant);
    }
}