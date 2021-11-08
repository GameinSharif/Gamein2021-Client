using System;
using RTLTMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour
{
    public GameObject DatePickerPanelPrefab;
    public RTLTextMeshPro currentSelectedDate;

    public DateTime Value
    {
        get => value;
        set
        {
            this.value = value;
            currentSelectedDate.text = value.ToString("yyyy-MM-dd");
        }
    }

    private DateTime value;
    private DatePickerPanel panel;
    private bool panelOpened = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (panelOpened) return;

        panel = Instantiate(DatePickerPanelPrefab, transform)
            .GetComponent<DatePickerPanel>();
        panelOpened = true;

        panel.onOk += selectedDate =>
        {
            Value = selectedDate;
            ClosePanel();
        };
        panel.onCancel += ClosePanel;
    }

    private void ClosePanel()
    {
        Destroy(panel.gameObject);
        panelOpened = false;
    }

}