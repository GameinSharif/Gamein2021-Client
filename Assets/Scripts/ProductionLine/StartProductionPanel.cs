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

        public TMP_InputField productAmount_I;

        public RTLTextMeshPro total;
        
        
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
            /*foreach (var item in choices)
            {
                item.SetActive(false);
            }*/

            var products = GameDataManager.Instance.Products
                .Where(c => c.productionLineTemplateId == data.productionLineTemplateId).ToList();
            
            productDropdown.ClearOptions();
            currentLineProducts = products;
            productDropdown.AddOptions(products.Select(c => c.name).ToList());
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
            total.text = productAmount_I.text +  " \u00D7 "+_template.batchSize + " = " + int.Parse(productAmount_I.text) * _template.batchSize;
            SelectedProductId = -1;
        }

        private void ChooseProduct(int optionIndex)
        {
            SelectedProductId = currentLineProducts[optionIndex].id;
            //print(SelectedProductId);
        }
        /*private void Select(bool toggleOn, int productId)
        {
            SelectedProductId = toggleOn ? productId : -1;
        }*/
        
        private void AmountChange(string value)
        {
            if(string.IsNullOrEmpty(value))
                productAmount_I.text = "0";

            start_B.interactable = int.Parse(productAmount_I.text) > 0 && SelectedProductId != -1;
            total.text = productAmount_I.text +  " \u00D7 "+_template.batchSize + " = " + int.Parse(productAmount_I.text) * _template.batchSize;

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
            if (productId == 27) //CarbonDiaxide has no ingredients
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