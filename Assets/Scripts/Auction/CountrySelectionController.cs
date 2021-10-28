using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CountrySelectionController : MonoBehaviour
{
    public static CountrySelectionController Instance;

    [Serializable]
    public class CountryCard
    {
        public Sprite blackMap;
        public Utils.Country country;
    }

    public GameObject countrySelectionCanvas;

    public List<CountryCardSetter> countryCardSetters;
    public List<CountryCard> cards;
    public Button goToMapButton;
    public Button startRandomizeProcessButton;

    private List<string> _countryNameLocalizeKeys = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        goToMapButton.gameObject.SetActive(false);
        startRandomizeProcessButton.gameObject.SetActive(true);

        FillCountryNameLocalizeKeyList();
        DisplayCards();
    }

    private void FillCountryNameLocalizeKeyList()
    {
        _countryNameLocalizeKeys.Add("auction_france");
        _countryNameLocalizeKeys.Add("auction_germany");
        _countryNameLocalizeKeys.Add("auction_united_kingdom");
        _countryNameLocalizeKeys.Add("auction_netherlands");
        _countryNameLocalizeKeys.Add("auction_belgium");
        _countryNameLocalizeKeys.Add("auction_switzerland");
    }
        
    private void DisplayCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            string localizeKey = _countryNameLocalizeKeys[i];

            Sprite blackMap = cards[i].blackMap;
            countryCardSetters[i].SetAllValues(localizeKey, blackMap);
        }
    }

    public void OnStartRandomizeProcessButtonClick()
    {
        startRandomizeProcessButton.interactable = false;
        for(int i = 0; i < cards.Count; i++)
        {
            countryCardSetters[i].SetMaskActive(true);
        }
        
        StartCoroutine(ShowCountryCoroutine());
    }
    
    private IEnumerator ShowCountryCoroutine()
    {
        for (int i=0;i < 50; i++)
        {
            int randomCountryIndex = Random.Range(0, Enum.GetNames(typeof(Utils.Country)).Length);
            ShowRandomlySelectedCountry(randomCountryIndex);

            float waitTime = Random.Range(0.1f, 0.2f);
            yield return new WaitForSeconds(waitTime);
        }

        Enum.TryParse(PlayerPrefs.GetString("Country"), out Utils.Country country);
        int countryIndex = (int) country;

        ShowRandomlySelectedCountry(countryIndex);

        goToMapButton.gameObject.SetActive(true);
        startRandomizeProcessButton.gameObject.SetActive(false);
    }

    private void ShowRandomlySelectedCountry(int countryIndex)
    {
        DisableAllSelections();

        countryCardSetters[countryIndex].SetMaskActive(false);
        countryCardSetters[countryIndex].SetSelectedBorderActive(true);
    }

    public void DisableAllSelections()
    {
        foreach (CountryCardSetter countryCardSetter in countryCardSetters)
        {
            countryCardSetter.SetSelectedBorderActive(false);
            countryCardSetter.SetMaskActive(true);
        }
    }

}
