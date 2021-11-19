using System;
using System.Collections.Generic;

[Serializable]
public class NewContractSupplierResponse : ResponseObject
{
    public List<Utils.ContractSupplier> contractSuppliers;
    public float price;
    public String result;
}