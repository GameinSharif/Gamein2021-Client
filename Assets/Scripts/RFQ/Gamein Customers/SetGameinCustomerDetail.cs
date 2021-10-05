using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class SetGameinCustomerDetail: MonoBehaviour
{
    [HideInInspector] public RFQUtils.GameinCustomer GameinCustomerData;

    public RTLTextMeshPro RowNumberTxt;
    public RTLTextMeshPro GameinCustomerNameTxt;

    public void InitializeGameinCustomer(RFQUtils.GameinCustomer gameinCustomer)
    {
        GameinCustomerData = gameinCustomer;

        RowNumberTxt.text = gameinCustomer.id.ToString();
        GameinCustomerNameTxt.text = gameinCustomer.name;
    }

    public void OnShowOnMapButtonClick()
    {
        //TODO
    }

    public void OnShowDemandsButtonClick()
    {
        //TODO
    }
}