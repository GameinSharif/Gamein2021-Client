using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsController : MonoBehaviour
{
    public static NewsController Instance;

    public GameObject newsCanvas;
    public GameObject newNewspaperImage;
    public Button newspapersButton;

    private List<Utils.News> _allNews;
    private int _newsIndex;
    private WeeklyNewsController _weeklyNewsController;
    private BreakingNewsController _breakingNewsController;

    private void Awake()
    {
        Instance = this;
    }

    public void OnShowPreviousNewspaperButtonClick()
    {
        _allNews = GameDataManager.Instance.News;
        _newsIndex = _newsIndex == 0 ? _allNews.Count - 1 : _newsIndex - 1;
        if (_allNews[_newsIndex].newsType == Utils.NewsType.COMMON)
        {
            _weeklyNewsController.SetInfo(_allNews[_newsIndex]);
        }
        else
        {
            _breakingNewsController.SetInfo(_allNews[_newsIndex], false);
        }    
    }
    
    public void OnShowNextNewspaperButtonClick()
    {
        _allNews = GameDataManager.Instance.News;
        _newsIndex = _newsIndex == _allNews.Count - 1 ? 0 : _newsIndex + 1;
        if (_allNews[_newsIndex].newsType == Utils.NewsType.COMMON)
        {
            _weeklyNewsController.SetInfo(_allNews[_newsIndex]);
        }
        else
        {
            _breakingNewsController.SetInfo(_allNews[_newsIndex], false);
        }
    }

    public void OnShowNewsButtonClick()
    {
        newNewspaperImage.SetActive(false);
        _allNews = GameDataManager.Instance.News;
        if (_allNews.Count > 0)
        {
            _newsIndex = _allNews.Count - 1;
            _weeklyNewsController = newsCanvas.GetComponent<WeeklyNewsController>();
            _breakingNewsController = newsCanvas.GetComponent<BreakingNewsController>();
            PlayerPrefs.SetInt("LastNewsPaperNo", _newsIndex + 1);
            if (_allNews[_newsIndex].newsType == Utils.NewsType.COMMON)
            {
                _weeklyNewsController.SetInfo(_allNews[_newsIndex]);
            }
            else
            {
                _breakingNewsController.SetInfo(_allNews[_newsIndex], false);
            }
            newsCanvas.SetActive(true);
        }
    }

    public void OnBreakingNewsReceived(Utils.News news)
    {
        SFXManager.Instance.Play(SFXManager.SfxID.NEWS_NOTIFICATION);
        BreakingNewsController controller = newsCanvas.GetComponent<BreakingNewsController>();
        controller.SetInfo(news, true);
        newsCanvas.SetActive(true);
    }

    public void SetNewNewspaperImageActive()
    {
        newNewspaperImage.SetActive(true);
    }
    
}
