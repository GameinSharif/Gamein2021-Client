using System.Collections;
using System.Collections.Generic;
using System;
using ProductionLine;

public class Utils
{
    [Serializable]
    public class Team
    {
        public int id;
        public string teamName;
        public Country country;
        public int factoryId;
        public float credit;
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
    public class Contract
    {
        public int id;
        public int teamId;
        public int gameinCustomerId;
        public int productId;
        public CustomDate contractDate;
        public int supplyAmount;
        public float pricePerUnit;
        public int boughtAmount;
        //TODO add parameters needed after calculating shares
        public int lostSalePenalty;
        public int terminatePenalty;
        public bool isTerminated;
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
        public int gameinCustomerId;
        public int productId;
        public int amount;
    }
    
    [Serializable]
    public class Supplier
    {
        public int id;
        public string name;
        public Country country;
        public List<int> materials;
        public double latitude;
        public double longitude;
    }
    
    [Serializable]
    public class WeekSupply
    {
        public int id;
        public int week;
        public int supplierId;
        public int productId;
        public int price;
        public int sales;
    }
    
    [Serializable]
    public class ContractSupplierDetail
    {
        public int id;
        public CustomDate contractDate;
        public int boughtAmount;
        public float pricePerUnit;
        public TransportNodeType transportNodeType;
    }

    [Serializable]
    public class ContractSupplier
    {
        public int id;
        public CustomDate contractDate;
        public int supplierId;
        public int teamId;
        public int materialId;
        public float pricePerUnit;
        public int boughtAmount;
        public VehicleType transportType;
        public bool hasInsurance;
        public float transportationCost;
        public int terminatePenalty;
        public bool isTerminated;
        public int noMoneyPenalty;
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
        public List<ProductIngredient> ingredientsPerUnit; //for SemiFinishedProducts & Finished only except CarbonDioxide (id = 27)
        public int minPrice;
        public int maxPrice;
        public float maintenanceCostPerDay;
    }

    [Serializable]
    public class ProductIngredient
    {
        public int productId;
        public int amount;
    }
    
    [Serializable]
    public class DC
    {
        public int id;
        public int? ownerId;
        public string name;
        public DCType type;
        public double latitude;
        public double longitude;
        public int buyingPrice;
        public int sellingPrice;
        public int capacity;
    }

    public enum DCType
    {
        SemiFinished, Finished
    }

    [Serializable]
    public class Storage
    {
        public int id;
        public int buildingId;
        public bool dc;
        public List<StorageProduct> products;
    }

    [Serializable]
    public class StorageProduct
    {
        public int id;
        public int productId;
        public int amount;
    }
    
    public enum StorageType
    {
        WAREHOUSE = 0,
        DC = 1,
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
        public int rawMaterialCapacity;
        public int semiFinishedProductCapacity;
        public int finishedProductCapacity;
        public float insuranceCostFactor;
        public int distanceConstant;
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
    public class EfficiencyLevel
    {
        public int efficiencyPercentage;
        public int upgradeCost;
    }

    [Serializable]
    public class QualityLevel
    {
        public int upgradeCost;
        public double brandIncreaseRatioPerProduct;
    }

    [Serializable]
    public class ProductionLineTemplate
    {
        public int id;
        public string name;
        public int constructionCost;
        public int constructRequiredDays;
        public int scrapPrice;
        public int batchSize;
        public int dailyProductionRate;
        public List<EfficiencyLevel> efficiencyLevels;
        public int weeklyMaintenanceCost;
        public int setupCost;
        public int productionCostPerOneProduct;
        public List<QualityLevel> qualityLevels;
    }

    public enum VehicleType
    {
        AIRPLANE, TRUCK, TRAIN, VANET
    }

    public class Vehicle
    {
        public int id;
        public VehicleType vehicleType;
        public int speed;
        public int capacity;
        public int costPerKilometer;
        public float coefficient;
    }

    public enum TransportNodeType
    {
        SUPPLIER, GAMEIN_CUSTOMER, DC, FACTORY
    }

    public enum TransportState
    {
        SUCCESSFUL, IN_WAY, CRUSHED, PENDING
    }

    public class Transport
    {
        public int id;
        public VehicleType vehicleType;
        public TransportNodeType sourceType;
        public int sourceId;
        public TransportNodeType destinationType;
        public int destinationId;
        public CustomDate startDate;
        public CustomDate endDate;
        public bool hasInsurance;
        public TransportState transportState;
        public int contentProductId;
        public int contentProductAmount;
    }
}
