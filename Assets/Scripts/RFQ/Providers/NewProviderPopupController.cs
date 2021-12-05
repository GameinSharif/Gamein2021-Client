using System;
using System.Collections.Generic;
using ProductionLine;
using UnityEngine;
using TMPro;

public class NewProviderPopupController : MonoBehaviour
{
    public static NewProviderPopupController Instance;

    public GameObject NewProviderPopupCanvas;

    public List<ProductDetailsSetter> ProductDetailsSetters;

    public TMP_InputField CapacityInputfield;
    public TMP_InputField PriceInputfield;
    public TMP_InputField AveragePriceInputfield;
    public TMP_InputField MinPriceOnRecordInputfield;
    public TMP_InputField MaxPriceOnRecordInputfield;
    public Localize minMaxLocalize;
    public TMP_Dropdown storageDropdown;

    private List<Utils.Storage> _storageOptions = new List<Utils.Storage>();
    private Utils.Product _selectedProduct = null;
    private bool _isSendingRequest = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnNewProviderResponseEvent += OnNewProviderResponse;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnNewProviderResponseEvent -= OnNewProviderResponse;
    }

    private void OnNewProviderResponse(NewProviderResponse newProviderResponse)
    {
        _isSendingRequest = false;
        if (newProviderResponse.newProvider != null)
        {
            ProvidersController.Instance.AddMyProviderToList(newProviderResponse.newProvider);
            NotificationsController.Instance.AddNewNotification("notification_new_provider",
            GameDataManager.Instance.GetProductName(newProviderResponse.newProvider.productId));
            NewProviderPopupCanvas.SetActive(false);
        }
        else
        {
            NewProviderPopupCanvas.SetActive(false);
            DialogManager.Instance.ShowErrorDialog();
        }
    }

    public void OnOpenNewProviderPopupClick()
    {
        _isSendingRequest = false;
        
        ClearInputFields();

        SetProducts();
        
        InitializeDropdownOptions();

        NewProviderPopupCanvas.SetActive(true);
    }

    private void InitializeDropdownOptions()
    {
        _storageOptions.Clear();
        storageDropdown.ClearOptions();
        
        foreach (var storage in StorageManager.Instance.Storages)
        {
            var optionData = new TMP_Dropdown.OptionData(storage.dc ? ("DC " + storage.buildingId): "Warehouse");
            storageDropdown.options.Add(optionData);
            _storageOptions.Add(storage);
        }

        storageDropdown.value = -1;
    }

    private void ClearInputFields()
    {
        CapacityInputfield.text = "";
        PriceInputfield.text = "";
        AveragePriceInputfield.text = "";
        MinPriceOnRecordInputfield.text = "";
        MaxPriceOnRecordInputfield.text = "";
    }

    private void SetProducts()
    {
        int index = 0;
        foreach (Utils.Product product in GameDataManager.Instance.Products)
        {
            if (product.productType == Utils.ProductType.SemiFinished)
            {
                bool hasThisProductsProductionLine = ProductionLinesDataManager.Instance.HasProductionLineOfProduct(product);
                bool isNotCurrentlyProviderOfThisProduct = !ProvidersController.Instance.IsProviderOfProduct(product.id);

                ProductDetailsSetters[index].SetData(product, hasThisProductsProductionLine && isNotCurrentlyProviderOfThisProduct, "NewProvider");
                
                index++;
            }
        }
    }

    public void OnProductClick(Utils.Product product)
    {
        _selectedProduct = product;

        var (mean, max, min) = ProvidersController.Instance.CalculateMeanMaxMinByProductId(product.id);

        AveragePriceInputfield.text = mean.ToString("0.00");
        MaxPriceOnRecordInputfield.text = max.ToString("0.00");
        MinPriceOnRecordInputfield.text = min.ToString("0.00");

        minMaxLocalize.SetKey("min_max_text", product.minPrice.ToString(), product.maxPrice.ToString());
    }

    public void OnDoneButtonClick()
    {
        if (_isSendingRequest)
        {
            return;
        }

        string capacity = CapacityInputfield.text;
        string price = PriceInputfield.text;
        if (string.IsNullOrEmpty(capacity) || string.IsNullOrEmpty(price))
        {
            DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
            return;
        }

        if (_selectedProduct == null)
        {
            DialogManager.Instance.ShowErrorDialog("no_product_selected_error");
            return;
        }

        if (storageDropdown.value < 0)
        {
            DialogManager.Instance.ShowErrorDialog("no_storage_selected_error");
            return;
        }
        
        var parsedCapacity = int.Parse(capacity);
        if (parsedCapacity < 1)
        {
            DialogManager.Instance.ShowErrorDialog("invalid_capacity_error");
            return; 
        }
        
        var storage = _storageOptions[storageDropdown.value];

        if (storage.dc)
        {
            var dc = GameDataManager.Instance.GetDcById(storage.buildingId);
            bool typeMatches = dc.type == Utils.DCType.SemiFinished &&
                               _selectedProduct.productType == Utils.ProductType.SemiFinished;
            
            if (!typeMatches)
            {
                DialogManager.Instance.ShowErrorDialog("dc_and_product_type_mismatch_error");
                return; 
            }
        }
        
        var parsedPrice = float.Parse(price);
        var product = _selectedProduct;

        if (parsedPrice > product.maxPrice || parsedPrice < product.minPrice)
        {
            DialogManager.Instance.ShowErrorDialog("price_min_max_error");
            return;
        }
        
        _isSendingRequest = true;
        NewProviderRequest newProviderRequest = new NewProviderRequest(RequestTypeConstant.NEW_PROVIDER, _selectedProduct.id, parsedCapacity, parsedPrice, storage.id);
        RequestManager.Instance.SendRequest(newProviderRequest);
    }
}
