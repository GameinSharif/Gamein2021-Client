using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakingNewsController : MonoBehaviour
{
    public Image newsImage;
    public RTLTextMeshPro mainTitle;
    public RTLTextMeshPro mainText;
    public RTLTextMeshPro newsDate;
    public GameObject forWeekly;
    public GameObject navigateButtons;
    public RTLTextMeshPro newsNo;
    public Localize headerLocalize;

    public void SetInfo(Utils.News newspaper, bool isSingle)
    {
        if (LocalizationManager.GetCurrentLanguage() == LocalizationManager.LocalizedLanguage.English)
        {
            mainTitle.text = newspaper.mainTitleEng;
            mainText.text = newspaper.mainTextEng;
        }
        else
        {
            mainTitle.text = newspaper.mainTitleFa;
            mainText.text = newspaper.mainTextFa;
        }
        newsImage.sprite = GameDataManager.Instance.NewsSprites[newspaper.imageIndex];
        newsDate.SetText(MainHeaderManager.Instance.gameDate.ToString());
        headerLocalize.SetKey("breaking_news_title");
        if (isSingle)
        {
            navigateButtons.SetActive(false);
            forWeekly.SetActive(false);
            newsNo.gameObject.SetActive(false);
        }
        else
        {
            newsNo.gameObject.SetActive(true);
            navigateButtons.SetActive(true);
            forWeekly.SetActive(false);
            newsNo.text = newspaper.week.ToString();
        }
    }
}
