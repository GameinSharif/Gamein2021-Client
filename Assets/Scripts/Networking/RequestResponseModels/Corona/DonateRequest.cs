using System;

[Serializable]
public class DonateRequest : RequestObject
{
    public float donatedAmount;
    
    public DonateRequest(RequestTypeConstant requestTypeConstant, float donatedAmount) : base(requestTypeConstant)
    {
        this.donatedAmount = donatedAmount;
    }
}