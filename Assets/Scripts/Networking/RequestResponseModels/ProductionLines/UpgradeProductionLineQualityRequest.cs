using System;

[Serializable]
public class UpgradeProductionLineQualityRequest : RequestObject
{
    public int productionLineId;

    public UpgradeProductionLineQualityRequest(RequestTypeConstant requestTypeConstant, int productionLineId) : base(requestTypeConstant)
    {
        this.productionLineId = productionLineId;
    }
}