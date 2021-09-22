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
        EventManager.Instance.OnGetContractsResponseEvent += OnGetMyDealsResponse;
    }

    public void GetContracts()
    {
        GetContractsRequest getContractsRequest = new GetContractsRequest(RequestTypeConstant.GET_CONTRACTS);
        RequestManager.Instance.SendRequest(getContractsRequest);
    }

    public void OnGetMyDealsResponse(GetContractsResponse getContractsResponse)
    {

    }
}
