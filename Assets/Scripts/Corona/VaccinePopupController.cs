using System;
using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VaccinePopupController : MonoBehaviour
{
    public static VaccinePopupController Instance;

    
    public Button coronaButton;
    public RTLTextMeshPro collectedRatio;
    public TMP_InputField donateAmountInputField;
    public Button donateButton;
    public List<Slider> otherCountriesStatus;
    public Slider myCountryStatus;
    public Localize teamDonatedAmountText;
    
    private float _teamDonation = -1;

    public float TeamDonation
    {
        get => _teamDonation;
        set
        {
            _teamDonation = value;
            teamDonatedAmountText.SetKey("corona_donate_amount", _teamDonation.ToString());
        }
    }

    private void Awake()
    {
        Instance = this;
        TeamDonation = PlayerPrefs.GetFloat("DonatedAmount", 0f);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnDonateResponseEvent += OnGetDonateResponse;
        EventManager.Instance.OnCoronaInfoResponseEvent += OnGetCoronaInfoResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDonateResponseEvent -= OnGetDonateResponse;
        EventManager.Instance.OnCoronaInfoResponseEvent -= OnGetCoronaInfoResponse;
    }

    private void Start()
    {
        donateButton.onClick.AddListener(Donate);
        coronaButton.gameObject.SetActive(false);
    }
    
    public void CheckCorona()
    {
        if (!(GameDataManager.Instance.CoronaInfos is null))
        {
            SetData(GameDataManager.Instance.CoronaInfos);
            coronaButton.gameObject.SetActive(true);
        }
    }

    private void Donate()
    {
        if (string.IsNullOrEmpty(donateAmountInputField.text)) return;

        var amount = float.Parse(donateAmountInputField.text);
        if(amount == 0) return;
        if (amount > MainHeaderManager.Instance.Money)
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
            return;
        }

        if (amount < 0 || amount > RemainDonate())
        {
            DialogManager.Instance.ShowErrorDialog("invalid_donate_error");
            return;
        }

        var request = new DonateRequest(RequestTypeConstant.DONATE, amount);
        RequestManager.Instance.SendRequest(request);
        TeamDonation += amount;
    }

    private void OnGetDonateResponse(DonateResponse response)
    {
        if (response.result != "success")
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }
        var myCountry = PlayerPrefs.GetString("Country");
        foreach (var info in response.infos.Where(info => info.country.ToString() == myCountry))
        {
            NotificationsController.Instance.AddNewNotification("notification_donate", info.currentCollectedAmount.ToString("0.00"));
        }
        SetData(response.infos);
    }

    private void OnGetCoronaInfoResponse(CoronaInfoResponse response)
    {
        if(response.coronaInfos is null || response.coronaInfos.Count == 0) 
            return;

        SetData(response.coronaInfos);
        coronaButton.gameObject.SetActive(true);
    }


    private void SetData(List<Utils.CoronaInfoDto> infos)
    {
        var i = 0;
        var myCountry = PlayerPrefs.GetString("Country");
        foreach (var info in infos)
        {
            if (info.country.ToString() == myCountry)
            {
                collectedRatio.text = info.currentCollectedAmount + "/" + info.amountToBeCollect;
                myCountryStatus.maxValue = info.amountToBeCollect;
                myCountryStatus.value = info.currentCollectedAmount;
            }
            else
            {
                otherCountriesStatus[i].maxValue = info.amountToBeCollect;
                otherCountriesStatus[i].value = info.currentCollectedAmount;
                var percent = info.currentCollectedAmount / info.amountToBeCollect;
                otherCountriesStatus[i].GetComponentInChildren<Localize>()
                    .SetKey("corona_country_" + info.country, percent.ToString("P0"));
                i++;
            }
        }
    }

    private float RemainDonate()
    {
        var myCountry = PlayerPrefs.GetString("Country");
        var info = GameDataManager.Instance.CoronaInfos.FirstOrDefault(c => c.country.ToString() == myCountry);
        return info.amountToBeCollect - info.currentCollectedAmount;
    }
}