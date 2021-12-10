using System;
using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class StartProductionPanel : MonoBehaviour
    {
        //public GameObject choicePrefab;
        //public Transform choicesParent;

        public List<GameObject> ingredientItems;

        public TMP_InputField productAmount_I;

        public RTLTextMeshPro formula;
        public RTLTextMeshPro total;
        public Localize productionTime;
        public Image productIcon;

        //public ToggleGroup _toggleGroup;
        public Button start_B;

        private int _selectedProductId;

        private int SelectedProductId
        {
            get => _selectedProductId;
            set
            {
                start_B.interactable = int.Parse(productAmount_I.text) > 0 && value != -1;
                _selectedProductId = value;
            }
        }

        public TMP_Dropdown productDropdown;

        //private List<GameObject> choices = new List<GameObject>();

        private Utils.ProductionLineDto data;
        private Utils.ProductionLineTemplate _template;

        private List<Utils.Product> currentLineProducts;

        private void Awake()
        {
            for (int i = 0; i < 6; i++)
            {
                //var c = Instantiate(choicePrefab, choicesParent);
                //choices.Add(c);
                //c.GetComponent<Toggle>().group = _toggleGroup;
            }

            start_B.onClick.AddListener(StartButton);
            productDropdown.onValueChanged.AddListener(ChooseProduct);
            productAmount_I.onValueChanged.AddListener(AmountChange);
        }

        public void Setup(Utils.ProductionLineDto data)
        {
            this.data = data;
            _template = GameDataManager.Instance.GetProductionLineTemplateById(data.productionLineTemplateId);

            var products = GameDataManager.Instance.Products
                .Where(c => c.productionLineTemplateId == data.productionLineTemplateId).ToList();

            productDropdown.ClearOptions();
            currentLineProducts = products;
            productDropdown.AddOptions(products.Select(c => "product_" + c.name).ToList());
            //for (var i = 0; i < products.Count; i++)
            //{
            //var product = products[i];
            //productDropdown.AddOptions(new List<string>(product.name));
            //choices[i].SetActive(true);
            //choices[i].GetComponentInChildren<Localize>().SetKey("product_" + product.name);
            //choices[i].GetComponent<Toggle>().onValueChanged.AddListener(on => Select(on, product.id));
            //}
            //_toggleGroup.SetAllTogglesOff();

            productAmount_I.text = "0";
            SelectedProductId = -1;
            ShowProductionTime();
            ShowTotalProduction();
            foreach (var item in ingredientItems)
            {
                item.SetActive(false);
            }
        }

        private void ChooseProduct(int optionIndex)
        {
            SelectedProductId = currentLineProducts[optionIndex].id;
            if (SelectedProductId == -1) return;
            ShowIngredients(SelectedProductId);
            ShowProductionTime();
            ShowTotalProduction();
        }

        private void ShowIngredients(int productId)
        {
            foreach (var item in ingredientItems)
            {
                item.SetActive(false);
            }

            if (productId == 27) return; //CarbonDioxide has no ingredients

            var ingredients = GameDataManager.Instance.GetProductById(productId).ingredientsPerUnit;
            if (ingredients is null) return;

            for (var i = 0; i < ingredients.Count; i++)
            {
                var item = ingredientItems[i];
                item.GetComponentInChildren<Image>().sprite =
                    GameDataManager.Instance.ProductSprites[ingredients[i].productId];
                item.GetComponentInChildren<RTLTextMeshPro>().text = ingredients[i].amount.ToString();
                item.SetActive(true);
            }
        }

        private void AmountChange(string value)
        {
            if (string.IsNullOrEmpty(value))
                productAmount_I.text = "0";

            start_B.interactable = int.Parse(productAmount_I.text) > 0 && SelectedProductId != -1;
            ShowTotalProduction();
            ShowProductionTime();
        }

        private void ShowTotalProduction()
        {
            if (SelectedProductId == -1)
            {
                productIcon.enabled = false;
                total.enabled = false;
            }
            else
            {
                productIcon.enabled = true;
                total.enabled = true;
                var final = Mathf.FloorToInt(float.Parse(productAmount_I.text) * _template.batchSize *
                    _template.efficiencyLevels[data.efficiencyLevel].efficiencyPercentage / 100);
                total.text = final.ToString();
                productIcon.sprite = GameDataManager.Instance.ProductSprites[SelectedProductId];
            }

            formula.text = productAmount_I.text + " \u00D7 " + _template.batchSize + " \u00D7 " +
                           _template.efficiencyLevels[data.efficiencyLevel].efficiencyPercentage + "/100";
        }

        private void ShowProductionTime()
        {
            var time = Mathf.CeilToInt(float.Parse(productAmount_I.text) / _template.dailyProductionRate);
            productionTime.SetKey("time_DAY", time.ToString());
        }

        public void StartButton()
        {
            if (SelectedProductId == -1)
            {
                DialogManager.Instance.ShowErrorDialog("no_product_selected_error");
                return;
            }

            int amount = int.Parse(productAmount_I.text);
            if (amount <= 0)
            {
                DialogManager.Instance.ShowErrorDialog("empty_input_field_error");
                return;
            }

            if (!HaveEnoughMoneyForProduct(amount))
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_money_error");
                return;
            }

            if (!HaveEnoughMaterialForProduct(SelectedProductId, amount))
            {
                DialogManager.Instance.ShowErrorDialog("not_enough_material_error");
                return;
            }

            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new StartProductionRequest(RequestTypeConstant.START_PRODUCTION, data.id, SelectedProductId,
                            amount);
                    RequestManager.Instance.SendRequest(request);
                    CloseButton();
                }
            });
        }

        private bool HaveEnoughMaterialForProduct(int productId, int amount)
        {
            if (productId == 27) //CarbonDioxide has no ingredients
            {
                return true;
            }

            var ingredients = GameDataManager.Instance.GetProductById(productId).ingredientsPerUnit;
            if (ingredients is null) return true;

            foreach (var ingredient in ingredients)
            {
                if (ingredient.productId == 4) //always has Water
                {
                    continue;
                }

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