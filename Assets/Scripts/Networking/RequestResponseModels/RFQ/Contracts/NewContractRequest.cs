using System;
using UnityEngine;

[Serializable]
public class NewContractRequest : RequestObject
{
    public int gameinCustomerId;
    public int storageId;
    public int productId;
    public int amount;
    public float pricePerUnit;
    public int weeks;
    
    public NewContractRequest(RequestTypeConstant requestTypeConstant, int gameinCustomerId, int storageId, int productId, int amount, float pricePerUnit, int weeks) : base(requestTypeConstant)
    {
        this.gameinCustomerId = gameinCustomerId;
        this.storageId = storageId;
        this.productId = productId;
        this.amount = amount;
        this.pricePerUnit = pricePerUnit;
        this.weeks = weeks;
    }
}