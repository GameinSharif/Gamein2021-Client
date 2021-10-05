using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFQUtils
{
    public enum ContractType
    {
        ONCE,
        LONGTERM
    }

    public class ContractDetail
    {
        public int id;
        public CustomDateTime contractDate;
        public int maxAmount;
        public int boughtAmount;
        public int pricePerUnit;
        public int lostSalePenalty;
    }

    public class ContractModel
    {
        public int id;
        public GameinCustomer gameinCustomer;
        public int productId;
        public ContractType contractType;
        public List<ContractDetail> contractDetails;
        public int terminatePenalty;
    }

    public class GameinCustomer
    {
        public int id;
        public string name;
        public double latitude;
        public double longitude;
    }

    public class Product
    {
        public int id;
        public int categoryId;
        public int productionLineId;
        public string name;
        public List<ProductIngredient> ingredientsPerUnit;
    }

    public class ProductIngredient
    {
        public int productId;
        public int amount;
    }
}
