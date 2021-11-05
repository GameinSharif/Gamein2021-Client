using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ContractSupplierItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro supplierName;
    //public Localize productNameLocalize;
    public RTLTextMeshPro productName;
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

    public void SetInfo(int no, string supplierName,  string productName, string contractType)
    {
        this.no.text = no.ToString();
        this.supplierName.text = supplierName;
        //productNameLocalize.SetKey("product_" + productNameKey);
        this.productName.text = productName;
        this.contractType.text = contractType;
        
    }

    public void SetInfo(int no, Utils.ContractSupplier contractSupplier)
    {
        SetInfo(
            no: no,
            supplierName: GameDataManager.Instance.GetSupplierName(contractSupplier.supplierId),
            //productNameKey: GameDataManager.Instance.GetProductById(offer.productId).name,
            productName: GameDataManager.Instance.GetProductName(contractSupplier.materialId), 
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
            AddContractItemToList(_contractSupplierDetails[i], i + 1);
        }
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedDetailsGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void AddContractItemToList(Utils.ContractSupplierDetail contractSupplierDetail, int index)
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
        DetailsParent.SetActive(false);
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
        // TODO
    }
}
