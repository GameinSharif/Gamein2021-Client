using System;
using RTLTMPro;
using UnityEngine;

public class MainHeaderManager : MonoBehaviour
{

    public static MainHeaderManager Instance;

    public CustomDate gameDate = new CustomDate(0,0,0);

    public GameObject header;
    public RTLTextMeshPro moneyText;
    public RTLTextMeshPro gameDateText;

    private void Awake()
    {
        Instance = this;
        EventManager.Instance.OnGameTimeResponseEvent += OnGameTimeReceived;
        EventManager.Instance.OnMoneyUpdateResponseEvent += OnMoneyUpdateReceived;
    }

    private void Start()
    {
        DontDestroyOnLoad(header);
        SetMoney();
        SetDate();
    }

    private void OnGameTimeReceived(GameTimeResponse gameTimeResponse)
    {
        gameDate = gameTimeResponse.gameDate;
        SetDate();
    }

    private void OnMoneyUpdateReceived(MoneyUpdateResponse moneyUpdateResponse)
    {
        PlayerPrefs.SetInt("Money", moneyUpdateResponse.money);
        SetMoney();
    }

    private void SetMoney()
    {
        moneyText.text = PlayerPrefs.GetInt("Money", 0) + "$";
    }

    private void SetDate()
    {
        gameDateText.text = gameDate.year + "\\" +
                            gameDate.month.ToString().PadLeft(2, '0') + "\\" + 
                            gameDate.day.ToString().PadLeft(2, '0');
    }
}