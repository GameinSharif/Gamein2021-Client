using System;
using UnityEngine;

[Serializable]
public class NewContractSupplierRequest : RequestObject
{
    public int supplierId;
    public int materialId;
    public int vehicleId;
    public bool hasInsurance;
    public int amount;
    public int weeks;

    public NewContractSupplierRequest(RequestTypeConstant requestTypeConstant, Utils.WeekSupply weekSupply, int weeks, int amount, int vehicleId) : base(requestTypeConstant)
    {
        supplierId = weekSupply.supplierId;
        materialId = weekSupply.productId;
        hasInsurance = true; //TODO what is it
        this.amount = amount;
        this.weeks = weeks;
        this.vehicleId = vehicleId;
    }
}