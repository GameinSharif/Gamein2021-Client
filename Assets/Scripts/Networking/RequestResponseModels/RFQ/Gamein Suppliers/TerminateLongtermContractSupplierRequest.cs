using System;
using UnityEngine;

[Serializable]
public class TerminateLongtermContractSupplierRequest : RequestObject
{

    public int contractId;
    
    public TerminateLongtermContractSupplierRequest(RequestTypeConstant requestTypeConstant, int contractId) : base(requestTypeConstant)
    {
        this.contractId = contractId;
    }
}