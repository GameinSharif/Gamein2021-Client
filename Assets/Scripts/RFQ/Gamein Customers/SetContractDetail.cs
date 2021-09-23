using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class SetContractDetail : MonoBehaviour
{
    [HideInInspector] public RFQUtils.ContractModel ContractData;

    public RTLTextMeshPro RowNumberTxt;
    public RTLTextMeshPro GameinCustomerNameTxt;
    public RTLTextMeshPro ProductNameTxt;
    public Localize ContractTypeLocalize;
    public GameObject TerminateButtonGameObject;

    public void InitializeContract(RFQUtils.ContractModel contractData, int index)
    {
        ContractData = contractData;

        RowNumberTxt.text = index.ToString();
        //TODO
    }

    public void OnShowDetailsButtonClick()
    {

    }

    public void OnTerminateContractButtonClick()
    {

    }
}
