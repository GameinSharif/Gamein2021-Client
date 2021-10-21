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
    public class CountryCard
    {
        public Sprite blackMap;
        public Sprite cardBg;

        public Utils.Country country;
    }

    public class CountryCardsManager
    {
        public List<CountryCard> cards = new List<CountryCard>();
    }

    public CountryCardsManager cardManager;
    public GameObject[] cardSlots;
    public GameObject loadingCircle;
    public Button goToMapButton;
    public Button getCountryButton;
    public GameObject countrySelectionCanvas;

    private List<string> _countryNameLocalizeKeys = new List<string>();

    private void Start()
    {
        goToMapButton.gameObject.SetActive(false);
        getCountryButton.gameObject.SetActive(true);
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
        for(int i = 0; i < cardManager.cards.Count; i++)
        {
            cardSlots[i].transform.GetChild(3).GetComponent<RTLTextMeshPro>().text = cardManager.cards[i].country.ToString();
            cardSlots[i].transform.GetChild(3).GetComponent<Localize>().SetKey(_countryNameLocalizeKeys[i]);
            //cardSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = cardManager.cards[i].cardBg;
            cardSlots[i].transform.GetChild(2).GetComponent<Image>().sprite = cardManager.cards[i].blackMap;
            cardSlots[i].transform.GetChild(0).gameObject.SetActive(false);
            cardSlots[i].transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    public void OnGetCountryButtonClicked()
    {
        loadingCircle.gameObject.SetActive(true);
        
        for(int i = 0; i < cardManager.cards.Count; i++)
        {
            cardSlots[i].transform.GetChild(4).gameObject.SetActive(true);
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
        cardSlots[countryIndex].transform.GetChild(4).gameObject.SetActive(false);
        cardSlots[countryIndex].transform.GetChild(0).gameObject.SetActive(true);
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
            if (cardSlot.transform.GetChild(3).GetComponent<RTLTextMeshPro>().text == countryName)
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    public void onGoToMapButtonClicked()
    {
        countrySelectionCanvas.SetActive(false);
        SceneManager.LoadSceneAsync("MapScene", LoadSceneMode.Additive);
    }
    
}
