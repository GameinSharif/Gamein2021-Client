public class StartTransportForPlayerStoragesRequest : RequestObject
{
    public int sourceId;
    public Utils.TransportNodeType sourceType;
    public int destinationId;
    public Utils.TransportNodeType destinationType;
    public int productId;
    public int amount;
    public bool hasInsurance;
    public Utils.VehicleType vehicleType;

    public StartTransportForPlayerStoragesRequest(int sourceId, Utils.TransportNodeType sourceType, int destinationId,
        Utils.TransportNodeType destinationType, int productId, int amount, bool hasInsurance,
        Utils.VehicleType vehicleType) : base(RequestTypeConstant.START_TRANSPORT_FOR_STORAGES)
    {
        this.sourceId = sourceId;
        this.sourceType = sourceType;
        this.destinationId = destinationId;
        this.destinationType = destinationType;
        this.productId = productId;
        this.amount = amount;
        this.hasInsurance = hasInsurance;
        this.vehicleType = vehicleType;
    }
}