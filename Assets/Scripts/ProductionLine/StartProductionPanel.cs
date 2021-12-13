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
        //public ToggleGroup _toggleGroup;

        public TMP_Dropdown productDropdown;
        public TMP_InputField productAmount_I;
        public List<GameObject> ingredientItems;
        public Localize costBox;
        public Localize actualRate;
        public Localize productionTime;
        public RTLTextMeshPro finalAmount;
        public Image productIcon;
        public Button start_B;

        public Color ingredientShortageColor;

        private int _selectedProductId;

        private int SelectedProductId
        {
            get => _selectedProductId;
            set
            {
                start_B.interactable = Amount > 0 && value != -1;
                _selectedProductId = value;
            }
        }

        private int Amount
        {
            get
            {
                try
                {
                    var x = int.Parse(productAmount_I.text);
                    return x;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

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

            productAmount_I.text = "";
            SelectedProductId = -1;
            actualRate.SetKey("production_line_actual_rate",
                _template.efficiencyLevels[data.efficiencyLevel].efficiencyPercentage + "%");

            UpdateShowData();
        }

        private void ChooseProduct(int optionIndex)
        {
            SelectedProductId = currentLineProducts[optionIndex].id;

            UpdateShowData();
        }

        private void AmountChange(string value)
        {
            if (string.IsNullOrEmpty(value))
                productAmount_I.text = "0";
            if (Amount < 0)
                productAmount_I.text = "0";

            UpdateShowData();
        }

        private void ShowIngredients(int productId)
        {
            foreach (var item in ingredientItems)
            {
                item.SetActive(false);
            }

            if (productId == 27 || productId == -1 || Amount <= 0) return; //CarbonDioxide has no ingredients


            var ingredients = GameDataManager.Instance.GetProductById(productId).ingredientsPerUnit;
            if (ingredients is null) return;

            for (var i = 0; i < ingredients.Count; i++)
            {
                var item = ingredientItems[i];
                item.GetComponentInChildren<Image>().sprite =
                    GameDataManager.Instance.ProductSprites[ingredients[i].productId - 1];
                var ingredientAmount = item.GetComponentInChildren<RTLTextMeshPro>();
                ingredientAmount.text = (ingredients[i].amount * Amount).ToString();

                if (ingredients[i].productId == 4)
                {
                    ingredientAmount.color = Color.white;
                }
                else
                {
                    var stock = StorageManager.Instance.GetProductAmountByStorage(StorageManager.Instance.GetWarehouse(),
                        ingredients[i].productId);
                    ingredientAmount.color = ingredients[i].amount * Amount > stock ? ingredientShortageColor : Color.white;
                }

                item.SetActive(true);
            }
        }

        private void UpdateShowData()
        {
            ShowIngredients(SelectedProductId);

            costBox.SetKey("production_line_cost_box",
                Amount + "\u00D7" + _template.productionCostPerOneProduct * _template.batchSize,
                _template.setupCost.ToString(), CalculateProductionCost(Amount).ToString());

            var time = Mathf.CeilToInt((float) Amount / _template.dailyProductionRate);
            productionTime.SetKey("production_line_production_duration", time.ToString());

            if (SelectedProductId == -1)
            {
                productIcon.enabled = false;
                finalAmount.enabled = false;
            }
            else
            {
                productIcon.enabled = true;
                finalAmount.enabled = true;
                var final = Mathf.FloorToInt((float) Amount * _template.batchSize *
                    _template.efficiencyLevels[data.efficiencyLevel].efficiencyPercentage / 100);
                finalAmount.text = final.ToString();
                productIcon.sprite = GameDataManager.Instance.ProductSprites[SelectedProductId - 1];
            }

            /*formula.text = productAmount_I.text + " \u00D7 " + _template.batchSize + " \u00D7 " +
                           _template.efficiencyLevels[data.efficiencyLevel].efficiencyPercentage + "/100";*/

            start_B.interactable = Amount > 0 && SelectedProductId != -1;
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