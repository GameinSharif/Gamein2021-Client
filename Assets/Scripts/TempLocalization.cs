using UnityEngine;
using RTLTMPro;

public class TempLocalization : MonoBehaviour
{
   public static TempLocalization Instance;

   private void Awake()
   {
       Instance = this;
   }

   public Localize localize;
   public RTLTextMeshPro text;
}