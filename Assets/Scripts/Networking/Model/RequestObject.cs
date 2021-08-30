using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

[Serializable]
public class RequestObject
{
    public int playerId;
    public RequestTypeConstant requestTypeConstant;
    public object requestData;

    public RequestObject(int playerId, RequestTypeConstant requestTypeConstant, object requestData)
    {
        this.playerId = playerId;
        this.requestTypeConstant = requestTypeConstant;
        this.requestData = requestData;
    }
}