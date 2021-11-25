using System;
using RTLTMPro;
using UnityEngine;

public class MainHeaderManager : MonoBehaviour
{
    public static MainHeaderManager Instance;

    [HideInInspector] public CustomDate gameDate = new CustomDate(0,0,0);

    public RTLTextMeshPro valueRTLTMP;
    public RTLTextMeshPro moneyRTLTMP;
    public RTLTextMeshPro gameDateText;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGameTimeResponseEvent += OnGameTimeReceived;
        EventManager.Instance.OnMoneyUpdateResponseEvent += OnMoneyUpdateReceived;
    }

    private void Start()
    {
        moneyRTLTMP.text = PlayerPrefs.GetFloat("Money").ToString();
        valueRTLTMP.text = PlayerPrefs.GetFloat("Value").ToString();
        SetDate();
    }

    private void OnGameTimeReceived(GameTimeResponse gameTimeResponse)
    {
        gameDate = gameTimeResponse.gameDate;
        SetDate();
    }

    private void OnMoneyUpdateReceived(MoneyUpdateResponse moneyUpdateResponse)
    {
        Money = moneyUpdateResponse.money;
        Value = moneyUpdateResponse.value;
    }

    public float Money
    {
        set
        {
            PlayerPrefs.SetFloat("Money", value);
            moneyRTLTMP.text = value.ToString();
        }

        get => PlayerPrefs.GetFloat("Money", 0f);
    }

    //TODO update value too
    public float Value
    {
        set
        {
            PlayerPrefs.SetFloat("Value", value);
            valueRTLTMP.text = value.ToString();
        }

        get => PlayerPrefs.GetFloat("Value", 0f);
    }

    private void SetDate()
    {
        gameDateText.text = gameDate.year + "/" +
                            gameDate.month.ToString().PadLeft(2, '0') + "/" + 
                            gameDate.day.ToString().PadLeft(2, '0');
    }
}