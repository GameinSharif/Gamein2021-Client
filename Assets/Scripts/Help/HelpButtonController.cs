using System;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonController : MonoBehaviour
{
    public GameObject helpFramePrefab;
    public string localizeKey;
    public float x = 1;
    public float y = 1;

    private GameObject _frame;

    private void OnMouseEnter()
    {
        if (_frame == null)
        {
            var buttonTransform = this.transform;
            
            _frame = Instantiate(helpFramePrefab, buttonTransform.parent, true);

            var rectTransform = _frame.GetComponent<RectTransform>();
            rectTransform.SetPositionAndRotation(buttonTransform.position, buttonTransform.rotation);
            rectTransform.localScale = Vector3.one;
            rectTransform.pivot = new Vector2(x, y);
            
            _frame.GetComponent<HelpFrameController>().SetText(localizeKey);
        }
        _frame.SetActive(true);
    }

    private void OnMouseExit()
    {
        _frame.SetActive(false);
    }
}