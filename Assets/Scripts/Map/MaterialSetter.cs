using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer MeshRenderer;

    public void SetMaterial(Material material)
    {
        MeshRenderer.material = material;
    }
}
