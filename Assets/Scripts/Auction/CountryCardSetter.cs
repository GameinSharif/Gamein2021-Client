using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountryCardSetter : MonoBehaviour
{
    public GameObject selectedBorder;
    public GameObject cardBg;
    public GameObject blackMap;
    public GameObject countryName;
    public GameObject unavailableMask;

    public void SetAllValues(string countryName, string localizeKey, Sprite blackMap, Sprite cardBg)
    {
        this.cardBg.GetComponent<Image>().sprite = cardBg;
        this.blackMap.GetComponent<Image>().sprite = blackMap;
        this.countryName.GetComponent<RTLTextMeshPro>().text = countryName;
        //this.countryName.GetComponent<Localize>().SetKey(localizeKey);
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

    public string GetCountryName()
    {
        return countryName.GetComponent<RTLTextMeshPro>().text;
    }
}
