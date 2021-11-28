using System;

[Serializable]
public class ProductCreationCompletedResponse : ResponseObject
{
    public Utils.ProductionLineProductDto product;
    public Utils.ProductionLineDto productLine;
}
