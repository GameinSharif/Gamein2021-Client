using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameinCustomersManager : MonoBehaviour
{
    public static GameinCustomersManager Instance;

    public GameObject GameinCustomersParentGameObject;
    public GameObject GameinCustomersPrefab;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeGameinCustomersInShop(List<RFQUtils.GameinCustomer> gameinCustomers)
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

            gameinCustomerGameObject.transform.SetSiblingIndex(i);
            gameinCustomerGameObject.SetActive(true);
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
