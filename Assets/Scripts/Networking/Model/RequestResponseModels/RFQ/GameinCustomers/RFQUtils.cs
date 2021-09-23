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
        public CustomDateTime contractDate;
        public int amount;
        public int pricePerUnit;
    }

    public class ContractModel
    {
        public int contractIndex;
        public int gameinCustomerIndex;
        public int categoryIndex;
        public int productIndex;
        public ContractType contractType;
        public List<ContractDetail> contractDetails;
    }
}
