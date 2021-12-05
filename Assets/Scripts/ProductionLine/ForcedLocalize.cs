using RTLTMPro;
using UnityEngine;

[RequireComponent(typeof(RTLTextMeshPro), typeof(Localize))]
public class ForcedLocalize : MonoBehaviour
{
    private RTLTextMeshPro text;
    private Localize localize;
    private void Awake()
    {
        text = GetComponent<RTLTextMeshPro>();
        localize = GetComponent<Localize>();

        text.onChange += SetLocalize;
    }

    private void SetLocalize(string newValue)
    {
        if(!newValue.StartsWith("product_")) return;
        localize.SetKey(newValue);
    }
}
