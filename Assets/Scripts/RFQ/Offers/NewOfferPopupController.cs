using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewOfferPopupController : MonoBehaviour
{
    public static NewOfferPopupController Instance;

    public GameObject NewOfferPopupCanvas;

    public List<ProductDetailsSetter> ProductDetailsSetters;
    public List<GameObject> IsSelectedGameObjects;

    public TMP_InputField CapacityInputfield;
    public TMP_InputField PriceInputfield;
    public TMP_InputField AveragePriceInputfield;
    public TMP_InputField MinPriceOnRecordInputfield;
    public TMP_InputField MaxPriceOnRecordInputfield;

    public DatePicker deadline;

    private int _selectedProductId = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewOfferResponseEvent += OnNewOfferResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewOfferResponseEvent -= OnNewOfferResponse;
    }

    private void OnNewOfferResponse(NewOfferResponse newOfferResponse)
    {
        //TODO
    }

    public void OnOpenNewOfferPopupClick()
    {
        //TODO clear inputfields

        SetProducts();

        NewOfferPopupCanvas.SetActive(true);
    }

    private void SetProducts()
    {
        int index = 0;
        foreach (Utils.Product product in GameDataManager.Instance.Products)
        {
            if (product.productType == Utils.ProductType.SemiFinished)
            {
                bool hasThisProductsProductionLine = true;
                //TODO

                ProductDetailsSetters[index].SetData(product, hasThisProductsProductionLine, index);
                index++;
            }
        }
    }

    public void OnProductClick(int productId, int index)
    {
        DisableAllSelections();
        IsSelectedGameObjects[index].SetActive(true);

        //TODO Set Prices
    }

    public void DisableAllSelections()
    {
        foreach (GameObject gameObject in IsSelectedGameObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDoneButtonClick()
    {
        string capacity = CapacityInputfield.text;
        string price = PriceInputfield.text;
        if (string.IsNullOrEmpty(capacity) || string.IsNullOrEmpty(price) || _selectedProductId == 0)
        {
            //TODO show error
            return;
        }

        NewProviderRequest newProviderRequest = new NewProviderRequest(RequestTypeConstant.LOGIN, _selectedProductId, int.Parse(capacity), float.Parse(price));
        RequestManager.Instance.SendRequest(newProviderRequest);
    }

    public void OnPlaceOfferClicked()
    {
        //int volume = Convert.ToInt32(volumeInput.text);
        //int costPerUnit = Convert.ToInt32(costPerUnitInput.text);

        //if (volume < 0 || costPerUnit < 0)
        //{
        //    //TODO show error
        //    Debug.Log("Negative input");
        //    return;
        //}


        //var offer = new NewOfferTransitModel(
        //    type: dropdown.options[dropdown.value].text,
        //    volume: volume,
        //    costPerUnit: costPerUnit,
        //    earliestExpectedArrival: new CustomDateTime(eea.Value),
        //    latestExpectedArrival: new CustomDateTime(lea.Value),
        //    offerDeadline: new CustomDateTime(deadline.Value)
        //);
        //var request = new NewOfferRequest(RequestTypeConstant.NEW_OFFER, offer);

        //RequestManager.Instance.SendRequest(request);
    }
}
