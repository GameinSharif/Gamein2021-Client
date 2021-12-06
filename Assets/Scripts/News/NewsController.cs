using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsController : MonoBehaviour
{
    public static NewsController Instance;

    public GameObject newsPaperCanvas;
    public GameObject breakingNewsCanvas;
    public GameObject newNewspaperImage;

    private List<Utils.News> _allWeeklyNews;
    private int _newsIndex;
    private WeeklyNewsController _weeklyNewsController;

    private void Awake()
    {
        Instance = this;
    }

    public void OnShowPreviousNewspaperButtonClick()
    {
        _allWeeklyNews = GameDataManager.Instance.GetAllWeeklyNews();
        _newsIndex = _newsIndex == 0 ? _allWeeklyNews.Count - 1 : _newsIndex - 1;
        _weeklyNewsController.SetInfo(_allWeeklyNews[_newsIndex]);
    }
    
    public void OnShowNextNewspaperButtonClick()
    {
        _allWeeklyNews = GameDataManager.Instance.GetAllWeeklyNews();
        _newsIndex = _newsIndex == _allWeeklyNews.Count - 1 ? 0 : _newsIndex + 1;
        _weeklyNewsController.SetInfo(_allWeeklyNews[_newsIndex]);
    }

    public void OnShowNewsButtonClick()
    {
        newNewspaperImage.SetActive(false);
        //int lastNewspaperNo = PlayerPrefs.GetInt("LastNewsPaperNo", 0);
        _allWeeklyNews = GameDataManager.Instance.GetAllWeeklyNews();
        if (_allWeeklyNews.Count > 0)
        {
            _newsIndex = _allWeeklyNews.Count - 1;
            _weeklyNewsController = newsPaperCanvas.GetComponent<WeeklyNewsController>();
            PlayerPrefs.SetInt("LastNewsPaperNo", _newsIndex + 1);
            _weeklyNewsController.SetInfo(_allWeeklyNews[_newsIndex]);
            newsPaperCanvas.SetActive(true);
        }
    }

    public void OnBreakingNewsReceived(Utils.News news)
    {
        BreakingNewsController controller = breakingNewsCanvas.GetComponent<BreakingNewsController>();
        controller.SetInfo(news);
        breakingNewsCanvas.SetActive(true);
    }

    public void SetNewNewspaperImageActive()
    {
        newNewspaperImage.SetActive(true);
    }

    public void Test1()
    {
        GameDataManager.Instance.TestButton1();
    }
    
    public void Test2()
    {
        Debug.Log("in Test2");
        GameDataManager.Instance.TestButton2();
    }
    
}
