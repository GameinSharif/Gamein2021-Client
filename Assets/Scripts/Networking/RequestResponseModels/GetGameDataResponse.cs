using System.Collections.Generic;
using System;

[Serializable]
public class GetGameDataResponse : ResponseObject
{
    public List<Utils.Team> teams;
    public List<Utils.GameinCustomer> gameinCustomers;
    public List<Utils.Product> products;
    public List<Utils.Factory> factories;
    public List<Utils.Supplier> suppliers;

    public Utils.GameConstants gameConstants;
}
