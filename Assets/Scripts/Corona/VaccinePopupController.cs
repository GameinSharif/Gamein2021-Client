using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VaccinePopupController : MonoBehaviour
{
    public Button coronaButton;
    
    public TMP_InputField donateAmount;
    public Button donateButton;
    public List<Slider> otherCountriesStatus;
    public Slider myCountryStatus;

    private void Start()
    {
        EventManager.Instance.OnDonateResponseEvent += OnGetDonateResponse;
        EventManager.Instance.OnCoronaInfoResponseEvent += OnGetCoronaInfoResponse;
        donateButton.onClick.AddListener(Donate);
        coronaButton.gameObject.SetActive(!(GameDataManager.Instance.CoronaInfos is null));
    }

    private void Donate()
    {
        if (string.IsNullOrEmpty(donateAmount.text)) return;

        var amount = float.Parse(donateAmount.text);
        if (amount > MainHeaderManager.Instance.Money)
        {
            DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
            return;
        }

        var request = new DonateRequest(RequestTypeConstant.DONATE, amount);
        RequestManager.Instance.SendRequest(request);
    }

    private void OnGetDonateResponse(DonateResponse response)
    {
        if (response.result != "success")
        {
            DialogManager.Instance.ShowErrorDialog();
            return;
        }

        SetData(response.infos);
    }

    private void OnGetCoronaInfoResponse(CoronaInfoResponse response)
    {
        SetData(response.infos);
    }


    private void SetData(List<Utils.CoronaInfoDto> infos)
    {
        var i = 0;
        var myCountry = PlayerPrefs.GetString("Country");
        foreach (var info in infos)
        {
            if (info.country.ToString() == myCountry)
            {
                myCountryStatus.maxValue = info.amountToBeCollect;
                myCountryStatus.value = info.currentCollectedAmount;
            }
            else
            {
                otherCountriesStatus[i].maxValue = info.amountToBeCollect;
                otherCountriesStatus[i].value = info.currentCollectedAmount;
                otherCountriesStatus[i].GetComponentInChildren<Localize>()
                    .SetKey("auction_" + info.country.ToString().ToLower());
                i++;
            }
        }
    }
}