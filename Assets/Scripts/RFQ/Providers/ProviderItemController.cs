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
    public GameObject RemoveProviderButtonGameObject;
    public GameObject NegotiateButtonGameObject;

    private Utils.Provider _provider;

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
        SetInfo(
            no: no,
            teamName: GameDataManager.Instance.GetTeamName(provider.teamId),
            productNameKey: GameDataManager.Instance.GetProductById(provider.productId).name,
            capacity: provider.capacity,
            price: provider.price);

        if (PlayerPrefs.GetInt("TeamId") == provider.teamId)
        {
            RemoveProviderButtonGameObject.SetActive(true);
            NegotiateButtonGameObject.SetActive(false);
        }
        else
        {
            RemoveProviderButtonGameObject.SetActive(false);
            NegotiateButtonGameObject.SetActive(true);
        }

        _provider = provider;
    }

    public void OnNegotiateButtonClick()
    {
        //TODO
    }

    public void OnRemoveProviderButtonClick()
    {
        //TODO
    }
}
