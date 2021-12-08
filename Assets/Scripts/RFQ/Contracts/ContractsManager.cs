using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractsManager : MonoBehaviour, IComparer<Utils.Contract>
{
    public static ContractsManager Instance;

    public RectTransform ContractParent;
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
        myContracts.Sort(this);

        foreach (GameObject gameObject in _spawnedObjects)
        {
            gameObject.SetActive(false);
        }

        for (int i=0; i < myContracts.Count; i++)
        {
            AddContractItemToList(myContracts[i], i);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ContractParent);
    }

    public void AddContractItemToList(Utils.Contract contract, int index = -1)
    {
        bool shouldRebuild = false;
        if (index == -1)
        {
            index = ~ myContracts.BinarySearch(contract, this);
            myContracts.Insert(index, contract);
            shouldRebuild = true;
        }

        GameObject contractGameObject = GetPoolledContractGameObject();

        SetContractDetail setContractDetail = contractGameObject.GetComponent<SetContractDetail>();
        setContractDetail.InitializeContract(contract);
        contractGameObject.transform.SetSiblingIndex(index);
        
        contractGameObject.SetActive(true);

        if (shouldRebuild)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(ContractParent);
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

        var instance = Instantiate(ContractPrefab, ContractParent);
        _spawnedObjects.Add(instance);
        return instance;
    }

    public int Compare(Utils.Contract a, Utils.Contract b)
    {
        var dateDiff = a.contractDate.CompareTo(b.contractDate);
        if (dateDiff != 0)
        {
            return dateDiff;
        }

        return a.id - b.id;
    }
}