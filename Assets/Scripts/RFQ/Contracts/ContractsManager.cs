using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractsManager : MonoBehaviour
{
    public static ContractsManager Instance;

    public GameObject ContractParentGameObject;
    public GameObject ContractPrefab;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    [HideInInspector] public List<Utils.Contract> myContracts = new List<Utils.Contract>();

    private void Awake()
    {
        Instance = this;
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
            AddContractItemToList(getContractsResponse.contracts[i]);
        }

        Canvas.ForceUpdateCanvases();
    }

    public void AddContractItemToList(Utils.Contract contract)
    {
        GameObject contractGameObject = GetPoolledContractGameObject();

        SetContractDetail setContractDetail = contractGameObject.GetComponent<SetContractDetail>();
        setContractDetail.InitializeContract(contract);

        contractGameObject.SetActive(true);
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
}