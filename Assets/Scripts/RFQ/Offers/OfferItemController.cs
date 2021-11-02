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
    public GameObject offerStatusGameObject;
    public Localize offerStatusLocalize;

    public GameObject RemoveOfferButtonGameObject;
    public GameObject AcceptOfferButtonGameObject;

    private Utils.Offer _offer;

    public void SetInfo(int no, string teamName, string productNameKey, int volume, float costPerUnit, CustomDate offerDeadline, Utils.OfferStatus offerStatus)
    {
        offerStatusLocalize.SetKey(offerStatus.ToString());

        SetInfo(no, teamName, productNameKey, volume, costPerUnit, offerDeadline);
    }

    public void SetInfo(int no, string teamName, string productNameKey, int volume, float costPerUnit, CustomDate offerDeadline)
    {
        this.no.text = no.ToString();
        this.teamName.text = teamName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.volume.text = volume.ToString();
        this.costPerUnit.text = costPerUnit.ToString("0.00");
        totalPrice.text = (volume * costPerUnit).ToString("0.00");
        this.offerDeadline.text = offerDeadline.ToString();
    }

    public void SetInfo(int no, Utils.Offer offer)
    {
        if (PlayerPrefs.GetInt("TeamId") == offer.teamId)
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

            RemoveOfferButtonGameObject.SetActive(true);
            AcceptOfferButtonGameObject.SetActive(false);
        }
        else
        {
            SetInfo(
                no: no,
                teamName: GameDataManager.Instance.GetTeamName(offer.teamId),
                productNameKey: GameDataManager.Instance.GetProductById(offer.productId).name,
                volume: offer.volume,
                costPerUnit: offer.costPerUnit,
                offerDeadline: offer.offerDeadline
            );

            offerStatusGameObject.SetActive(false);
            RemoveOfferButtonGameObject.SetActive(false);
            AcceptOfferButtonGameObject.SetActive(true);
        }

        _offer = offer;
    }

    public void OnAcceptOfferButtonClick()
    {
        //TODO
    }

    public void OnRemoveOfferButtonClick()
    {
        //TODO
    }
}
