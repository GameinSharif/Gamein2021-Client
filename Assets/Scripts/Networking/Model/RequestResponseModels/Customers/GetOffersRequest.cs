using System;
using UnityEngine;

[Serializable]
public class GetOffersRequest : RequestObject
{
    public GetOffersRequest(RequestTypeConstant requestTypeConstant) : base(requestTypeConstant)
    {}
}