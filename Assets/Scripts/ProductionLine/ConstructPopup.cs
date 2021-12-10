using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ConstructPopup : MonoBehaviour
    {
        public GameObject choicePrefab;

        //public Transform choicesParent;
        public Transform semiFinishedTemplates;
        public Transform finishedTemplates;
        
        public GameObject semiFinishedTemplatesGO;
        public GameObject finishedTemplatesGO;

        //public ToggleGroup _toggleGroup;
        //public Sprite defaultImageForChoice;
        //public Button construct_B;


        /*private int _currentSelected;

        private int CurrentSelected
        {
            get => _currentSelected;
            set
            {
                construct_B.interactable = value != -1;
                _currentSelected = value;
            }
        }*/

        private void Awake()
        {
            foreach (var template in GameDataManager.Instance.ProductionLineTemplates)
            {
                var products = GameDataManager.Instance.Products
                    .Where(c => c.productionLineTemplateId == template.id).ToList();
                var c = Instantiate(choicePrefab,
                    products[0].productType == Utils.ProductType.Finished ? finishedTemplates : semiFinishedTemplates);
                c.GetComponent<ProductionLineTemplateChoice>().Setup(template.name, template.constructionCost,
                    () => Construct(template.id));
                //c.GetComponent<Toggle>().group = _toggleGroup;
                //c.GetComponent<Toggle>().onValueChanged.AddListener(on => Select(on, template.id));
            }
            GoToTab(1);
        }

        public void GoToTab(int number)
        {
            switch (number)
            {
                case 1:
                    finishedTemplatesGO.SetActive(false);
                    semiFinishedTemplatesGO.SetActive(true);
                    break;
                case 2:
                    semiFinishedTemplatesGO.SetActive(false);
                    finishedTemplatesGO.SetActive(true);
                    break;
            }
        }
        
        private void Construct(int templateID)
        {
            ProductionLinesController.Instance.ConstructProductionLine(templateID);
            CloseButton();
        }

        private void OnEnable()
        {
            //_toggleGroup.SetAllTogglesOff();
            //CurrentSelected = -1;
        }

        private void Select(bool toggleOn, int templateId)
        {
            //CurrentSelected = toggleOn ? templateId : -1;
        }

        public void ConstructButton()
        {
            //ProductionLinesController.Instance.ConstructProductionLine(CurrentSelected);
            //CloseButton();
        }

        public void CloseButton()
        {
            gameObject.SetActive(false);
        }
    }
}