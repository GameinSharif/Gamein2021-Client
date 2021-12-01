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
    private bool _isSendingTerminateOrAccept = false;

    private void OnEnable()
    {
        EventManager.Instance.OnTerminateOfferResponseEvent += OnTerminateOfferResponseRecieved;
        EventManager.Instance.OnAcceptOfferResponseEvent += OnAcceptOfferResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTerminateOfferResponseEvent -= OnTerminateOfferResponseRecieved;
        EventManager.Instance.OnAcceptOfferResponseEvent -= OnAcceptOfferResponse;
    }

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

            if (offer.offerStatus == Utils.OfferStatus.ACTIVE)
            {
                RemoveOfferButtonGameObject.SetActive(true);
            }
            else
            {
                RemoveOfferButtonGameObject.SetActive(false);
            }
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
        _isSendingTerminateOrAccept = false;
    }

    public void OnRemoveOfferButtonClick()
    {
        if (_isSendingTerminateOrAccept)
        {
            return;
        }
        
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                _isSendingTerminateOrAccept = true;
                TerminateOfferRequest terminateOfferRequest = new TerminateOfferRequest(RequestTypeConstant.TERMINATE_OFFER, _offer.id);
                RequestManager.Instance.SendRequest(terminateOfferRequest);
            }
        });
        
    }

    private void OnTerminateOfferResponseRecieved(TerminateOfferResponse terminateOfferResponse)
    {
        if (_offer.id == terminateOfferResponse.terminatedOfferId)
        {
            offerStatusLocalize.SetKey("TERMINATED");
            RemoveOfferButtonGameObject.SetActive(false);
        }
    }

    public void OnAcceptOfferButtonClick()
    {
        if (_isSendingTerminateOrAccept)
        {
            return;
        }
        
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                _isSendingTerminateOrAccept = true;
                AcceptOfferRequest acceptOfferRequest = new AcceptOfferRequest(_offer.id);
                RequestManager.Instance.SendRequest(acceptOfferRequest);
            }
        });
    }

    private void OnAcceptOfferResponse(AcceptOfferResponse acceptOfferResponse)
    {
        if (_offer.id == acceptOfferResponse.acceptedOffer.id)
        {
            offerStatusLocalize.SetKey("ACCEPTED");
            AcceptOfferButtonGameObject.SetActive(false);
            if (acceptOfferResponse.acceptedOffer.teamId != PlayerPrefs.GetInt("TeamId"))
            {
                OffersController.Instance.AddAcceptedOfferToList(acceptOfferResponse.acceptedOffer);
                MainHeaderManager.Instance.Money -= (int)(acceptOfferResponse.acceptedOffer.volume * acceptOfferResponse.acceptedOffer.costPerUnit);
            }
            else
            {
                MainHeaderManager.Instance.Money += (int)(acceptOfferResponse.acceptedOffer.volume * acceptOfferResponse.acceptedOffer.costPerUnit);
            }
            NotificationsController.Instance.AddNewNotification("notification_offer_accepted",
                GameDataManager.Instance.GetProductById(acceptOfferResponse.acceptedOffer.productId).name);
        }
    }
}
