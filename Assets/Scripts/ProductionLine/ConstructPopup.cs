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
        private int currentSelected = -1;

        private ToggleGroup _toggleGroup;
        
        private void Awake()
        {
            print(GameDataManager.Instance.ProductionLineTemplates.Count);
            foreach (var template in GameDataManager.Instance.ProductionLineTemplates)
            {
                var c = Instantiate(choicePrefab, choicesParent);
                c.GetComponent<Button>().onClick.AddListener(() => { currentSelected = template.id; });
                c.GetComponentInChildren<RTLTextMeshPro>().text = template.name;
            }
        }

        private void OnEnable()
        {
            currentSelected = -1;
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