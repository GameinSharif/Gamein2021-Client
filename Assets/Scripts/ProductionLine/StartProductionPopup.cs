using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class StartProductionPopup : MonoBehaviour
    {
        public GameObject choicePrefab;
        public Transform choicesParent;

        public TMP_InputField productAmount_I;

        public ToggleGroup _toggleGroup;
        public Button start_B;

        private int _selectedProduct;
        private int SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                start_B.interactable = value != -1;
                _selectedProduct = value;
            }
        }

        private List<GameObject> choices = new List<GameObject>();

        private Utils.ProductionLineDto data;
        private Utils.ProductionLineTemplate _template;

        private void Awake()
        {
            for (int i = 0; i < 6; i++)
            {
                var c = Instantiate(choicePrefab, choicesParent);
                choices.Add(c);
                c.GetComponent<Toggle>().group = _toggleGroup;
            }
        }

        public void Setup(Utils.ProductionLineDto data)
        {
            this.data = data;
            _template = GameDataManager.Instance.GetProductionLineTemplateById(data.productionLineTemplateId);
            foreach (var item in choices)
            {
                item.SetActive(false);
            }

            var products = GameDataManager.Instance.Products
                .Where(c => c.productionLineTemplateId == data.productionLineTemplateId).ToList();
            for (var i = 0; i < products.Count; i++)
            {
                var product = products[i];
                choices[i].SetActive(true);
                choices[i].GetComponentInChildren<Localize>().SetKey("product_" + product.name);
                choices[i].GetComponent<Toggle>().onValueChanged.AddListener(on => Select(on, product.id));
            }
            _toggleGroup.SetAllTogglesOff();

            SelectedProduct = -1;
        }
        
        private void Select(bool toggleOn, int productId)
        {
            SelectedProduct = toggleOn ? productId : -1;
        }

        public void StartButton()
        {
            int amount = int.Parse(productAmount_I.text);
            if (!HaveEnoughMoneyForProduct(amount))
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            if (!HaveEnoughMaterialForProduct(SelectedProduct, amount))
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_material_error");
                return;
            }

            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new StartProductionRequest(RequestTypeConstant.START_PRODUCTION, data.id, SelectedProduct,
                            amount);
                    RequestManager.Instance.SendRequest(request);
                    CloseButton();
                }
            });
        }

        private bool HaveEnoughMaterialForProduct(int productId, int amount)
        {
            var ingredients = GameDataManager.Instance.GetProductById(productId).ingredientsPerUnit;
            if (ingredients is null) return true;
            
            foreach (var ingredient in ingredients)
            {
                if (ingredient.amount * amount * _template.batchSize >
                    StorageManager.Instance.GetProductAmountByStorage(StorageManager.Instance.GetWarehouse(),
                        ingredient.productId))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HaveEnoughMoneyForProduct(int amount)
        {
            return CalculateProductionCost(amount) <= MainHeaderManager.Instance.Money;
        }

        public void CloseButton()
        {
            gameObject.SetActive(false);
        }

        private int CalculateProductionCost(int amount)
        {
            return _template.productionCostPerOneProduct * _template.batchSize * amount + _template.setupCost;
        }
    }
}