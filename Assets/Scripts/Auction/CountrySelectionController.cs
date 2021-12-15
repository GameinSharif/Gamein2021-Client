using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
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

    public Localize luckyCountry;
    public List<CountryCardSetter> countryCardSetters;
    public List<CountryCard> cards;
    public Button goToMapButton;
    public Button startRandomizeProcessButton;

    private List<string> _countryNameLocalizeKeys = new List<string>();
    private List<Vector2> _countryCapitalsLocaltion = new List<Vector2>();
    private int _countryIndex;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        FillCountryNameLocalizeKeyList();
        DisplayCards();
        countrySelectionCanvas.SetActive(true);

        if (PlayerPrefs.HasKey("SeenRandomizeProcess"))
        {
            ShowTheActualCountry();
        }
        else
        {
            goToMapButton.gameObject.SetActive(false);
            startRandomizeProcessButton.gameObject.SetActive(true);
        }
    }

    private void FillCountryNameLocalizeKeyList()
    {
        _countryNameLocalizeKeys.Add("auction_france");
        _countryNameLocalizeKeys.Add("auction_germany");
        _countryNameLocalizeKeys.Add("auction_united_kingdom");
        _countryNameLocalizeKeys.Add("auction_netherlands");
        _countryNameLocalizeKeys.Add("auction_belgium");
        _countryNameLocalizeKeys.Add("auction_switzerland");

        _countryCapitalsLocaltion.Add(new Vector2(48.8586f, 2.2885f));
        _countryCapitalsLocaltion.Add(new Vector2(52.4426f, 13.4252f));
        _countryCapitalsLocaltion.Add(new Vector2(51.5557f, -0.1052f));
        _countryCapitalsLocaltion.Add(new Vector2(52.3707f, 4.8997f));
        _countryCapitalsLocaltion.Add(new Vector2(47.3760f, 8.5748f));
        _countryCapitalsLocaltion.Add(new Vector2(43.2732f, 5.4162f));
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
        
        SFXManager.Instance.Play(SFXManager.SfxID.RANDOMIZE_COUNTRY);
        StartCoroutine(ShowCountryCoroutine());
    }
    
    private IEnumerator ShowCountryCoroutine()
    {
        for (int i=0;i < 50; i++)
        {
            int randomCountryIndex = Random.Range(0, Enum.GetNames(typeof(Utils.Country)).Length);
            ShowRandomlySelectedCountry(randomCountryIndex);

            yield return new WaitForSeconds(0.1f);
        }

        ShowTheActualCountry();
    }

    private void ShowTheActualCountry()
    {
        Enum.TryParse(PlayerPrefs.GetString("Country"), out Utils.Country country);
        _countryIndex = (int)country;

        ShowRandomlySelectedCountry(_countryIndex);

        goToMapButton.gameObject.SetActive(true);
        startRandomizeProcessButton.gameObject.SetActive(false);
        luckyCountry.SetKey(_countryNameLocalizeKeys[_countryIndex]);
        PlayerPrefs.SetString("SeenRandomizeProcess", "True");
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

    public void OnGoToMapButtonClick()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        //MapManager.SnapToLocaltionOnOpenMap = _countryCapitalsLocaltion[_countryIndex];
        MapManager.Instance.SnapToLocation(_countryCapitalsLocaltion[_countryIndex]);

        //MainMenuManager.Instance.OpenPage(0);

        countrySelectionCanvas.SetActive(false);
    }

}
