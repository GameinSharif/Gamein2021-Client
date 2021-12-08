using System;
using RTLTMPro;
using UnityEngine;

public class MainHeaderManager : MonoBehaviour
{
    public static MainHeaderManager Instance;

    [HideInInspector] public CustomDate gameDate = new CustomDate(0,0,0);
    [HideInInspector] public int weekNumber = 0;

    public RTLTextMeshPro valueRTLTMP;
    public RTLTextMeshPro moneyRTLTMP;

    public RTLTextMeshPro gameDateText;
    public Localize dayLocalize;
    public RTLTextMeshPro weekNumberText;

    public RTLTextMeshPro brandRTLTMP;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGameTimeResponseEvent += OnGameTimeReceived;
        EventManager.Instance.OnMoneyUpdateResponseEvent += OnMoneyUpdateReceived;
    }

    private void Start()
    {
        moneyRTLTMP.text = PlayerPrefs.GetFloat("Money").ToString("0.00");
        valueRTLTMP.text = PlayerPrefs.GetFloat("Value").ToString("0.00");
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

        //TODO donated amount

        if (MapManager.IsInMap)
        {
            MapManager.Instance.CashForAuction.text = Money.ToString("0.00");
        }
    }

    public float Money
    {
        set
        {
            PlayerPrefs.SetFloat("Money", value);
            moneyRTLTMP.text = value.ToString("0.00");
        }

        get => PlayerPrefs.GetFloat("Money", 0f);
    }

    //TODO update value too
    public float Value
    {
        set
        {
            PlayerPrefs.SetFloat("Value", value);
            valueRTLTMP.text = value.ToString("0.00");
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
    }
}