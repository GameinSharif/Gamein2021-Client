using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class GameinCustomersController : MonoBehaviour
{
    public static GameinCustomersController Instance;

    public GameObject FinishedProductItemPrefab;
    public GameObject FinishedProductsScrollViewParent;
    
    private List<GameObject> _spawnedRawProductGameObjects = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void UpdateDemands()
    {
        List<Utils.Product> FinishedProducts = GameDataManager.Instance.GetFinishedProducts();
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < FinishedProducts.Count; i++)
        {
            AddFinishedProductItemToList(FinishedProducts[i], i + 1);
        }
    }
    
    private void AddFinishedProductItemToList(Utils.Product product, int index)
    {
        GameObject createdItem = GetItem(FinishedProductsScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        FinishedProductItemController controller = createdItem.GetComponent<FinishedProductItemController>();
        controller.SetInfo(product);

        createdItem.SetActive(true);
    }
    
    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedRawProductGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem = Instantiate(FinishedProductItemPrefab, parent.transform);
        _spawnedRawProductGameObjects.Add(newItem);

        return newItem;
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedRawProductGameObjects)
        {
            gameObject.SetActive(false);
        }
    }    
}
