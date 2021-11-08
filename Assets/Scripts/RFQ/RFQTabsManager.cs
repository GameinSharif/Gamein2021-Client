using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RFQTabsManager : MonoBehaviour
{
    public List<GameObject> TabsCanvasGameObjects;
    public List<Image> TabsImages;

    public void OnSelectTabButton(int index)
    {
        DisableAllTabs();

        TabsCanvasGameObjects[index].SetActive(true);
        TabsImages[index].enabled = true;
    }

    private void DisableAllTabs()
    {
        foreach (GameObject gameObject in TabsCanvasGameObjects)
        {
            gameObject.SetActive(false);
        }

        foreach (Image image in TabsImages)
        {
            image.enabled = false;
        }
    }
}
