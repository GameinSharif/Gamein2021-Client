public class AcceptOfferRequest : RequestObject
{

    public int offerId;
    
    public AcceptOfferRequest(int offerId) : base(RequestTypeConstant.ACCEPT_OFFER)
    {
        this.offerId = offerId;
    }
}