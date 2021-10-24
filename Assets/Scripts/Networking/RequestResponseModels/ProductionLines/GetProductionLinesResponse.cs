using System;
using System.Collections.Generic;

[Serializable]
public class GetProductionLinesResponse : ResponseObject
{
    private List<Utils.ProductionLine> productionLines;
}