using System;
using UnityEngine;

[Serializable]
public class GetContractSuppliersRequest : RequestObject
{
    public GetContractSuppliersRequest(RequestTypeConstant requestTypeConstant) : base(requestTypeConstant)
    {}
}