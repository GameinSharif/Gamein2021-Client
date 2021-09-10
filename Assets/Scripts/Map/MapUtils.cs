using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;

public class MapUtils
{
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

    public class OnMapMarker
    {
        public Vector2d Location;
        public GameObject SpawnedObject;

        public OnMapMarker(Vector2d location, GameObject spawnedObject)
        {
            Location = location;
            SpawnedObject = spawnedObject;
        }
    }

    [System.Serializable]
    public class MapLine
    {
        public enum LineType
        {
            StorageToShop
        }

        public LineType MapLineType;
        public Material LineMaterial;
    }
}
