using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using RTLTMPro;
using UnityEngine.UI;

public class CustomersController : MonoBehaviour
{
    public static CustomersController Instance;

    public GameObject CustomersParentGameObject;
    public GameObject ContractsParentGameObject;

    public GameObject CustomersScrollPanel;

    public GameObject demandItemPrefab;

    private Utils.Product _product;
    private List<Utils.WeekDemand> _demands;

    [HideInInspector] public int StorageIndex;

    public GameObject WarehouseTabGameObject;
    public GameObject DCTabGameObject;
    public RTLTextMeshPro DCTabId;

    private List<FinishedProductCustomerItemController> _finishedProductCustomerItemControllers;
    private List<GameObject> _spawnedDemandsItemGameObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnShowDemandersClick(Utils.Product product)
    {
        _product = product;
        _finishedProductCustomerItemControllers = new List<FinishedProductCustomerItemController>();

        SetStorageIndex(0);
        SetDemands();

        ContractsParentGameObject.SetActive(false);
        CustomersParentGameObject.SetActive(true);
    }
      
    private void SetDemands()
    {
        _demands = GameDataManager.Instance.GetCurrentWeekFinishedProductDemands(_product.id);
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < _demands.Count; i++)
        {
            AddDemandItemToList(_demands[i], i + 1);
        }
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedDemandsItemGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void AddDemandItemToList(Utils.WeekDemand demand, int index)
    {
        GameObject createdItem = GetItem(CustomersScrollPanel);
        createdItem.transform.SetSiblingIndex(index);

        FinishedProductCustomerItemController controller = createdItem.GetComponent<FinishedProductCustomerItemController>();
        controller.SetInfo(demand, StorageManager.Instance.Storages[StorageIndex]);

        _finishedProductCustomerItemControllers.Add(controller);
        createdItem.SetActive(true);
    }

    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedDemandsItemGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        var newItem = Instantiate(demandItemPrefab, parent.transform);
        _spawnedDemandsItemGameObjects.Add(newItem);
        return newItem;
    }
    
    public void ChangeStorage(int change)
    {
        SetStorageIndex((StorageIndex + change + StorageManager.Instance.Storages.Count) % StorageManager.Instance.Storages.Count);

        foreach (FinishedProductCustomerItemController controller in _finishedProductCustomerItemControllers)
        {
            controller.OnChangeSourceStorage(StorageManager.Instance.Storages[StorageIndex]);
        }
      
    }

    private void SetStorageIndex(int index)
    {
        StorageIndex = index;

        if (index == 0)
        {
            WarehouseTabGameObject.SetActive(true);
            DCTabGameObject.SetActive(false);
        }
        else
        {
            WarehouseTabGameObject.SetActive(false);
            DCTabGameObject.SetActive(true);
            DCTabId.text = StorageManager.Instance.Storages[StorageIndex].buildingId.ToString();
        }
    }
}
