using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private MeshRenderer MeshRenderer;
    [SerializeField] private TextMesh textMesh;

    public void Initialize(MapUtils.MapAgentMarker mapAgentMarker, string name = null)
    {
        MeshRenderer.material = mapAgentMarker.MarkerMaterial;
        if (name != null)
        {
            textMesh.text = name;
        }
        textMesh.color = mapAgentMarker.TextColor;
    }
}
