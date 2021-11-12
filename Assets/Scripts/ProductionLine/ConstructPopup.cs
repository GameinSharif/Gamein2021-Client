using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionLine
{
    public class ConstructPopup : MonoBehaviour
    {
        public GameObject choicePrefab;
        public Transform choicesParent;
        private int currentSelected = -1;

        private void Awake()
        {
            foreach (var template in GameDataManager.Instance.ProductionLineTemplates)
            {
                var c = Instantiate(choicePrefab, choicesParent);
                c.GetComponent<Button>().onClick.AddListener(() => { currentSelected = template.id; });
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