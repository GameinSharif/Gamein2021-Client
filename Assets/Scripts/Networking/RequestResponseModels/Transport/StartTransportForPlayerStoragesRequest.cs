public class StartTransportForPlayerStoragesRequest : RequestObject
{
    public int sourceId;
    public string sourceType;
    public int destinationId;
    public string destinationType;
    public int productId;
    public int amount;
    public bool hasInsurance;
    public string vehicleType;

    public StartTransportForPlayerStoragesRequest(int sourceId, Utils.TransportNodeType sourceType, int destinationId,
        Utils.TransportNodeType destinationType, int productId, int amount, bool hasInsurance,
        Utils.VehicleType vehicleType) : base(RequestTypeConstant.TRANSPORT_TO_STORAGE)
    {
        this.sourceId = sourceId;
        this.sourceType = sourceType.ToString();
        this.destinationId = destinationId;
        this.destinationType = destinationType.ToString();
        this.productId = productId;
        this.amount = amount;
        this.hasInsurance = hasInsurance;
        this.vehicleType = vehicleType.ToString();
    }
}