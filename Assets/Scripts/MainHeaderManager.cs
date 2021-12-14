using System;
using System.Collections;
using DG.Tweening;
using RTLTMPro;
using UnityEngine;

public class MainHeaderManager : MonoBehaviour
{
    public static MainHeaderManager Instance;

    [HideInInspector] public CustomDate gameDate = new CustomDate(0, 0, 0);
    [HideInInspector] public int weekNumber = 0;

    public RTLTextMeshPro valueRTLTMP;
    public RTLTextMeshPro moneyRTLTMP;

    public RTLTextMeshPro gameDateText;
    public Localize dayLocalize;
    public RTLTextMeshPro weekNumberText;

    public RTLTextMeshPro dayTimeText;

    public RTLTextMeshPro brandRTLTMP;

    private const float OneDayTime = 20; //seconds 
    private Tween dayTimer;


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGameTimeResponseEvent += OnGameTimeReceived;
        EventManager.Instance.OnMoneyUpdateResponseEvent += OnMoneyUpdateReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGameTimeResponseEvent -= OnGameTimeReceived;
        EventManager.Instance.OnMoneyUpdateResponseEvent -= OnMoneyUpdateReceived;
    }

    private void Start()
    {
        moneyRTLTMP.text = ((int) PlayerPrefs.GetFloat("Money")).ToString();
        valueRTLTMP.text = ((int) PlayerPrefs.GetFloat("Value")).ToString();
        brandRTLTMP.text = PlayerPrefs.GetFloat("Brand").ToString("0.00");

        SetDate();
    }

    private void OnGameTimeReceived(GameTimeResponse gameTimeResponse)
    {
        gameDate = gameTimeResponse.gameDate;
        weekNumber = gameTimeResponse.week;

        SetDate();
    }

    private void OnMoneyUpdateReceived(MoneyUpdateResponse moneyUpdateResponse)
    {
        Money = moneyUpdateResponse.money;
        Value = moneyUpdateResponse.value;
        Brand = moneyUpdateResponse.brand;

        VaccinePopupController.Instance.TeamDonation = moneyUpdateResponse.donatedAmount;
    }

    public float Money
    {
        set
        {
            PlayerPrefs.SetFloat("Money", value);
            moneyRTLTMP.text = ((int)value).ToString();
        }

        get => PlayerPrefs.GetFloat("Money", 0f);
    }

    //TODO update value too
    public float Value
    {
        set
        {
            PlayerPrefs.SetFloat("Value", value);
            valueRTLTMP.text = ((int)value).ToString();
        }

        get => PlayerPrefs.GetFloat("Value", 0f);
    }

    public float Brand
    {
        set
        {
            PlayerPrefs.SetFloat("Brand", value);
            brandRTLTMP.text = value.ToString("0.00");
        }

        get => PlayerPrefs.GetFloat("Brand", 0f);
    }

    private void SetDate()
    {
        gameDateText.text = gameDate.year + "/" +
                            gameDate.month.ToString().PadLeft(2, '0') + "/" +
                            gameDate.day.ToString().PadLeft(2, '0');

        dayLocalize.SetKey(gameDate.ToDateTime().DayOfWeek.ToString());

        weekNumberText.text = weekNumber + "/100";

        dayTimer.Kill();
        int nowTime = 0;
        dayTimer = DOTween.To(() => nowTime, x =>
        {
            nowTime = x;
            dayTimeText.text = (nowTime / 60).ToString("00") + ":" + (nowTime % 60).ToString("00");
        }, 1439, OneDayTime).SetEase(Ease.Linear);
        dayTimer.onComplete += () => { dayTimeText.text = "00:00"; };
    }

    public void OnSupportButtonClick()
    {
        Application.OpenURL("https://zil.ink/gamein_admin");
    }
}