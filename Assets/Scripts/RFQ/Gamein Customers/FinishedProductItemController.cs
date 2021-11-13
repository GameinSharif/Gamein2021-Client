using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class FinishedProductItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public Localize productNameLocalize;

    public GameObject ShowDemandsButtonGameObject;
    public GameObject HideDemandsButtonGameObject;
    public GameObject DemandsParent;
    public GameObject DemandItemsParent;

    public GameObject DemandItemPrefab;
    
    private Utils.Product _rawProduct;
    private List<Utils.WeekDemand> _Demands;
    private List<GameObject> _spawnedDemandItemGameObjects = new List<GameObject>();

    public void SetInfo(int no, string productName)
    {
        this.no.text = no.ToString();
        productNameLocalize.SetKey("product_" + productName);
    }

    public void SetInfo(int no, Utils.Product finishedProduct)
    {
        _rawProduct = finishedProduct;

        SetInfo(
            no: no,
            productName: finishedProduct.name
        );
        
        ShowDemandsButtonGameObject.SetActive(true);
        HideDemandsButtonGameObject.SetActive(false);
        SetDemands();
        DemandsParent.SetActive(false);
    }

    public void SetDemands()
    {
        _Demands = GameDataManager.Instance.GetCurrentWeekRawProductDemands(_rawProduct.id);
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < _Demands.Count; i++)
        {
            AddContractDetailItemToList(_Demands[i], i + 1);
        }
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedDemandItemGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void AddContractDetailItemToList(Utils.WeekDemand Demand, int index)
    {
        GameObject createdItem = GetItem(DemandItemsParent);
        createdItem.transform.SetSiblingIndex(index);

        FinishedProductCustomerItemController controller = createdItem.GetComponent<FinishedProductCustomerItemController>();
        controller.SetInfo(index, Demand);

        createdItem.SetActive(true);
    }

    private GameObject GetItem(GameObject parent)
    {
        foreach (GameObject gameObject in _spawnedDemandItemGameObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        GameObject newItem;
        newItem = Instantiate(DemandItemPrefab, parent.transform);
        _spawnedDemandItemGameObjects.Add(newItem);
        return newItem;
    }

    
    public void OnShowDemandsButtonClicked()
    {
        DemandsParent.SetActive(true);
        ShowDemandsButtonGameObject.SetActive(false);
        HideDemandsButtonGameObject.SetActive(true);
    }

    public void OnHideDemandsButtonClicked()
    {
        DemandsParent.SetActive(false);
        ShowDemandsButtonGameObject.SetActive(true);
        HideDemandsButtonGameObject.SetActive(false);
    }
    
}
