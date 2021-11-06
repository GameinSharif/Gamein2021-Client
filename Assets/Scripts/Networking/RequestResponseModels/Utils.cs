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
        public int productionLineTemplateId; //for SemiFinishedProducts & Finished only
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
    public class ProductionLineDto
    {
        public int id;
        public int productionLineTemplateId;
        public int teamId;
        public List<ProductionLineProductDto> products;
        public int qualityLevel;
        public int efficiencyLevel;
        public ProductionLineStatus status;
    }
    
    [Serializable]
    public class ProductionLineProductDto {
        public int id;
        public int productId;
        public int amount;
        public CustomDate startDate;
        public CustomDate endDate;
    }
    
    [Serializable]
    public class ProductionLineTemplate
    {
        public int id;
        public string name;
        public int constructionCost;
        public int scrapPrice;
        public int batchSize;
        public int dailyProductionRate;
        public List<EfficiencyLevel> efficiencyLevels;
        public int weeklyMaintenanceCost;
        public int setupCost;
        public int productionCostPerOneProduct;
        public List<QualityLevel> qualityLevels;
    }
}
