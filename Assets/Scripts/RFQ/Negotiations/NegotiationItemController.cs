using System;
using System.Collections;
using UnityEngine;
using RTLTMPro;
using TMPro;
using UnityEngine.UI;

public class NegotiationItemController : MonoBehaviour
{
    public RTLTextMeshPro supplierOrDemander;
    public Localize productNameLocalize;
    public RTLTextMeshPro amount;
    public RTLTextMeshPro demanderCostPerUnit;
    public RTLTextMeshPro supplierCostPerUnit;
    public Localize negotiationStatusLocalize;
    public Localize storageOrDistanceLocalize;

    public Image mainImage;
    public Sprite closedSprite;
    public Sprite openedSprite;

    public TMP_InputField pricePerUnitInputField;

    public GameObject onOpenObjects;
    public Toggle showActionToggle;

    public Utils.Negotiation Negotiation => _negotiation;

    private Utils.Negotiation _negotiation;
    private bool _isSupply;
    private Utils.Team _supplierOrDemanderTeam;
    private Utils.Product _product;
    private Tuple<int, bool> _storageDetails;

    private bool _isSendingRequest = false;

    public void Initialize(Utils.Negotiation negotiation, bool isSupply)
    {
        _isSupply = isSupply;
        _negotiation = negotiation;
        _product = GameDataManager.Instance.GetProductById(_negotiation.productId);
        _storageDetails = StorageManager.Instance.GetStorageDetailsById(_negotiation.sourceStorageId);

        if (_isSupply)
        {
            _supplierOrDemanderTeam = GameDataManager.Instance.GetTeamById(_negotiation.demanderId);

            if (_storageDetails.Item2)
            {
                storageOrDistanceLocalize.SetKey("negotiation_item_dc", _storageDetails.Item1.ToString());
            }
            else
            {
                storageOrDistanceLocalize.SetKey("negotiation_item_warehouse");
            }
        }
        else
        {
            _supplierOrDemanderTeam = GameDataManager.Instance.GetTeamById(_negotiation.supplierId);

            storageOrDistanceLocalize.SetKey("distance", CalculateDistance().ToString());
        }

        supplierOrDemander.text = _supplierOrDemanderTeam.teamName;
        
        productNameLocalize.SetKey("product_" + _product.name);
        amount.text = _negotiation.amount.ToString();
        demanderCostPerUnit.text = _negotiation.costPerUnitDemander.ToString("0.00");
        supplierCostPerUnit.text = _negotiation.costPerUnitSupplier.ToString("0.00");
        negotiationStatusLocalize.SetKey(_negotiation.state.ToString());
    }

    public void UpdateEditedNegotiation(Utils.Negotiation editedNegotiation)
    {
        _negotiation = editedNegotiation;
        
        demanderCostPerUnit.text = _negotiation.costPerUnitDemander.ToString("0.00");
        supplierCostPerUnit.text = _negotiation.costPerUnitSupplier.ToString("0.00");
        negotiationStatusLocalize.SetKey(_negotiation.state.ToString());

        if (_negotiation.state == Utils.NegotiationState.DEAL)
        {
            showActionToggle.isOn = false;
            showActionToggle.gameObject.SetActive(false);
        }

        _isSendingRequest = false;
    }

    private int CalculateDistance()
    {
        var source =
            GameDataManager.Instance.GetLocationByTypeAndId(
                _storageDetails.Item2 ? Utils.TransportNodeType.DC : Utils.TransportNodeType.FACTORY,
                _storageDetails.Item1);
        var destination = GameDataManager.Instance.GetMyTeamLocaionOnMap();
        return TransportManager.Instance.GetTransportDistance(source, destination, Utils.VehicleType.TRUCK);
    }

    public void ShowActionToggleOnValueChanged(bool value)
    {
        onOpenObjects.SetActive(value);
        if (value)
        {
            mainImage.sprite = openedSprite;
        }
        else
        {
            mainImage.sprite = closedSprite;
        }

        if (_isSupply)
        {
            NegotiationsController.Instance.RebuildSupplyNegotiationsLayout();
        }
        else
        {
            NegotiationsController.Instance.RebuildDemandNegotiationsLayout();
        }
    }

    public void OnRejectButtonClick()
    {
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                _isSendingRequest = true;
                var rejectRequest = new RejectNegotiationRequest(_negotiation.id);
                RequestManager.Instance.SendRequest(rejectRequest);
            }
        });
    }

    public void OnChatButtonClick()
    {
        int otherTeamId = _negotiation.supplierId == PlayerPrefs.GetInt("TeamId") ? _negotiation.demanderId : _negotiation.supplierId;
        ChatsListController.Instance.OpenChatFromNegotiation(otherTeamId);
    }

    public void OnChangePriceButtonClick()
    {
        if (_isSendingRequest || _negotiation.state != Utils.NegotiationState.IN_PROGRESS)
        {
            return;
        }

        string price = pricePerUnitInputField.text;
        if (string.IsNullOrEmpty(price))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        var parsedPrice = float.Parse(price);
        
        if (parsedPrice > _product.maxPrice || parsedPrice < _product.minPrice)
        {
            DialogManager.Instance.ShowErrorDialog("price_min_max_error");
            return;
        }

        if (_negotiation.supplierId == PlayerPrefs.GetInt("TeamId"))
        {
            var availableAmount = StorageManager.Instance.GetProductAmountByStorage(StorageManager.Instance.GetWarehouse(), _product.id);

            if (availableAmount < _negotiation.amount)
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_available_amount_error");
                return;
            }
        }
        
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (agreed)
            {
                _isSendingRequest = true;
                EditNegotiationCostPerUnitRequest editNegotiationCostPerUnitRequest = new EditNegotiationCostPerUnitRequest(RequestTypeConstant.EDIT_NEGOTIATION_COST_PER_UNIT, _negotiation.id, parsedPrice);
                RequestManager.Instance.SendRequest(editNegotiationCostPerUnitRequest);
                StartCoroutine(RequestTimeOut());
            }
        });
    }

    private IEnumerator RequestTimeOut()
    {
        yield return new WaitForSeconds(5f);
        _isSendingRequest = false;
    }
}
