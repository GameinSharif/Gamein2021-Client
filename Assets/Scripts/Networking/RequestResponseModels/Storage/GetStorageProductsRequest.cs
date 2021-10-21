using System;

[Serializable]
public class GetStorageProductsRequest : RequestObject
{
    public GetStorageProductsRequest(RequestTypeConstant requestTypeConstant) : base(requestTypeConstant)
    {
        
    }
}