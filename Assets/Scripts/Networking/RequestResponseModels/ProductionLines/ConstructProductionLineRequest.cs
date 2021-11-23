using System;

[Serializable]
public class ConstructProductionLineRequest : RequestObject
{
    public int productionLineTemplateId;
    
    public ConstructProductionLineRequest(RequestTypeConstant requestTypeConstant, int productionLineTemplateId) : base(requestTypeConstant)
    {
        this.productionLineTemplateId = productionLineTemplateId;
    }
}
