using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using TMPro;

public class NegotiationItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public GameObject demanderGameObject;
    public RTLTextMeshPro demanderTeamName;
    public GameObject supplierGameObject;
    public RTLTextMeshPro supplierTeamName;
    public Localize productNameLocalize;
    public RTLTextMeshPro amount;
    public RTLTextMeshPro demanderCostPerUnit;
    public RTLTextMeshPro supplierCostPerUnit;
    public Localize negotiationStatusLocalize;
    public GameObject openActionsButtonGameObject;

    public TMP_InputField PricePerUnitInputfield;

    public GameObject actionsGameObjects;

    private Utils.Negotiation _negotiation;
    private bool _isSendingRequest = false;

    public void OnEditNegotiationCostPerUnitResponseReceived(Utils.Negotiation negotiation)
    {
        _isSendingRequest = false;
        if (negotiation.id == _negotiation.id)
        {
            SetInfo(int.Parse(no.OriginalText), negotiation);

            if (_negotiation.state == Utils.NegotiationState.DEAL)
            {
                actionsGameObjects.SetActive(false);
                openActionsButtonGameObject.SetActive(false);
            }
        }
    }

    private void SetInfo(int no, string demanderTeamName, string supplierTeamName, string productNameKey, int amount, float demanderCostPerUnit, float supplierCostPerUnit, Utils.NegotiationState negotiationState)
    {
        this.no.text = no.ToString();
        this.demanderTeamName.text = demanderTeamName;
        this.supplierTeamName.text = supplierTeamName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.amount.text = amount.ToString();
        this.demanderCostPerUnit.text = demanderCostPerUnit.ToString("0.00");
        this.supplierCostPerUnit.text = supplierCostPerUnit.ToString("0.00");
        negotiationStatusLocalize.SetKey(negotiationState.ToString());

        if (negotiationState != Utils.NegotiationState.IN_PROGRESS)
        {
            actionsGameObjects.SetActive(false);
            openActionsButtonGameObject.SetActive(false);
        }
    }

    public void SetSupplyNegotiationInfo(int no, Utils.Negotiation negotiation)
    {
        supplierGameObject.SetActive(false);

        SetInfo(no, negotiation);
    }

    public void SetDemandNegotiationInfo(int no, Utils.Negotiation negotiation)
    {
        demanderGameObject.SetActive(false);

        SetInfo(no, negotiation);
    }

    private void SetInfo(int no, Utils.Negotiation negotiation)
    {
        SetInfo(
            no: no,
            demanderTeamName: GameDataManager.Instance.GetTeamName(negotiation.demanderId),
            supplierTeamName: GameDataManager.Instance.GetTeamName(negotiation.supplierId),
            productNameKey: GameDataManager.Instance.GetProductById(negotiation.productId).name,
            amount: negotiation.amount,
            demanderCostPerUnit: negotiation.costPerUnitDemander,
            supplierCostPerUnit: negotiation.costPerUnitSupplier,
            negotiationState: negotiation.state
        );

        _negotiation = negotiation;
    }

    public void ToggleActions()
    {
        if (openActionsButtonGameObject.activeSelf)
        {
            actionsGameObjects.SetActive(!actionsGameObjects.activeSelf);
        }
    }

    public void OnRejectButtonClick()
    {
        // TODO
    }

    public void OnChatButtonClick()
    {
        // TODO
    }

    public void OnChangePriceButtonClick()
    {
        if (_isSendingRequest || _negotiation.state != Utils.NegotiationState.IN_PROGRESS)
        {
            return;
        }

        string price = PricePerUnitInputfield.text;
        if (string.IsNullOrEmpty(price))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        _isSendingRequest = true;
        EditNegotiationCostPerUnitRequest editNegotiationCostPerUnitRequest = new EditNegotiationCostPerUnitRequest(RequestTypeConstant.EDIT_NEGOTIATION_COST_PER_UNIT, _negotiation.id, float.Parse(price));
        RequestManager.Instance.SendRequest(editNegotiationCostPerUnitRequest);
    }
}
