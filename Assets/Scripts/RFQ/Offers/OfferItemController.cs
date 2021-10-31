using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class OfferItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro teamName;
    public Localize productNameLocalize;
    public RTLTextMeshPro volume;
    public RTLTextMeshPro costPerUnit;
    public RTLTextMeshPro totalPrice;
    public RTLTextMeshPro offerDeadline;
    public Localize OfferStatusLocalize;

    public GameObject RemoveOfferButtonGameObject;
    public GameObject AcceptOfferButtonGameObject;

    private Utils.Offer _offer;

    public void SetInfo(int no, string teamName, string productNameKey, int volume, float costPerUnit, CustomDate offerDeadline, Utils.OfferStatus offerStatus)
    {
        this.no.text = no.ToString();
        this.teamName.text = teamName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.volume.text = volume.ToString();
        this.costPerUnit.text = costPerUnit.ToString("0.00");
        totalPrice.text = (volume * costPerUnit).ToString("0.00");
        this.offerDeadline.text = offerDeadline.ToString();

        //TODO OfferStatus
    }

    public void SetInfo(int no, Utils.Offer offer)
    {
        SetInfo(
            no: no,
            teamName: GameDataManager.Instance.GetTeamName(offer.teamId),
            productNameKey: GameDataManager.Instance.GetProductById(offer.productId).name,
            volume: offer.volume,
            costPerUnit: offer.costPerUnit,
            offerDeadline: offer.offerDeadline,
            offerStatus: offer.offerStatus
        );

        if (PlayerPrefs.GetInt("TeamId") == offer.teamId)
        {
            RemoveOfferButtonGameObject.SetActive(true);
            AcceptOfferButtonGameObject.SetActive(false);
        }
        else
        {
            RemoveOfferButtonGameObject.SetActive(false);
            AcceptOfferButtonGameObject.SetActive(true);
        }

        _offer = offer;
    }


    public void OnAcceptOfferClicked()
    {
        // TODO
    }
}
