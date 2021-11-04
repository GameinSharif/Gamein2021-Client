using System;
using System.Collections.Generic;

[Serializable]
public class GetContractSuppliersResponse : ResponseObject
{
    public List<Utils.ContractSupplier> ContractSuppliers;
}