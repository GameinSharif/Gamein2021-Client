using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class ProviderItemController : MonoBehaviour
{
    public RTLTextMeshPro no;
    public RTLTextMeshPro teamName;
    public Localize productNameLocalize;
    public RTLTextMeshPro capacity;
    public RTLTextMeshPro price;
    public GameObject providerStateGameObject;
    public Localize providerStateLocalize;

    public GameObject RemoveProviderButtonGameObject;
    public GameObject NegotiateButtonGameObject;

    private Utils.Provider _provider;
    private bool _isSendingTerminateOrAccept = false;

    public void SetInfo(int no, string teamName, string productNameKey, int capacity, float price, Utils.ProviderState providerState)
    {
        providerStateLocalize.SetKey(providerState.ToString());

        SetInfo(no, teamName, productNameKey, capacity, price);
    }

    private void SetInfo(int no, string teamName, string productNameKey, int capacity, float price)
    {
        this.no.text = no.ToString();
        this.teamName.text = teamName;
        productNameLocalize.SetKey("product_" + productNameKey);
        this.capacity.text = capacity.ToString();
        this.price.text = price.ToString("0.00");
    }

    public void SetInfo(int no, Utils.Provider provider)
    {
        if (PlayerPrefs.GetInt("TeamId") == provider.teamId)
        {
            SetInfo(
                no: no,
                teamName: GameDataManager.Instance.GetTeamName(provider.teamId),
                productNameKey: GameDataManager.Instance.GetProductById(provider.productId).name,
                capacity: provider.capacity,
                price: provider.price,
                providerState: provider.state);

            if (provider.state == Utils.ProviderState.ACTIVE)
            {
                RemoveProviderButtonGameObject.SetActive(true);
            }
            else
            {
                RemoveProviderButtonGameObject.SetActive(false);
            }
            NegotiateButtonGameObject.SetActive(false);
        }
        else
        {
            SetInfo(
                no: no,
                teamName: GameDataManager.Instance.GetTeamName(provider.teamId),
                productNameKey: GameDataManager.Instance.GetProductById(provider.productId).name,
                capacity: provider.capacity,
                price: provider.price);

            RemoveProviderButtonGameObject.SetActive(false);
            NegotiateButtonGameObject.SetActive(true);
        }

        _provider = provider;
        _isSendingTerminateOrAccept = false;
}

    public void OnNegotiateButtonClick()
    {
        if (_isSendingTerminateOrAccept)
        {
            return;
        }

        _isSendingTerminateOrAccept = true;
        //TODO
    }

    public void OnRemoveProviderButtonClick()
    {
        if (_isSendingTerminateOrAccept)
        {
            return;
        }

        _isSendingTerminateOrAccept = true;
        //TODO
    }
}
