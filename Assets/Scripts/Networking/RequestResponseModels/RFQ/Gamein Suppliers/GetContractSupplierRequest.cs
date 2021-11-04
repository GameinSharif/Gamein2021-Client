using System;
using UnityEngine;

[Serializable]
public class GetContractSupplierRequest : RequestObject
{
    public GetContractSupplierRequest(RequestTypeConstant requestTypeConstant) : base(requestTypeConstant)
    {}
}