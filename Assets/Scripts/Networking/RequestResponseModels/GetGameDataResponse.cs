using System.Collections.Generic;
using System;

[Serializable]
public class GetGameDataResponse : ResponseObject
{
    public List<Utils.Team> teams;
    public List<Utils.GameinCustomer> gameinCustomers;
    public List<Utils.Product> products;
    public List<Utils.DC> dcDtos;
    public List<Utils.Factory> factories;
    public List<Utils.Supplier> suppliers;
    public List<Utils.Vehicle> vehicles;
    
    public Utils.GameConstants gameConstants;
    public List<Utils.ProductionLineTemplate> productionLineTemplates;
}
