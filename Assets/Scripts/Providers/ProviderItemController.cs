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
        string company,
        string type,
        string maxMonthlyCap,
        string providerAverageCost,
        string providerMinOnRecord,
        string providerMaxOnRecord)
    {
        this.no.text = no.ToString();
        this.company.text = company;
        this.type.text = type;
        this.maxMonthlyCap.text = maxMonthlyCap;
        this.providerAverageCost.text = providerAverageCost;
        this.providerMinOnRecord.text = providerMinOnRecord;
        this.providerMaxOnRecord.text = providerMaxOnRecord;
    }

    public void SetInfo(int no, Provider provider)
    {
        SetInfo(
            no: no,
            company: provider.Company,
            type: provider.Type,
            maxMonthlyCap: provider.MaxMonthlyCap.ToString(),
            providerAverageCost: provider.ProviderAverageCost.ToString(),
            providerMinOnRecord: provider.ProviderMinOnRecord.ToString(),
            providerMaxOnRecord: provider.ProviderMaxOnRecord.ToString()
        );
    }
    
    public void OnSendOfferClicked()
    {
        // TODO
    }
}
