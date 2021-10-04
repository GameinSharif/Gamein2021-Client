using System;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedPoolingSystem<T>
{
    private readonly Transform _parent;
    private readonly GameObject[] _prefabs;
    
    //gameObject, indexOfPrefab, indexInParent, data
    private readonly Action<GameObject, int, int, T> _initializerAction;
    private readonly List<GameObject>[] _objectsOfPrefab;
    public int ActiveCount { get; private set; }

    public EnhancedPoolingSystem(Transform parent, GameObject[] prefabs, Action<GameObject, int, int, T> initializerAction, int capacityPerPrefab)
    {
        _parent = parent;
        _prefabs = prefabs;
        _initializerAction = initializerAction;
        ActiveCount = 0;
        _objectsOfPrefab = new List<GameObject>[prefabs.Length];
        
        ClearParent();
        Initialize(capacityPerPrefab);
    }
    
    private void ClearParent()
    {
        foreach (Transform child in _parent)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    private void Initialize(int capacityPerPrefab)
    {
        for (int i = 0; i < _prefabs.Length; i++)
        {
            _objectsOfPrefab[i] = new List<GameObject>(capacityPerPrefab);
            for (int j = 0; j < capacityPerPrefab; j++)
            {
                var gameObject = UnityEngine.Object.Instantiate(_prefabs[i], _parent);
                _objectsOfPrefab[i].Add(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
    
    private GameObject GetAvailableGameObjectOfIndex(int i)
    {
        foreach (GameObject gameObject in _objectsOfPrefab[i])
        {
            if (!gameObject.activeInHierarchy)
            {
                return gameObject;
            }
        }
        
        var newGameObject = UnityEngine.Object.Instantiate(_prefabs[i], _parent);
        _objectsOfPrefab[i].Add(newGameObject);
        return newGameObject;
    }

    public void Add(int indexOfPrefab, int indexInParent, T data)
    {
        indexInParent = indexInParent > ActiveCount ? ActiveCount : indexInParent;

        var gameObject = GetAvailableGameObjectOfIndex(indexOfPrefab);
        
        _initializerAction.Invoke(gameObject, indexOfPrefab, indexInParent, data);

        gameObject.SetActive(true);
        gameObject.transform.SetSiblingIndex(indexInParent);
        ActiveCount++;
    }

    public void Add(int indexOfPrefab, T data)
    {
        Add(indexOfPrefab, ActiveCount, data);
    }

    public void Remove(GameObject gameObject)
    {
        
        if (!gameObject.transform.IsChildOf(_parent))
        {
            return;
        }

        LogicalRemove(gameObject);
    }

    public void Remove(int index)
    {
        index = index >= ActiveCount ? ActiveCount - 1 : index;
        var child = _parent.GetChild(index);
        
        LogicalRemove(child.gameObject);
    }

    public void RemoveAll()
    {
        foreach (Transform child in _parent)
        {
            child.gameObject.SetActive(false);
        }

        ActiveCount = 0;
    }

    private void LogicalRemove(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.SetSiblingIndex(_parent.childCount - 1);
        ActiveCount--;
    }
    
}
