using System.Collections.Generic;

public class GetGameDataResponse : ResponseObject
{
    public List<RFQUtils.GameinCustomer> gameinCustomers;
    public List<RFQUtils.Product> products;
}
