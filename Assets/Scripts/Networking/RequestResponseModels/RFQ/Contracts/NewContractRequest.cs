using System;
using UnityEngine;

[Serializable]
public class NewContractRequest : RequestObject
{
    public int gameinCustomerId;
    public int productId;
    public int amount;
    public float pricePerUnit;
    public int weeks;
    
    public NewContractRequest(RequestTypeConstant requestTypeConstant, int gameinCustomerId, int productId, int amount, float pricePerUnit, int weeks) : base(requestTypeConstant)
    {
        this.gameinCustomerId = gameinCustomerId;
        this.productId = productId;
        this.amount = amount;
        this.pricePerUnit = pricePerUnit;
        this.weeks = weeks;
    }
}