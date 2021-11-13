using System;

[Serializable]
public class SellDCRequest : RequestObject
{
    public int dcId;

    public SellDCRequest(RequestTypeConstant requestTypeConstant, int dcId) : base(requestTypeConstant)
    {
        this.dcId = dcId;
    }
}