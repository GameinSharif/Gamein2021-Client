using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaterialSetter : MonoBehaviour
{
    [SerializeField] private LineRenderer LineRenderer;

    public void SetMaterial(Material material)
    {
        LineRenderer.material = material;
    }
}
