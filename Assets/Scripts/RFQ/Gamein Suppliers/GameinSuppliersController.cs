using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class GameinSuppliersController : MonoBehaviour
{
    public static GameinSuppliersController Instance;

    [HideInInspector] List<Utils.ContractSupplier> MyContractSuppliers = new List<Utils.ContractSupplier>();
    [HideInInspector] List<Utils.Product> RawProducts;

    public GameObject rawProductItemPrefab;
    public GameObject contractSupplierItemPrefab;

    public GameObject contractSuppliersScrollViewParent;
    public GameObject rawProductsScrollViewParent;

    public GameObject gameinSuppliersCanvas;
    
    private List<GameObject> _spawnedContractGameObjects = new List<GameObject>();
    private List<GameObject> _spawnedRawProductGameObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetContractSuppliersResponseEvent += OnGetContractSuppliersResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetContractSuppliersResponseEvent -= OnGetContractSuppliersResponse;
    }

    public void UpdateSupplies()
    {
        RawProducts = GameDataManager.Instance.GetRawProducts();
        DeactiveAllChildrenInScrollPanel(false);
        for (int i = 0; i < RawProducts.Count; i++)
        {
            AddRawProductItemToList(RawProducts[i], i + 1);
        }
    }
    
    public void OnGetContractSuppliersResponse(GetContractSuppliersResponse getContractSuppliersResponse)
    {
        MyContractSuppliers = getContractSuppliersResponse.contractsSupplier;
        DeactiveAllChildrenInScrollPanel(true);
        for (int i = 0; i < MyContractSuppliers.Count; i++)
        {
            AddContractItemToList(MyContractSuppliers[i], i + 1);
        }
    }
    
    private void AddRawProductItemToList(Utils.Product product, int index)
    {
        GameObject createdItem = GetItem(rawProductsScrollViewParent, false);
        createdItem.transform.SetSiblingIndex(index);

        RawProductItemController controller = createdItem.GetComponent<RawProductItemController>();
        controller.SetInfo(index, product);

        createdItem.SetActive(true);
    }

    public void AddContractItemsToList(List<Utils.ContractSupplier> contractSuppliers)
    {
        foreach (Utils.ContractSupplier contractSupplier in contractSuppliers)
        {
            MyContractSuppliers.Add(contractSupplier);
            AddContractItemToList(contractSupplier, MyContractSuppliers.Count);   
        }
    }
    
    private void AddContractItemToList(Utils.ContractSupplier contractSupplier, int index)
    {
        GameObject createdItem = GetItem(contractSuppliersScrollViewParent, true);
        createdItem.transform.SetSiblingIndex(index);

        ContractSupplierItemController controller = createdItem.GetComponent<ContractSupplierItemController>();
        controller.SetInfo(contractSupplier);

        createdItem.SetActive(true);
    }
    
    private GameObject GetItem(GameObject parent, bool isContract)
    {
        GameObject newItem;
        
        if (isContract)
        {
            foreach (GameObject gameObject in _spawnedContractGameObjects)
            {
                if (!gameObject.activeSelf)
                {
                    return gameObject;
                }
            }
        
            newItem = Instantiate(contractSupplierItemPrefab, parent.transform);
            _spawnedContractGameObjects.Add(newItem);
        }
        else
        {
            foreach (GameObject gameObject in _spawnedRawProductGameObjects)
            {
                if (!gameObject.activeSelf)
                {
                    return gameObject;
                }
            }

            newItem = Instantiate(rawProductItemPrefab, parent.transform);
            _spawnedRawProductGameObjects.Add(newItem);
        }
        return newItem;
    }

    private void DeactiveAllChildrenInScrollPanel(bool isContract)
    {
        if (isContract)
        {
            foreach (GameObject gameObject in _spawnedContractGameObjects)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject gameObject in _spawnedRawProductGameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void SetGameinSuppliersCanvasActive(bool active)
    {
        gameinSuppliersCanvas.SetActive(active);
    }
    
}
