using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using RTLTMPro;
using UnityEngine.UI;

public class SuppliersController : MonoBehaviour
{
    public static SuppliersController Instance;

    public GameObject SuppliersParentGameObject;
    public GameObject ContractsParentGameObject;

    public GameObject SuppliersScrollPanel;

    public GameObject supplyItemPrefab;

    private Utils.Product _product;
    private List<Utils.WeekSupply> _supplies;
    private List<GameObject> _spawnedSupplyItemGameObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnShowSuppliersClick(Utils.Product product)
    {
        _product = product;
        
        SetSupplies();

        ContractsParentGameObject.SetActive(false);
        SuppliersParentGameObject.SetActive(true);
        //Canvas.ForceUpdateCanvases();
    }
        
    private void SetSupplies()
    {
        _supplies = GameDataManager.Instance.GetCurrentWeekRawProductSupplies(_product.id);
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < _supplies.Count; i++)
        {
            AddSupplierItemToList(_supplies[i], i + 1);
        }
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedSupplyItemGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void AddSupplierItemToList(Utils.WeekSupply supply, int index)
    {
        GameObject createdItem = GetItem(SuppliersScrollPanel);
        createdItem.transform.SetSiblingIndex(index);

        RawProductSupplyItemController controller = createdItem.GetComponent<RawProductSupplyItemController>();
        controller.SetInfo(supply);

        createdItem.SetActive(true);
    }

    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedSupplyItemGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem = Instantiate(supplyItemPrefab, parent.transform);
        _spawnedSupplyItemGameObjects.Add(newItem);

        return newItem;
    }
}