using System;
using UnityEngine;
using Random = System.Random;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogCanvas;
    public Localize localize;
    public GameObject otherButtons;
    public GameObject closeButton;
    
    private Action<bool> _action;

    private void Awake()
    {
        Instance = this;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.H))
    //     {
    //         Instance.ShowConfirmDialog(value => Debug.Log(value + "" + new Random().NextDouble()), "gamein_suppliers_deals_title");
    //     }
    // }

    public void ShowConfirmDialog(Action<bool> action, string messageKey = null, params string[] replaceStrings)
    {
        _action = action;
        
        messageKey ??= "dialog_popup_default_confirm_text";
        localize.SetKey(messageKey, replaceStrings);
        
        otherButtons.SetActive(true);
        closeButton.SetActive(false);
        
        dialogCanvas.SetActive(true);
    }

    public void ShowErrorDialog(string messageKey = null, params string[] replaceStrings)
    {
        messageKey ??= "dialog_popup_default_error_text";
        localize.SetKey(messageKey, replaceStrings);
        
        otherButtons.SetActive(false);
        closeButton.SetActive(true);

        dialogCanvas.SetActive(true);
    }

    public void OnAgreeButtonClicked()
    {
        OnCloseButtonClicked();
        _action.Invoke(true);
    }

    public void OnDisagreeButtonClicked()
    {
        _action.Invoke(false);
        OnCloseButtonClicked();
    }

    public void OnCloseButtonClicked()
    {
        dialogCanvas.SetActive(false);
    }
}