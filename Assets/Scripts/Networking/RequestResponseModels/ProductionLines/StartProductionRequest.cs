using System;

[Serializable]
public class StartProductionRequest : RequestObject
{
    public int productionLineId;
    public int productId;
    public int amount;

    public StartProductionRequest(RequestTypeConstant requestTypeConstant, int productionLineId, int productId, int amount) : base(requestTypeConstant)
    {
        this.productionLineId = productionLineId;
        this.productId = productId;
        this.amount = amount;
    }
}
