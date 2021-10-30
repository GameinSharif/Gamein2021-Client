using System;
using System.Collections.Generic;

[Serializable]
public class GetProductionLinesResponse : ResponseObject
{
    public List<Utils.ProductionLineDto> productionLines;
}