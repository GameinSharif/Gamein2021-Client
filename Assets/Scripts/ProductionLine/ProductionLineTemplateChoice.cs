using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionLineTemplateChoice : MonoBehaviour
{
    public Localize name_T;
    //public Image image;
    public RTLTextMeshPro cost_T;
    public Button BuyButton;
    public void Setup(string name, int cost, Action construct)
    {
        name_T.SetKey("production_line_template_" + name);
        cost_T.text = cost.ToString();
        BuyButton.onClick.AddListener(construct.Invoke);
    }
}
