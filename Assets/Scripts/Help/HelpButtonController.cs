using System;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonController : MonoBehaviour
{
    public GameObject helpFramePrefab;
    public string localizeKey;

    private GameObject _frame;

    private void OnMouseEnter()
    {
        if (_frame == null)
        {
            var buttonTransform = this.transform;
            
            _frame = Instantiate(helpFramePrefab, buttonTransform.parent, true);

            _frame.transform.SetPositionAndRotation(buttonTransform.position, buttonTransform.rotation);
            _frame.transform.localScale = Vector3.one;
            
            _frame.GetComponent<HelpFrameController>().SetText(localizeKey);
        }
        _frame.SetActive(true);
    }

    private void OnMouseExit()
    {
        _frame.SetActive(false);
    }
}