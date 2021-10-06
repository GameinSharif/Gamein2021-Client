using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GetContractsResponse : ResponseObject
{
    public List<RFQUtils.ContractModel> contracts;
}
