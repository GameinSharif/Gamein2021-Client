using System.Collections;
using System.Collections.Generic;
using System;

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

    [Serializable]
    public class Provider
    {
        public int id;
        public int teamId;
        public int productId;
        public int capacity;
        public float price;
    }

    public enum OfferStatus
    {
        ACTIVE, ACCEPTED, TERMINATED, PASSED_DEADLINE
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
        CLOSED, DEAL, IN_PROGRESS, PENDING
    }

    [Serializable]
    public class Negotiation
    {
        public int id;
        public int demanderTeamId;
        public int supplierTeamId;
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
        public AuctionBidStatus auctionBidStatus;
    }

    public class GameConstants
    {
        public int AuctionStartValue = 1000;
        public int AuctionStepValue = 100;
        public List<CustomDateTime> AuctionRoundsStartTime;
    }
}
