using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyNewsController : MonoBehaviour
{
    public Image newsImage;
    public RTLTextMeshPro mainTitle;
    public RTLTextMeshPro mainText;
    public RTLTextMeshPro subTexts1;
    public RTLTextMeshPro subTexts2;
    public RTLTextMeshPro subTexts3;
    public RTLTextMeshPro newsPaperNo;
    public GameObject forWeekly;
    public GameObject forSerious;
    public GameObject navigateButtons;

    public void SetInfo(Utils.News newspaper)
    {
        if (LocalizationManager.GetCurrentLanguage() == LocalizationManager.LocalizedLanguage.English)
        {
            mainTitle.text = newspaper.mainTitleEng;
            mainText.text = newspaper.mainTextEng;
            subTexts1.text = newspaper.subTextsEng1;
            subTexts2.text = newspaper.subTextsEng2;
            subTexts3.text = newspaper.subTextsEng3;
        }
        else
        {
            mainTitle.text = newspaper.mainTitleFa;
            mainText.text = newspaper.mainTextFa;
            subTexts1.text = newspaper.subTextsFa1;
            subTexts2.text = newspaper.subTextsFa2;
            subTexts3.text = newspaper.subTextsFa3;
        }

        newsImage.sprite = GameDataManager.Instance.NewsSprites[newspaper.imageIndex];
        newsPaperNo.text = newspaper.id.ToString();

        navigateButtons.SetActive(true);
        forWeekly.SetActive(true);
        forSerious.SetActive(false);
        newsPaperNo.gameObject.SetActive(true);
    }
    
}
