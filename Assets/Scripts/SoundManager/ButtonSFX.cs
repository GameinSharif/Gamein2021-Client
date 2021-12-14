using UnityEngine;
using UnityEngine.UI;

namespace SoundManager
{
    [RequireComponent(typeof(Button))]
    public class ButtonSFX : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SFXManager.Instance.Play(SFXManager.SfxID.CLICK);
            });
        }
    }
}