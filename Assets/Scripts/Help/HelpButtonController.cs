using System;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonController : MonoBehaviour
{
    public GameObject helpFramePrefab;
    public string localizeKey;
    public Transform parent;

    private GameObject _frame;

    private void OnMouseEnter()
    {
        if (_frame == null)
        {
            _frame = Instantiate(helpFramePrefab, parent, true);
            
            var buttonTransform = this.transform;
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