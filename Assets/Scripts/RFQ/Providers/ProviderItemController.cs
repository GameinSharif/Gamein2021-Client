using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ProviderItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro company;
    public RTLTextMeshPro type;
    public RTLTextMeshPro maxMonthlyCap;
    public RTLTextMeshPro providerAverageCost;
    public RTLTextMeshPro providerMinOnRecord;
    public RTLTextMeshPro providerMaxOnRecord;

    private void SetInfo(
        int no,
        string teamName,
        string productName,
        string capacity,
        string averagePrice,
        string minPriceOnRecord,
        string maxPriceOnRecord)
    {
        this.no.text = no.ToString();
        this.company.text = teamName;
        this.type.text = productName;
        this.maxMonthlyCap.text = capacity;
        this.providerAverageCost.text = averagePrice;
        this.providerMinOnRecord.text = minPriceOnRecord;
        this.providerMaxOnRecord.text = maxPriceOnRecord;
    }

    public void SetInfo(int no, Utils.Provider provider)
    {
        SetInfo(
            no: no,
            teamName: provider.team.teamName,
            productName: GameDataManager.Instance.Products[provider.productId].name,
            capacity: provider.capacity.ToString(),
            averagePrice: provider.averagePrice.ToString(),
            minPriceOnRecord: provider.minPriceOnRecord.ToString(),
            maxPriceOnRecord: provider.maxPriceOnRecord.ToString()
        );
    }
    
    public void OnSendOfferClicked()
    {
        // TODO
    }
}
