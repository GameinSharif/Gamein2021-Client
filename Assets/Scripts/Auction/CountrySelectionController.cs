using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CountrySelectionController : MonoBehaviour
{
    public CountryCardsManager cardManager;
    public GameObject[] cardSlots;
    public GameObject loadingCircle;

    private List<string> countryNameLocalizeKey = new List<string>();

    private void Start()
    {
        FillCountryNameLocalizeKeyList();
        DisplayCards();
    }

    private void FillCountryNameLocalizeKeyList()
    {
        countryNameLocalizeKey.Add("auction_france");
        countryNameLocalizeKey.Add("auction_germany");
        countryNameLocalizeKey.Add("auction_england");
        countryNameLocalizeKey.Add("auction_netherlands");
        countryNameLocalizeKey.Add("auction_belgium");
        countryNameLocalizeKey.Add("auction_switzerland");
    }
        
    private void DisplayCards()
    {
        for(int i = 0; i < cardManager.cards.Count; i++)
        {
            cardSlots[i].transform.GetChild(3).GetComponent<RTLTextMeshPro>().text = cardManager.cards[i].countryName;
            cardSlots[i].transform.GetChild(3).GetComponent<Localize>().SetKey(countryNameLocalizeKey[i]);
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
        cardSlots[countryIndex].transform.GetChild(4).gameObject.SetActive(false);
        cardSlots[countryIndex].transform.GetChild(0).gameObject.SetActive(true);
        loadingCircle.gameObject.SetActive(false);
    }

    private int GetTeamsCountry()
    {
        //TODO get it from server
        return Random.Range(0, cardManager.cards.Count);
    }
    
}
