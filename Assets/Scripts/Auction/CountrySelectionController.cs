using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountrySelectionController : MonoBehaviour
{
    public CountryCardsManager cardManager;
    public GameObject[] cardSlots;
    
    private void Start()
    {
        DisplayCards();
    }

    private void DisplayCards()
    {
        for(int i = 0; i < cardManager.cards.Count; i++)
        {
            cardSlots[i].transform.GetChild(2).GetComponent<RTLTMPro.RTLTextMeshPro>().text = cardManager.cards[i].countryName;

            //cardSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = cardManager.cards[i].cardBg;
            cardSlots[i].transform.GetChild(1).GetComponent<Image>().sprite = cardManager.cards[i].blackMap;
            
        }
    }
}
