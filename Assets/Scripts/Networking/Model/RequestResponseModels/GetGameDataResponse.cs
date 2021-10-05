using System.Collections.Generic;
using System;

[Serializable]
public class GetGameDataResponse : ResponseObject
{
    public List<RFQUtils.GameinCustomer> gameinCustomers;
    public List<RFQUtils.Product> products;
}
