using System;

[Serializable]
public class UpgradeProductionLineEfficiencyRequest : RequestObject
{
    public int productionLineId;

    public UpgradeProductionLineEfficiencyRequest(RequestTypeConstant requestTypeConstant, int productionLineId) : base(requestTypeConstant)
    {
        this.productionLineId = productionLineId;
    }
}