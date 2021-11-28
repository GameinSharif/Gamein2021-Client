using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using RTLTMPro;
using UnityEngine.UI;

public class FinishedProductDemandersPopupController : MonoBehaviour
{
    public static FinishedProductDemandersPopupController Instance;

    public GameObject finishedProductDemandersPopupCanvas;
    public GameObject finishedProductDemandersScrollViewParent;

    public GameObject demandItemPrefab;

    public Image productImage;
    public Localize productNameLocalize;

    private Utils.Product _product;
    private List<Utils.WeekDemand> _demands;
    private List<GameObject> _spawnedDemandsItemGameObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void OnShowDemandersClick(Utils.Product product)
    {
        _product = product;
        
        productNameLocalize.SetKey("product_" + product.name);
        productImage.sprite = GameDataManager.Instance.ProductSprites[product.id - 1];
        SetDemands();
        finishedProductDemandersPopupCanvas.SetActive(true);
        Canvas.ForceUpdateCanvases();
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
        GameObject createdItem = GetItem(finishedProductDemandersScrollViewParent);
        createdItem.transform.SetSiblingIndex(index);

        FinishedProductCustomerItemController controller = createdItem.GetComponent<FinishedProductCustomerItemController>();
        controller.SetInfo(index, demand);

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
    
}
