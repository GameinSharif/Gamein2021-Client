using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditNegotiationCostPerUnitRequest : RequestObject
{
    public int negotiationId;
    public float newCostPerUnit;

    public EditNegotiationCostPerUnitRequest(RequestTypeConstant requestTypeConstant, int negotiationId, float newCostPerUnit) : base(requestTypeConstant)
    {
        this.negotiationId = negotiationId;
        this.newCostPerUnit = newCostPerUnit;
    }
}
