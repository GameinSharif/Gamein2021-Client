using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractMoreDetailsController : MonoBehaviour
{
    public static ContractMoreDetailsController Instance;
    public static Utils.Contract contractData;

    public GameObject detailsCanvasGameObject;

    public RTLTextMeshPro currentBrand;
    public RTLTextMeshPro valueShare;
    public RTLTextMeshPro minPrice;
    public RTLTextMeshPro maxPrice;
    public RTLTextMeshPro terminatePenalty;
    public RTLTextMeshPro lostSalePenalty;
    public GameObject lostSalePenaltyGameObject;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMoreDetailsButtonClick(Utils.Contract contract)
    {
        currentBrand.text = contract.currentBrand.ToString("0.00");

        if (contract.contractDate.ToDateTime() > MainHeaderManager.Instance.gameDate.ToDateTime())
        {
            valueShare.text = "-";
            minPrice.text = "-";
            maxPrice.text = "-";
        }
        else
        {
            valueShare.text = contract.valueShare.ToString("0.00");
            minPrice.text = contract.minPrice.ToString("0.00");
            maxPrice.text = contract.maxPrice.ToString("0.00");
        }

        terminatePenalty.text = contract.terminatePenalty.ToString();

        if (contract.lostSalePenalty != 0)
        {
            lostSalePenalty.text = contract.lostSalePenalty.ToString();
            lostSalePenaltyGameObject.SetActive(true);
        }
        else
        {
            lostSalePenaltyGameObject.SetActive(false);
        }

        contractData = contract;
        detailsCanvasGameObject.SetActive(true);
    }

    public void OnCloseButtonClick()
    {
        contractData = null;
        detailsCanvasGameObject.SetActive(false);
    }
}
