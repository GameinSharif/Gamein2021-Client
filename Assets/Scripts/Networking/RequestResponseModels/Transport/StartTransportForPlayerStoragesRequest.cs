using System;

public class StartTransportForPlayerStoragesRequest : RequestObject
{
    public int sourceId;
    public int sourceType;
    public int destinationId;
    public int destinationType;
    public int productId;
    public int amount;
    public bool hasInsurance;
    public string vehicleType;

    public StartTransportForPlayerStoragesRequest(int sourceId, Utils.TransportNodeType sourceType, int destinationId,
        Utils.TransportNodeType destinationType, int productId, int amount, bool hasInsurance,
        Utils.VehicleType vehicleType) : base(RequestTypeConstant.TRANSPORT_TO_STORAGE)
    {
        this.sourceId = sourceId;
        this.sourceType = Convert.ToInt32(sourceType);
        this.destinationId = destinationId;
        this.destinationType = Convert.ToInt32(destinationType);
        this.productId = productId;
        this.amount = amount;
        this.hasInsurance = hasInsurance;
        this.vehicleType = vehicleType.ToString();
    }
}