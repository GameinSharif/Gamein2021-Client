using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractsManager : MonoBehaviour
{
    public GameObject ContractParentGameObject;
    public GameObject ContractPrefab;

    private List<GameObject> _spawnedObjects;

    private void Awake()
    {
        EventManager.Instance.OnGetContractsResponseEvent += OnGetContractsResponse;
    }

    public void GetContracts()
    {
        GetContractsRequest getContractsRequest = new GetContractsRequest(RequestTypeConstant.GET_CONTRACTS);
        RequestManager.Instance.SendRequest(getContractsRequest);
    }

    public void OnGetContractsResponse(GetContractsResponse getContractsResponse)
    {
        foreach (GameObject gameObject in _spawnedObjects)
        {
            gameObject.SetActive(false);
        }

        for(int i=0; i < getContractsResponse.contracts.Count; i++)
        {
            GameObject contractGameObject = GetPoolledContractGameObject();

            SetContractDetail setContractDetail = contractGameObject.GetComponent<SetContractDetail>();
            setContractDetail.InitializeContract(getContractsResponse.contracts[i], i);

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

        var instance = Instantiate(ContractPrefab, ContractParentGameObject.transform);
        _spawnedObjects.Add(instance);
        return instance;
    }

    //TODO terminate longterm contract

    //TODO (another script) Get all gamein customers

    //TODO make a deal with gamein customer
}
