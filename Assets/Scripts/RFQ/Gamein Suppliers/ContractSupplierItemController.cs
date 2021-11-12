using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractSupplierItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro supplierName;
    public Localize productNameLocalize;
    public RTLTextMeshPro contractType;
    //public Localize OfferStatusLocalize;

    public GameObject ShowDetailsButtonGameObject;
    public GameObject HideDetailsButtonGameObject;
    public GameObject TerminateContractButtonGameObject;
    public GameObject DetailsParent;
    public GameObject DetailItemsParent;

    public GameObject ContractSupplierDetailItemPrefab;
    
    private Utils.ContractSupplier _contractSupplier;
    private List<Utils.ContractSupplierDetail> _contractSupplierDetails;
    private List<GameObject> _spawnedDetailsGameObjects = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent += OnTerminateLongtermContractSupplierResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTerminateLongtermContractSupplierResponseEvent -= OnTerminateLongtermContractSupplierResponseReceived;
    }
    
    public void SetInfo(int no, string supplierName,  string productNameKey, string contractType)
    {
        this.no.text = no.ToString();
        this.supplierName.text = supplierName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.contractType.text = contractType;
        
    }

    public void SetInfo(int no, Utils.ContractSupplier contractSupplier)
    {
        SetInfo(
            no: no,
            supplierName: GameDataManager.Instance.GetSupplierName(contractSupplier.supplierId),
            productNameKey: GameDataManager.Instance.GetProductById(contractSupplier.materialId).name,
            contractType: contractSupplier.contractType.ToString()
        );

        if (contractSupplier.contractType == Utils.ContractType.LONGTERM)
        {
            TerminateContractButtonGameObject.SetActive(true);
            ShowDetailsButtonGameObject.SetActive(true);
            HideDetailsButtonGameObject.SetActive(false);
        }
        else
        {
            TerminateContractButtonGameObject.SetActive(false);
            ShowDetailsButtonGameObject.SetActive(true);
            HideDetailsButtonGameObject.SetActive(false);
        }

        SetContractDetails();
        DetailsParent.SetActive(false);
        _contractSupplier = contractSupplier;
    }

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
    
    public void OnTerminateButtonClicked()
    {
        TerminateLongtermContractSupplierRequest terminateLongtermContractSupplier = new TerminateLongtermContractSupplierRequest(RequestTypeConstant.TERMINATE_LONGTERM_CONTRACT_WITH_SUPPLIER, _contractSupplier.id);
        RequestManager.Instance.SendRequest(terminateLongtermContractSupplier);
    }
    
    private void OnTerminateLongtermContractSupplierResponseReceived(TerminateLongtermContractSupplierResponse terminateLongtermContractSupplierResponse)
    {
        if (_contractSupplier.id == terminateLongtermContractSupplierResponse.contractId)
        {
            if (terminateLongtermContractSupplierResponse.result == "Successful")
            {
                //TODO show that contract was terminated visually
                TerminateContractButtonGameObject.SetActive(false);
            }
        }
    }
}
