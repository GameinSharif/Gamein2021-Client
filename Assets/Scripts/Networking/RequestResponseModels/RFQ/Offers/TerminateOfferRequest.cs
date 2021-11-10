using System;

[Serializable]
public class TerminateOfferRequest : RequestObject
{
    public int offerId;

    public TerminateOfferRequest(RequestTypeConstant requestTypeConstant, int offerId) : base(requestTypeConstant)
    {
        this.offerId = offerId;
    }
}