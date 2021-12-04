using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RFQTabsManager : MonoBehaviour
{
    public static RFQTabsManager Instance;

    public List<GameObject> TabsCanvasGameObjects;

    private int _currentlyOpenTabInex = -1;

    private void Awake()
    {
        Instance = this;
    }

    public void OnOpenMarketPage()
    {
        if (_currentlyOpenTabInex == -1)
        {
            OnSelectTabButton(0);
        }
    }

    public void OnSelectTabButton(int index)
    {
        DisableAllTabs();
        TabsCanvasGameObjects[index].SetActive(true);
        _currentlyOpenTabInex = index;
    }

    private void DisableAllTabs()
    {
        foreach (GameObject gameObject in TabsCanvasGameObjects)
        {
            gameObject.SetActive(false);
        }
    }
}
