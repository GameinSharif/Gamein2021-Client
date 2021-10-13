using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private MeshRenderer MeshRenderer;
    [SerializeField] private TextMesh textMesh;

    public void Initialize(MapUtils.MapAgentMarker mapAgentMarker, string name)
    {
        textMesh.text = name;
        textMesh.color = mapAgentMarker.TextColor;
        MeshRenderer.material = mapAgentMarker.MarkerMaterial;
    }

    public void SetMaterial(Material material)
    {
        MeshRenderer.material = material;
    }
}
