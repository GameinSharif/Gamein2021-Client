using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ConstructPopup : MonoBehaviour
    {
        public GameObject choicePrefab;
        public Transform choicesParent;
        public ToggleGroup _toggleGroup;
        public Sprite defaultImageForChoice;
        public Button construct_B;
        
        
        private int _currentSelected;
        private int CurrentSelected
        {
            get => _currentSelected;
            set
            {
                construct_B.interactable = value != -1;
                _currentSelected = value;
            }
        }

        private void Awake()
        {
            foreach (var template in GameDataManager.Instance.ProductionLineTemplates)
            {
                var c = Instantiate(choicePrefab, choicesParent);
                c.GetComponent<ProductionLineTemplateChoice>().Setup(template.name, defaultImageForChoice, template.constructionCost);
                c.GetComponent<Toggle>().group = _toggleGroup;
                c.GetComponent<Toggle>().onValueChanged.AddListener(on => Select(on, template.id));
            }
        }

        private void OnEnable()
        {
            _toggleGroup.SetAllTogglesOff();
            CurrentSelected = -1;
        }

        private void Select(bool toggleOn, int templateId)
        {
            CurrentSelected = toggleOn ? templateId : -1;
        }

        public void ConstructButton()
        {
            ProductionLinesController.Instance.ConstructProductionLine(CurrentSelected);
            CloseButton();
        }

        public void CloseButton()
        {
            gameObject.SetActive(false);
        }
    }
}