using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProviderNegotiationRequest : RequestObject
{
    public int providerId;
    public int amount;
    public float costPerUnitDemander;

    public NewProviderNegotiationRequest(RequestTypeConstant requestTypeConstant, int providerId, int amount, float costPerUnit) : base(requestTypeConstant)
    {
        this.providerId = providerId;
        this.amount = amount;
        this.costPerUnitDemander = costPerUnit;
    }
}
