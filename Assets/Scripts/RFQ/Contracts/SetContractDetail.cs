using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class SetContractDetail : MonoBehaviour
{
    [HideInInspector] public Utils.Contract ContractData;

    public RTLTextMeshPro RowNumberTxt;
    public RTLTextMeshPro GameinCustomerNameTxt;
    public RTLTextMeshPro ProductNameTxt;
    public Localize ContractTypeLocalize;
    public GameObject TerminateButtonGameObject;

    public void InitializeContract(Utils.Contract contractData, int index)
    {
        ContractData = contractData;

        RowNumberTxt.text = (index + 1).ToString();
        GameinCustomerNameTxt.text = contractData.gameinCustomer.name;
        ProductNameTxt.text = GameDataManager.Instance.Products[contractData.productId].name;
        ContractTypeLocalize.SetKey("contract_type_" + contractData.contractType.ToString().ToLower());

        if (contractData.contractType == Utils.ContractType.LONGTERM)
        {
            TerminateButtonGameObject.SetActive(true);
        }
        else
        {
            TerminateButtonGameObject.SetActive(false);
        }
    }

    public void OnShowDetailsButtonClick()
    {
        //TODO
    }

    public void OnTerminateContractButtonClick()
    {
        //TODO
    }
}
