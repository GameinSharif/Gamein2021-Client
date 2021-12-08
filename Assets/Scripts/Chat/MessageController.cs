using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public GameObject ReportButton;
    
    public RTLTextMeshPro text;

    public void SetText(string value)
    {
        text.text = value;
    }

    public void OnShowReportButtonClicked()
    {
        StartCoroutine(ShowReportButton());
    }
    
    private IEnumerator ShowReportButton()
    {
        ReportButton.SetActive(true);
        yield return new WaitForSeconds(5f);
        ReportButton.SetActive(false);
    }

    public void OnReportButtonClicked()
    {
        Debug.Log("reported");
        //TODO send request
    }
    
}
