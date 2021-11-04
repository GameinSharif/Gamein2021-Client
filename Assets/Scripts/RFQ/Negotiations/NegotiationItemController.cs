using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

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

    public GameObject actionsGameObjects;

    private Utils.Negotiation _negotiation;

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
        actionsGameObjects.SetActive(!actionsGameObjects.activeSelf);
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
        //TODO
    }
}
