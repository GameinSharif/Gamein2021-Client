using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Utils
{
    public class Team
    {
        public int id;
        public string teamName;
        public double latitude;
        public double longitude;
    }

    [Serializable]
    public class Provider
    {
        public int id;
        public Team team;
        public int productId;
        public int capacity;
        public int averagePrice;
        public int minPriceOnRecord;
        public int maxPriceOnRecord;
    }

    public enum ContractType
    {
        ONCE,
        LONGTERM
    }

    [Serializable]
    public class ContractDetail
    {
        public int id;
        public CustomDateTime contractDate;
        public int maxAmount;
        public int boughtAmount;
        public int pricePerUnit;
        public int lostSalePenalty;
    }

    [Serializable]
    public class Contract
    {
        public int id;
        public GameinCustomer gameinCustomer;
        public int productId;
        public ContractType contractType;
        public List<ContractDetail> contractDetails;
        public int terminatePenalty;
    }

    [Serializable]
    public class GameinCustomer
    {
        public int id;
        public string name;
        public double latitude;
        public double longitude;
    }

    [Serializable]
    public class WeekDemand
    {
        public int id;
        public int week;
        public GameinCustomer gameinCustomer;
        public int productId;
        public int amount;
    }

    [Serializable]
    public class Product
    {
        public int id;
        public int categoryId;
        public int productionLineId;
        public string name;
        public List<ProductIngredient> ingredientsPerUnit;
    }

    [Serializable]
    public class ProductIngredient
    {
        public int productId;
        public int amount;
    }
    
    [Serializable]
    public class DCDto
    {
        public int DCId;
        public int? ownerTeamId;
        public string name;
        public DCType type;
        public double latitude;
        public double longitude;
        public int purchasePrice;
        public int sellPrice;
    }

    public enum DCType
    {
        MIDDLE, FINAL
    }

    [Serializable]
    public class Storage
    {
        public StorageType type;
        public int DCId;
        public List<StorageProduct> storageProducts;
    }

    [Serializable]
    public class StorageProduct
    {
        public int productId;
        public int amount;
    }
    
    public enum StorageType
    {
        WAREHOUSE, DC
    }

}
