using System;

[Serializable]
public class RejectNegotiationRequest : RequestObject
{
    public int negotiationId;

    public RejectNegotiationRequest(int negotiationId) : base(RequestTypeConstant.REJECT_NEGOTIATION)
    {
        this.negotiationId = negotiationId;
    }
}