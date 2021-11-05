using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class RawProductItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro productName;
    public Localize productNameLocalize;

    public GameObject ShowSuppliesButtonGameObject;
    public GameObject HideSuppliesButtonGameObject;
    public GameObject SuppliesParent;
    public GameObject SupplyItemsParent;

    public GameObject SupplyItemPrefab;
    
    private Utils.Product _rawProduct;
    private List<Utils.WeekSupply> _supplies;
    private List<GameObject> _spawnedSupplyItemGameObjects = new List<GameObject>();

    public void SetInfo(int no, string productName)
    {
        this.no.text = no.ToString();
        this.productName.text = productName;
        productNameLocalize.SetKey("product_" + productName);
    }

    public void SetInfo(int no, Utils.Product rawProduct)
    {
        SetInfo(
            no: no,
            productName: rawProduct.name
        );
        
        ShowSuppliesButtonGameObject.SetActive(true);
        HideSuppliesButtonGameObject.SetActive(false);
        SetSupplies();
        SuppliesParent.SetActive(false);
        _rawProduct = rawProduct;
    }

    public void SetSupplies()
    {
        _supplies = GameDataManager.Instance.GetCurrentWeekRawProductSupplies(_rawProduct.id);
        DeactiveAllChildrenInScrollPanel();
        for (int i = 0; i < _supplies.Count; i++)
        {
            AddContractDetailItemToList(_supplies[i], i + 1);
        }
    }

    private void DeactiveAllChildrenInScrollPanel()
    {
        foreach (GameObject gameObject in _spawnedSupplyItemGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void AddContractDetailItemToList(Utils.WeekSupply supply, int index)
    {
        GameObject createdItem = GetItem(SupplyItemsParent);
        createdItem.transform.SetSiblingIndex(index);

        RawProductSupplyItemController controller = createdItem.GetComponent<RawProductSupplyItemController>();
        controller.SetInfo(index, supply);

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

        GameObject newItem;
        newItem = Instantiate(SupplyItemPrefab, parent.transform);
        _spawnedSupplyItemGameObjects.Add(newItem);
        return newItem;
    }

    
    public void OnShowSuppliesButtonClicked()
    {
        SuppliesParent.SetActive(true);
        ShowSuppliesButtonGameObject.SetActive(false);
        HideSuppliesButtonGameObject.SetActive(true);
    }

    public void OnHideSuppliesButtonClicked()
    {
        SuppliesParent.SetActive(false);
        ShowSuppliesButtonGameObject.SetActive(true);
        HideSuppliesButtonGameObject.SetActive(false);
    }
    
}
