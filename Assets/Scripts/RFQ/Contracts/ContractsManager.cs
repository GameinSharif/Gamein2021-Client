using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractsManager : MonoBehaviour
{
    public static ContractsManager Instance;

    public GameObject ContractParentGameObject;
    public GameObject ContractPrefab;

    private List<GameObject> _spawnedObjects;
    [HideInInspector] public List<Utils.Contract> myContracts;

    private void Awake()
    {
        Instance = this;
        _spawnedObjects = new List<GameObject>();
        myContracts = new List<Utils.Contract>();
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetContractsResponseEvent += OnGetContractsResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetContractsResponseEvent -= OnGetContractsResponse;
    }

    public void OnGetContractsResponse(GetContractsResponse getContractsResponse)
    {
        myContracts = getContractsResponse.contracts;
        
        foreach (GameObject gameObject in _spawnedObjects)
        {
            gameObject.SetActive(false);
        }

        for (int i=0; i < getContractsResponse.contracts.Count; i++)
        {
            AddContractItemToList(getContractsResponse.contracts[i], i);
        }

        Canvas.ForceUpdateCanvases();
    }

    private void AddContractItemToList(Utils.Contract contract, int index)
    {
        GameObject contractGameObject = GetPoolledContractGameObject();

        SetContractDetail setContractDetail = contractGameObject.GetComponent<SetContractDetail>();
        setContractDetail.InitializeContract(contract, index);

        contractGameObject.transform.SetSiblingIndex(index + 1);
        contractGameObject.SetActive(true);
    }

    public void AddContractItemsToList(List<Utils.Contract> contracts)
    {
        foreach (Utils.Contract contract in contracts)
        {
            myContracts.Add(contract);
            AddContractItemToList(contract, myContracts.Count-1);   
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

        var instance = Instantiate(ContractPrefab, ContractParentGameObject.transform);
        _spawnedObjects.Add(instance);
        return instance;
    }

    //TODO new contract gamein customer
}
