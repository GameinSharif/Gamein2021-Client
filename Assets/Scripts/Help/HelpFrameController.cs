using UnityEngine;
using UnityEngine.UI;

public class HelpFrameController : MonoBehaviour
{
    public Localize localize;
    public RectTransform mainTransform;

    public void SetText(string localizeKey, params string[] replaceStrings)
    {
        localize.SetKey(localizeKey, replaceStrings);
        LayoutRebuilder.ForceRebuildLayoutImmediate(mainTransform);
    }
}