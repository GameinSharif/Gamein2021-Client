using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetContractsRequest : RequestObject
{
    public GetContractsRequest(RequestTypeConstant requestTypeConstant)
    {
        this.playerId = PlayerPrefs.GetInt("PlayerId", 0);
        this.requestTypeConstant = Convert.ToInt32(requestTypeConstant);
    }
}
