﻿using System;
using UnityEngine;

[Serializable]
public class NewContractSupplierRequest : RequestObject
{
    public int supplierId;
    public int materialId;
    //private ContractSupplierDto contractSupplierDto;
    public Utils.VehicleType vehicleType;
    public bool hasInsurance;
    public int amount;
    public int weeks;

    public NewContractSupplierRequest(RequestTypeConstant requestTypeConstant, Utils.WeekSupply weekSupply, int weeks, int amount, Utils.VehicleType vehicleType) : base(requestTypeConstant)
    {
        supplierId = weekSupply.supplier.id;
        materialId = weekSupply.productId;
        hasInsurance = true; //TODO what is it
        this.amount = amount;
        this.weeks = weeks;
        this.vehicleType = vehicleType;
    }
}