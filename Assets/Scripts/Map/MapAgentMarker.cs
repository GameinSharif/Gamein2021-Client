using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapAgentMarker
{
    public enum AgentType
    {
        Manufacturer,
        Storage,
        Shop,
    }

    public AgentType MapAgentType;
    public Material MarkerMaterial;
}
