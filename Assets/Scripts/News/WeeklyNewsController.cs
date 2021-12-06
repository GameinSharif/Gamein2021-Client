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
    public RTLTextMeshPro newsDate;

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
        newsPaperNo.text = newspaper.week.ToString();
        //TODO game start date
        CustomDate newspaperDate = new CustomDate(2021, 12, 24).AddDays(newspaper.week * 7);
        newsDate.text = newspaperDate.ToString();
    }
    
}
