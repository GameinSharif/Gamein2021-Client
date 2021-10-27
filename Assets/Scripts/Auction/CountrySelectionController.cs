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
    [Serializable]
    public class CountryCard
    {
        public Sprite blackMap;
        public Sprite cardBg;
        public Utils.Country country;
    }

    public GameObject[] cardSlots;
    public List<CountryCard> cards = new List<CountryCard>();
    public GameObject loadingCircle;
    public Button goToMapButton;
    public Button getCountryButton;
    public GameObject countrySelectionCanvas;

    private List<string> _countryNameLocalizeKeys = new List<string>();

    private void Awake()
    {
        goToMapButton.gameObject.SetActive(false);
        goToMapButton.GetComponent<Button>().onClick.AddListener(delegate { onGoToMapButtonClicked(); });
        getCountryButton.gameObject.SetActive(true);
        getCountryButton.GetComponent<Button>().onClick.AddListener(delegate { OnGetCountryButtonClicked(); });
        FillCountryNameLocalizeKeyList();
        DisplayCards();
    }

    private void FillCountryNameLocalizeKeyList()
    {
        _countryNameLocalizeKeys.Add("auction_france");
        _countryNameLocalizeKeys.Add("auction_germany");
        _countryNameLocalizeKeys.Add("auction_england");
        _countryNameLocalizeKeys.Add("auction_netherlands");
        _countryNameLocalizeKeys.Add("auction_belgium");
        _countryNameLocalizeKeys.Add("auction_switzerland");
    }
        
    private void DisplayCards()
    {
        for(int i = 0; i < cards.Count; i++)
        {
            string countryName = cards[i].country.ToString();
            string localizeKey = _countryNameLocalizeKeys[i];
            Sprite cardBg = cards[i].cardBg;
            Sprite blackMap = cards[i].blackMap;
            cardSlots[i].GetComponent<CountryCardSetter>().SetAllValues(countryName, localizeKey, blackMap, cardBg);
        }
    }

    void OnGetCountryButtonClicked()
    {
        loadingCircle.gameObject.SetActive(true);
        
        for(int i = 0; i < cards.Count; i++)
        {
            cardSlots[i].GetComponent<CountryCardSetter>().SetMaskActive(true);
        }
        
        StartCoroutine(ShowCountryCoroutine());
    }
    
    IEnumerator ShowCountryCoroutine()
    {
        yield return new WaitForSeconds(10);

        int countryIndex = GetTeamsCountry();
        if (countryIndex < 0)
        {
            //TODO get country from server again
        }
        cardSlots[countryIndex].GetComponent<CountryCardSetter>().SetMaskActive(false);
        cardSlots[countryIndex].GetComponent<CountryCardSetter>().SetSelectedBorderActive(true);
        loadingCircle.gameObject.SetActive(false);
        goToMapButton.gameObject.SetActive(true);
        getCountryButton.gameObject.SetActive(false);
        PlayerPrefs.SetInt("IsFirstTime", 0);
    }

    private int GetTeamsCountry()
    {
        int index = 0;
        string countryName = PlayerPrefs.GetString("Country");
        foreach (GameObject cardSlot in cardSlots)
        {
            if (cardSlot.GetComponent<CountryCardSetter>().GetCountryName() == countryName)
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    void onGoToMapButtonClicked()
    {
        countrySelectionCanvas.SetActive(false);
        SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
    }
    
}
