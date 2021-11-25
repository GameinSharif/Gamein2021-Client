using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTabManager : MonoBehaviour
{
    public int index;

    public void OnTabButtonClick()
    {
        MainMenuManager.Instance.OpenPage(index);
    }
}
