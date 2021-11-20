using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractSupplierItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro supplierName;
    public Localize productNameLocalize;
    public RTLTextMeshPro contractDate;
    public RTLTextMeshPro currentWeekPrice;
    public RTLTextMeshPro boughtAmount;
    public RTLTextMeshPro transportType;
    public RTLTextMeshPro transportCost;
    public Localize hasInsurance;
    public RTLTextMeshPro terminatePenalty;
    public RTLTextMeshPro noMoneyPenalty;
    
    // public GameObject ShowDetailsButtonGameObject;
    // public GameObject HideDetailsButtonGameObject;
    public GameObject TerminateContractButtonGameObject;
    // public GameObject DetailsParent;
    // public GameObject DetailItemsParent;

    //public GameObject ContractSupplierDetailItemPrefab;
    
    private Utils.ContractSupplier _contractSupplier;
    //private List<Utils.ContractSupplierDetail> _contractSupplierDetails;
    //private List<GameObject> _spawnedDetailsGameObjects = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent += OnTerminateLongtermContractSupplierResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent -= OnTerminateLongtermContractSupplierResponseReceived;
    }
    
    public void SetInfo(int no, string supplierName, string productNameKey, CustomDate contractDate, float currentWeekPrice, int boughtAmount, Utils.VehicleType transportType, float transportCost, bool hasInsurance, int terminatePenalty, int noMoneyPenalty)
    {
        this.no.text = no.ToString();
        this.supplierName.text = supplierName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.contractDate.text = contractDate.ToString();
        this.currentWeekPrice.text = currentWeekPrice.ToString();
        this.boughtAmount.text = boughtAmount.ToString();
        this.transportType.text = transportType.ToString();
        this.transportCost.text = transportCost.ToString();
        this.hasInsurance.SetKey(hasInsurance ? "contract_supplier_item_insurance_yes" : "contract_supplier_item_insurance_no");
        this.terminatePenalty.text = terminatePenalty.ToString();
        this.noMoneyPenalty.text = noMoneyPenalty.ToString();
    }

    public void SetInfo(int no, Utils.ContractSupplier contractSupplier)
    {
        SetInfo(
            no: no,
            supplierName: GameDataManager.Instance.GetSupplierName(contractSupplier.supplierId),
            productNameKey: GameDataManager.Instance.GetProductById(contractSupplier.materialId).name,
            contractDate: contractSupplier.contractDate,
            currentWeekPrice: contractSupplier.pricePerUnit,
            boughtAmount: contractSupplier.boughtAmount,
            transportType: contractSupplier.transportType,
            transportCost: contractSupplier.transportationCost,
            hasInsurance: contractSupplier.hasInsurance,
            terminatePenalty: contractSupplier.terminatePenalty,
            noMoneyPenalty: contractSupplier.noMoneyPenalty
        );

        // if (contractSupplier.contractType == Utils.ContractType.LONGTERM)
        // {
        //     TerminateContractButtonGameObject.SetActive(true);
        //     ShowDetailsButtonGameObject.SetActive(true);
        //     HideDetailsButtonGameObject.SetActive(false);
        // }
        // else
        // {
        //     TerminateContractButtonGameObject.SetActive(false);
        //     ShowDetailsButtonGameObject.SetActive(true);
        //     HideDetailsButtonGameObject.SetActive(false);
        // }

        _contractSupplier = contractSupplier;
        // SetContractDetails();
        // DetailsParent.SetActive(false);
    }
    /*
    public void SetContractDetails()
    {
        _contractSupplierDetails = _contractSupplier.contractSupplierDetails;
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < _contractSupplierDetails.Count; i++)
        {
            AddContractDetailItemToList(_contractSupplierDetails[i], i + 1);
        }
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedDetailsGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void AddContractDetailItemToList(Utils.ContractSupplierDetail contractSupplierDetail, int index)
    {
        GameObject createdItem = GetItem(DetailItemsParent);
        createdItem.transform.SetSiblingIndex(index);

        ContractSupplierDetailItemController controller = createdItem.GetComponent<ContractSupplierDetailItemController>();
        controller.SetInfo(index, contractSupplierDetail);

        createdItem.SetActive(true);
    }

    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedDetailsGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem;
        newItem = Instantiate(ContractSupplierDetailItemPrefab, parent.transform);
        _spawnedDetailsGameObjects.Add(newItem);
        return newItem;
    }

    
    public void OnShowDetailsButtonClicked()
    {
        DetailsParent.SetActive(true);
        ShowDetailsButtonGameObject.SetActive(false);
        HideDetailsButtonGameObject.SetActive(true);
    }

    public void OnHideDetailsButtonClicked()
    {
        DetailsParent.SetActive(false);
        ShowDetailsButtonGameObject.SetActive(true);
        HideDetailsButtonGameObject.SetActive(false);
    }
    */
    public void OnTerminateButtonClicked()
    {
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                TerminateLongtermContractSupplierRequest terminateLongtermContractSupplier = new TerminateLongtermContractSupplierRequest(RequestTypeConstant.TERMINATE_LONGTERM_CONTRACT_WITH_SUPPLIER, _contractSupplier.id);
                RequestManager.Instance.SendRequest(terminateLongtermContractSupplier);
            }
        });
    }
    
    private void OnTerminateLongtermContractSupplierResponseReceived(TerminateLongtermContractSupplierResponse terminateLongtermContractSupplierResponse)
    {
        if (_contractSupplier.id == terminateLongtermContractSupplierResponse.contractSupplier.id)
        {
            if (terminateLongtermContractSupplierResponse.result == "Successful")
            {
                DialogManager.Instance.ShowErrorDialog("contract_supplier_successfully_terminated");
                MainHeaderManager.Instance.Money = MainHeaderManager.Instance.Money - terminateLongtermContractSupplierResponse.contractSupplier.terminatePenalty;
                TerminateContractButtonGameObject.SetActive(false);
            }
        }
    }
}
