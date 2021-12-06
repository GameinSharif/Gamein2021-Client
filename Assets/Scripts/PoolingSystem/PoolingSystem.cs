using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem<T>
{
    private readonly Transform parent;
    private readonly GameObject prefab;
    private readonly Action<GameObject, int, T> initializerAction;
    private int counter;

    public PoolingSystem(Transform parent, GameObject prefab, Action<GameObject, int, T> initializerAction, int initialCapacity=10, bool clearParent=true)
    {
        this.parent = parent;
        this.prefab = prefab;
        this.initializerAction = initializerAction;
        counter = 0;
        
        if (clearParent)
        {
            ClearParent();
        }

        for (int i = 0; i < initialCapacity; i++)
        {
            UnityEngine.Object.Instantiate(prefab, parent).SetActive(false);
        }
    }
    
    public int ActiveCount => counter;

    private void ClearParent()
    {
        foreach (Transform child in parent)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }
    
    private GameObject GetAvailableGameObject()
    {
        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeSelf)
            {
                return child.gameObject;
            }
        }
        
        var newGameObject = UnityEngine.Object.Instantiate(prefab, parent);
        return newGameObject;
    }

    public void Add(int index, T data)
    {
        index = index > ActiveCount ? ActiveCount : index;
        
        var gameObject = GetAvailableGameObject();
        
        initializerAction.Invoke(gameObject, index, data);

        gameObject.SetActive(true);
        counter++;
    }

    public void Add(T data)
    {
        Add(ActiveCount, data);
    }

    public void Remove(GameObject gameObject)
    {
        
        if (!gameObject.transform.IsChildOf(parent))
        {
            return;
        }

        gameObject.transform.SetSiblingIndex(parent.childCount - 1);
        gameObject.SetActive(false);
        counter--;
    }

    public void Remove(int index)
    {
        index = index >= ActiveCount ? ActiveCount - 1 : index;
        var child = parent.GetChild(index);
        child.SetSiblingIndex(parent.childCount - 1);
        child.gameObject.SetActive(false);
        counter--;
    }
    
    public void RemoveAll()
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }

        counter = 0;
    }
    
}
