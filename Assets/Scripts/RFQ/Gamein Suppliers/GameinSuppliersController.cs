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

    [HideInInspector] List<Utils.ContractSupplier> MyContractSuppliers;
    [HideInInspector] List<Utils.Product> Ingredients;

    public GameObject ingredientItemPrefab;
    public GameObject ingredientContractItemPrefab;

    public GameObject ContractSuppliersScrollViewParent;
    public GameObject IngredientsScrollViewParent;

    public GameObject gameinSuppliersCanvas;
    
    private List<GameObject> _spawnedContractGameObjects = new List<GameObject>();
    private List<GameObject> _spawnedIngredientGameObjects = new List<GameObject>();

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

    public void UpdateSupplies(List<Utils.WeekSupply> weekSupplies)
    {
        List<Utils.Product> rawProducts = GameDataManager.Instance.GetRawProducts();
        
    }
    
    public void OnGetContractSuppliersResponse(GetContractSuppliersResponse getContractSuppliersResponse)
    {
        int teamId = PlayerPrefs.GetInt("TeamId");
        MyContractSuppliers = getContractSuppliersResponse.ContractSuppliers.Where(d => d.teamId == teamId) as List<Utils.ContractSupplier>;
        DeactiveAllChildrenInScrollPanel(true);
        for (int i = 0; i < MyContractSuppliers.Count; i++)
        {
            AddContractItemToList(MyContractSuppliers[i], i + 1);
        }
    }
    
    private void AddContractItemToList(Utils.ContractSupplier contractSupplier, int index)
    {
        GameObject createdItem = GetItem(ContractSuppliersScrollViewParent, true);
        createdItem.transform.SetSiblingIndex(index);

        ContractSupplierItemController controller = createdItem.GetComponent<ContractSupplierItemController>();
        controller.SetInfo(index, contractSupplier);

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
        
            newItem = Instantiate(ingredientContractItemPrefab, parent.transform);
            _spawnedContractGameObjects.Add(newItem);
        }
        else //TODO not like this
        {
            foreach (GameObject gameObject in _spawnedIngredientGameObjects)
            {
                if (!gameObject.activeSelf)
                {
                    return gameObject;
                }
            }

            newItem = Instantiate(ingredientItemPrefab, parent.transform);
            _spawnedIngredientGameObjects.Add(newItem);
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
        else //TODO not how it should be done for the ingredients
        {
            foreach (GameObject gameObject in _spawnedIngredientGameObjects)
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
