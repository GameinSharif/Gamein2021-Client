using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameinCustomersManager : MonoBehaviour
{
    public static GameinCustomersManager Instance;

    public GameObject GameinCustomersParentGameObject;
    public GameObject GameinCustomersPrefab;

    private List<GameObject> _spawnedObjects;

    private void Awake()
    {
        Instance = this;
        _spawnedObjects = new List<GameObject>();
    }

    public void InitializeGameinCustomersInShop(List<Utils.GameinCustomer> gameinCustomers)
    {
        foreach (GameObject gameObject in _spawnedObjects)
        {
            gameObject.SetActive(false);
        }

        for (int i = 0; i < gameinCustomers.Count; i++)
        {
            GameObject gameinCustomerGameObject = GetPoolledContractGameObject();

            SetGameinCustomerDetail setGameinCustomerDetail = gameinCustomerGameObject.GetComponent<SetGameinCustomerDetail>();
            setGameinCustomerDetail.InitializeGameinCustomer(gameinCustomers[i]);

            gameinCustomerGameObject.transform.SetSiblingIndex(i + 1);
            gameinCustomerGameObject.SetActive(true);
        }
    }

    private GameObject GetPoolledContractGameObject()
    {
        foreach (GameObject gameObject in _spawnedObjects)
        {
            if (!gameObject.activeSelf)
            {
                return gameObject;
            }
        }

        var instance = Instantiate(GameinCustomersPrefab, GameinCustomersParentGameObject.transform);
        _spawnedObjects.Add(instance);
        return instance;
    }
}
