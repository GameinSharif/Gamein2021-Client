using System;

[Serializable]
public class TerminateLongtermContractSupplierResponse : ResponseObject
{
    public String result;
    public int penalty;
    public int contractId;
}