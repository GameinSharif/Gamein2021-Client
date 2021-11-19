using System;

[Serializable]
public class ProductCreationCompletedResponse : ResponseObject
{
    public Utils.ProductionLineProductDto product;
    public int productLineId;
}
