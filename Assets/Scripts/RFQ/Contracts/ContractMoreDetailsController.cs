using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractMoreDetailsController : MonoBehaviour
{
    public static ContractMoreDetailsController Instance;

    public GameObject detailsCanvasGameObject;

    public RTLTextMeshPro currentBrand;
    public RTLTextMeshPro valueShare;
    public RTLTextMeshPro minPrice;
    public RTLTextMeshPro maxPrice;
    public RTLTextMeshPro lostSalePenalty;
    public RTLTextMeshPro terminatePenalty;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMoreDetailsButtonClick(Utils.Contract contract)
    {
        currentBrand.text = contract.currentBrand.ToString();
        valueShare.text = contract.valueShare.ToString("0.00");
        minPrice.text = contract.minPrice.ToString("0.00");
        maxPrice.text = contract.maxPrice.ToString("0.00");
        lostSalePenalty.text = contract.lostSalePenalty.ToString();
        terminatePenalty.text = contract.terminatePenalty.ToString();

        detailsCanvasGameObject.SetActive(true);
    }
}
