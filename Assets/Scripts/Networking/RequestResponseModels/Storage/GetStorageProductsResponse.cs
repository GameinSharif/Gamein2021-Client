using System;
using System.Collections.Generic;

[Serializable]
public class GetStorageProductsResponse : ResponseObject
{
    public List<Utils.Storage> storages;
    public string result;
}