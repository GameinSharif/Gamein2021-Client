using System;
using RTLTMPro;
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
        
        private int currentSelected = -1;

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
            currentSelected = -1;
        }

        private void Select(bool toggleOn, int templateId)
        {
            print(templateId);
            currentSelected = toggleOn ? templateId : -1;
        }

        public void ConstructButton()
        {
            ProductionLinesController.Instance.ConstructProductionLine(currentSelected);
            CloseButton();
        }

        public void CloseButton()
        {
            gameObject.SetActive(false);
        }
    }
}