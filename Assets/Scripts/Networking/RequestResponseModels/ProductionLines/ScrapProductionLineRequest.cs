using System;

[Serializable]
public class ScarpProductionLineRequest : RequestObject
{
    public int productionLineId;
    
    public ScarpProductionLineRequest(RequestTypeConstant requestTypeConstant, int productionLineId) : base(requestTypeConstant)
    {
        this.productionLineId = productionLineId;
    }
}
