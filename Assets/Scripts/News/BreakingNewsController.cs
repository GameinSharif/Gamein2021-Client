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
    public GameObject forWeekly;
    public GameObject forSerious;
    public GameObject navigateButtons;
    public RTLTextMeshPro newsNo;

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
        forWeekly.SetActive(false);
        forSerious.SetActive(true);

        if (isSingle)
        {
            newsNo.gameObject.SetActive(false);
            navigateButtons.SetActive(false);        
        }
        else
        {
            newsNo.gameObject.SetActive(true);
            navigateButtons.SetActive(true);
            newsNo.text = newspaper.id.ToString();
        }
    }
}
