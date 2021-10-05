using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractsManager : MonoBehaviour
{
    public static ContractsManager Instance;

    public GameObject ContractParentGameObject;
    public GameObject ContractPrefab;

    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGetContractsResponseEvent += OnGetContractsResponse;
    }

    public void OnGetContractsResponse(GetContractsResponse getContractsResponse)
    {
        foreach (GameObject gameObject in _spawnedObjects)
        {
            gameObject.SetActive(false);
        }

        for (int i=0; i < getContractsResponse.contracts.Count; i++)
        {
            GameObject contractGameObject = GetPoolledContractGameObject();

            SetContractDetail setContractDetail = contractGameObject.GetComponent<SetContractDetail>();
            setContractDetail.InitializeContract(getContractsResponse.contracts[i], i);

            contractGameObject.transform.SetSiblingIndex(i + 1);
            contractGameObject.SetActive(true);
        }

        Canvas.ForceUpdateCanvases();
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

    //TODO new contract gamein customer
}
