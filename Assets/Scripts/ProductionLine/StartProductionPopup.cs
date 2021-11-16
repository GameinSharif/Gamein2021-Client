using System.Collections.Generic;
using System.Linq;
using Assets.Mapbox.Unity.MeshGeneration.Modifiers.MeshModifiers;
using RTLTMPro;
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

        private ToggleGroup _toggleGroup;
        private int selectedProduct = -1;

        private List<GameObject> choices = new List<GameObject>();

        private Utils.ProductionLineDto data;

        private void Awake()
        {
            for (int i = 0; i < 6; i++)
            {
                var c = Instantiate(choicePrefab, choicesParent);
                choices.Add(c);
                _toggleGroup.RegisterToggle(c.GetComponent<Toggle>());
            }
        }

        public void Setup(Utils.ProductionLineDto data)
        {
            this.data = data;
            _toggleGroup.SetAllTogglesOff();
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
                choices[i].GetComponent<Toggle>().onValueChanged.AddListener(on => Select(product.id));
            }

            selectedProduct = -1;
        }

        private void Select(int productId)
        {
            selectedProduct = productId;
        }

        public void StartButton()
        {
            int amount = int.Parse(productAmount_I.text);
            //TODO: check money and materials
            
            DialogManager.Instance.ShowConfirmDialog(agreed =>
            {
                if (agreed)
                {
                    var request =
                        new StartProductionRequest(RequestTypeConstant.START_PRODUCTION, data.id, selectedProduct, amount);
                    RequestManager.Instance.SendRequest(request);
                    CloseButton();
                }
            });
        }

        public void CloseButton()
        {
            gameObject.SetActive(false);
        }
    }
}