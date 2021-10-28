using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountryCardSetter : MonoBehaviour
{
    public GameObject selectedBorder;
    public Image blackMap;
    public Localize countryNameLocalize;
    public GameObject unavailableMask;

    public void SetAllValues(string countryNameKey, Sprite blackMapSprite)
    {
        blackMap.sprite = blackMapSprite;
        countryNameLocalize.SetKey(countryNameKey);

        selectedBorder.SetActive(false);
        unavailableMask.SetActive(false);
    }

    public void SetMaskActive(bool active)
    {
        unavailableMask.SetActive(active);
    }

    public void SetSelectedBorderActive(bool active)
    {
        selectedBorder.SetActive(active);
    }
}
