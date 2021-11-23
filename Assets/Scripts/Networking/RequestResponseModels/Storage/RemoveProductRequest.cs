using System;

[Serializable]
public class RemoveProductRequest : RequestObject
{
    public bool isDc;
    public int buildingId;
    public int productId;
    public int amount;

    public RemoveProductRequest(RequestTypeConstant requestTypeConstant, bool isDc, int buildingId, int productId, int amount) : base(requestTypeConstant)
    {
        this.isDc = isDc;
        this.buildingId = buildingId;
        this.productId = productId;
        this.amount = amount;
    }
}