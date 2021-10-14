using System;
using UnityEngine;

[Serializable]
public class RequestObject
{
    public int playerId;
    public int requestTypeConstant;

    public RequestObject(RequestTypeConstant requestTypeConstant)
    {
        this.playerId = PlayerPrefs.GetInt("PlayerId", 0);
        this.requestTypeConstant = Convert.ToInt32(requestTypeConstant);
    }
}