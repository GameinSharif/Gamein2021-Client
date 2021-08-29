using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

[Serializable]
public class RequestObject<T>
{
    public int PlayerId;
    public RequestTypeConstant RequestType;
    public T RequestData;

    public RequestObject(int playerId, RequestTypeConstant requestType, T requestData)
    {
        PlayerId = playerId;
        RequestType = requestType;
        RequestData = requestData;
    }
}