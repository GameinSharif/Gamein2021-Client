using System.Collections;
using System.Collections.Generic;
using System;
using ProductionLine;
using QualityLevel = ProductionLine.QualityLevel;

public class Utils
{
    [Serializable]
    public class Team
    {
        public int id;
        public string teamName;
        public Country country;
        public int factoryId;
    }

    public enum ProviderState
    {
        ACTIVE,
        TERMINATED
    }

    [Serializable]
    public class Provider
    {
        public int id;
        public int teamId;
        public int productId;
        public int capacity;
        public float price;
        public ProviderState state;
    }

    public enum OfferStatus
    {
        ACTIVE,
        ACCEPTED,
        TERMINATED,
        PASSED_DEADLINE
    }

    [Serializable]
    public class Offer
    {
        public int id;
        public int teamId;
        public int productId;
        public int volume;
        public OfferStatus offerStatus;
        public float costPerUnit;
        public CustomDate offerDeadline;
    }

    public enum NegotiationState
    {
        CLOSED,
        DEAL,
        IN_PROGRESS
    }

    [Serializable]
    public class Negotiation
    {
        public int id;
        public int demanderId;
        public int supplierId;
        public int productId;
        public int amount;
        public float costPerUnitDemander;
        public float costPerUnitSupplier;
        public NegotiationState state;
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

        public List<ProductIngredient>
            ingredientsPerUnit; //for SemiFinishedProducts & Finished only except CarbonDioxide (id = 36)
    }

    [Serializable]
    public class ProductIngredient
    {
        public int productId;
        public int amount;
    }

    public enum Country
    {
        France,
        Germany,
        UnitedKingdom,
        Netherlands,
        Belgium,
        Switzerland
    }

    [Serializable]
    public class Factory
    {
        public int id;
        public string name;
        public Country country;
        public double latitude;
        public double longitude;
    }

    public enum AuctionBidStatus
    {
        Active,
        Over
    }

    public class Auction
    {
        public int id;
        public int factoryId;
        public int highestBid;
        public int highestBidTeamId;
        public int lastRaiseAmount;
        public AuctionBidStatus auctionBidStatus;
    }

    public class GameConstants
    {
        public int AuctionStartValue;
        public int AuctionInitialStepValue;
        public int AuctionRoundDurationSeconds;
        public List<CustomDateTime> AuctionRoundsStartTime;
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
    public class ProductionLineProductDto
    {
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