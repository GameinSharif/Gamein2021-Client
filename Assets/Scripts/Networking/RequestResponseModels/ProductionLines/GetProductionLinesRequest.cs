using System;

[Serializable]
public class GetProductionLinesRequest : RequestObject
{
    public GetProductionLinesRequest(RequestTypeConstant requestTypeConstant) : base(requestTypeConstant)
    {
    }
}