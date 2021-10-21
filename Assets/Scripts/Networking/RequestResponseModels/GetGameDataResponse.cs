using System.Collections.Generic;
using System;

[Serializable]
public class GetGameDataResponse : ResponseObject
{
    public List<Utils.GameinCustomer> gameinCustomers;
    public List<Utils.Product> products;
}
