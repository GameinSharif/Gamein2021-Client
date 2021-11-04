using System;
using System.Collections.Generic;

[Serializable]
public class GetContractSupplierResponse : ResponseObject
{
    public List<Utils.ContractSupplier> ContractSuppliers;
}