using RTLTMPro;
using UnityEngine;

[RequireComponent(typeof(RTLTextMeshPro))]
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

    private void SetLocalize()
    {
        if(!text.text.StartsWith("product_")) return;
        text.onChange -= SetLocalize;
        localize.SetKey(text.text);
    }
}
