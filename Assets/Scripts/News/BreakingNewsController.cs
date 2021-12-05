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

    public void SetInfo(Utils.News newspaper)
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
    }
}
