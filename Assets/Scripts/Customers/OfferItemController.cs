using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;


public class OfferItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro company;
    public RTLTextMeshPro type;
    public RTLTextMeshPro volume;
    public RTLTextMeshPro costPerUnit;
    public RTLTextMeshPro total;
    public RTLTextMeshPro EEA;
    public RTLTextMeshPro LEA;
    public RTLTextMeshPro deadline;
    public RTLTextMeshPro frequency;

    public void SetInfo(
        int no,
        string company,
        string type,
        string volume,
        string costPerUnit,
        string total,
        string EEA,
        string LEA,
        string deadline,
        string frequency)
    {
        this.no.text = no.ToString();
        this.company.text = company;
        this.type.text = type;
        this.volume.text = volume;
        this.costPerUnit.text = costPerUnit;
        this.total.text = total;
        this.EEA.text = EEA;
        this.LEA.text = LEA;
        this.deadline.text = deadline;
        this.frequency.text = frequency;
    }

    public void SetInfo(int no, OfferViewModel offerViewModel)
    {
        SetInfo(
            no: no,
            company: offerViewModel.Company,
            type: offerViewModel.Type,
            volume: offerViewModel.Volume.ToString(),
            costPerUnit: offerViewModel.CostPerUnit.ToString(),
            total: offerViewModel.TotalCost.ToString(),
            EEA: offerViewModel.EEA,
            LEA: offerViewModel.LEA,
            deadline: offerViewModel.Deadline,
            frequency: offerViewModel.Frequency.ToString()
        );
    }


    public void OnAcceptOfferClicked()
    {
        // TODO
    }
}
