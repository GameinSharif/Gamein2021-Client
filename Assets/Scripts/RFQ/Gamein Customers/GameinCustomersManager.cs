using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameinCustomersManager : MonoBehaviour
{
    public GameObject GameinCustomersParentGameObject;
    public GameObject GameinCustomersPrefab;

    private List<GameObject> _spawnedObjects;

    public void InitializeGameinCustomersInShop(List<RFQUtils.GameinCustomer> gameinCustomers)
    {
        foreach (GameObject gameObject in _spawnedObjects)
        {
            gameObject.SetActive(false);
        }

        for (int i = 0; i < gameinCustomers.Count; i++)
        {
            GameObject contractGameObject = GetPoolledContractGameObject();

            SetGameinCustomerDetail setGameinCustomerDetail = GetComponent<SetGameinCustomerDetail>();
            setGameinCustomerDetail.InitializeGameinCustomer(gameinCustomers[i]);

            contractGameObject.transform.SetSiblingIndex(i);
            contractGameObject.SetActive(true);
        }
    }

    private GameObject GetPoolledContractGameObject()
    {
        foreach (GameObject gameObject in _spawnedObjects)
        {
            if (!gameObject.activeInHierarchy)
            {
                return gameObject;
            }
        }

        var instance = Instantiate(GameinCustomersPrefab, GameinCustomersParentGameObject.transform);
        _spawnedObjects.Add(instance);
        return instance;
    }
}
