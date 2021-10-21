using System;

[Serializable]
public class BuyDCRequest : RequestObject
{
    public int dcId;

    public BuyDCRequest(RequestTypeConstant requestTypeConstant, int dcId) : base(requestTypeConstant)
    {
        this.dcId = dcId;
    }
}