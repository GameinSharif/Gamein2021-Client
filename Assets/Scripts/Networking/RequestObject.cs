using System;
using UnityEngine;

[Serializable]
public class RequestObject
{
    public static string myToken;

    public string token;
    public int requestTypeConstant;

    public RequestObject(RequestTypeConstant requestTypeConstant)
    {
        this.token = myToken;
        this.requestTypeConstant = Convert.ToInt32(requestTypeConstant);
    }
}