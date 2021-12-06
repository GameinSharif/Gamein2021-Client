using RTLTMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    
    public RTLTextMeshPro en;
    public RTLTextMeshPro fa;
    public GameObject image;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetGraphics(LocalizationManager.GetCurrentLanguage() == LocalizationManager.LocalizedLanguage.Farsi);
    }

    public void OnChangeLanguageButtonClicked()
    {
        DialogManager.Instance.ShowConfirmDialog(agreed =>
        {
            if (!agreed) return;

            var newLanguage =
                LocalizationManager.GetCurrentLanguage() == LocalizationManager.LocalizedLanguage.English
                    ? LocalizationManager.LocalizedLanguage.Farsi
                    : LocalizationManager.LocalizedLanguage.English;
            
            LocalizationManager.Instance.SetLanguage(newLanguage.ToString());
            
            bool isFarsi = newLanguage == LocalizationManager.LocalizedLanguage.Farsi;

            SetGraphics(isFarsi);

            SceneManager.LoadScene("MenuScene");
        });
    }
    
    private readonly Vector3 _enScale = new Vector3(1, 1, 1);
    private readonly Vector3 _faScale = new Vector3(-1, 1, 1);

    private void SetGraphics(bool isFarsi)
    {
        image.transform.localScale = isFarsi ? _faScale : _enScale;
            
        fa.color = new Color(fa.color.r, fa.color.g, fa.color.b, isFarsi ? 1.0f : 0.55f);
        en.color = new Color(en.color.r, en.color.g, en.color.b, isFarsi ? 0.55f : 1.0f);
    }
}