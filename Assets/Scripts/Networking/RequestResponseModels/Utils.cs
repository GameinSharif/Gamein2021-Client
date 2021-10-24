using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProductionLine;
using QualityLevel = ProductionLine.QualityLevel;


public class Utils
{
    public class Team
    {
        public int id;
        public string teamName;
        //public Country country;
        public int factoryId;
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

    public enum ProductType
    {
        RawMaterial,
        SemiFinished,
        Finished
    }

    [Serializable]
    public class Product
    {
        public int id;
        public string categoryIds; //for SemiFinishedProducts only
        public int productionLineId; //for SemiFinishedProducts & Finished only
        public ProductType productType;
        public string name;
        public int volumetricUnit;
        public List<ProductIngredient> ingredientsPerUnit; //for SemiFinishedProducts & Finished only except CarbonDioxide (id = 36)
    }

    [Serializable]
    public class ProductIngredient
    {
        public int productId;
        public int amount;
    }
    
    [Serializable]
    public class ProductionLine
    {
        private int id;
        private int productionLineTemplateId;
        private int teamId;
        private List<Product> products;
        private int qualityLevel;
        private int efficiencyLevel;
        private ProductionLineStatus status;
    }
    
    [Serializable]
    public class ProductionLineTemplate
    {
        private int id;
        private string name;
        private int constructionCost;
        private int scrapPrice;
        private int batchSize;
        private int dailyProductionRate;
        private List<EfficiencyLevel> efficiencyLevels;
        private int weeklyMaintenanceCost;
        private int setupCost;
        private int productionCostPerOneProduct;
        private List<QualityLevel> qualityLevels;
    }
}
