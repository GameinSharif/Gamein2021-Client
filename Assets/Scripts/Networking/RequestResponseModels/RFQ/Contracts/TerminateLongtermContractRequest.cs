using System;
using UnityEngine;

[Serializable]
public class TerminateLongtermContractRequest : RequestObject
{

    public int contractId;
    
    public TerminateLongtermContractRequest(RequestTypeConstant requestTypeConstant, int contractId) : base(requestTypeConstant)
    {
        this.contractId = contractId;
    }
}