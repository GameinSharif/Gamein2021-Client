using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionLineTemplateChoice : MonoBehaviour
{
    public Localize name_T;
    public Image image;
    public RTLTextMeshPro cost_T;
    public void Setup(string name, Sprite image, int cost)
    {
        name_T.SetKey("production_line_template_" + name);
        this.image.sprite = image;
        cost_T.text = cost.ToString();
    }
}
