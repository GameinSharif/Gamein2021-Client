using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProviderRequest: RequestObject
{
    public int productId;
    public int capacity;
    public float price;

    public NewProviderRequest(RequestTypeConstant requestTypeConstant, int productId, int capacity, float price) : base(requestTypeConstant)
    {
        this.productId = productId;
        this.capacity = capacity;
        this.price = price;
    }
}
