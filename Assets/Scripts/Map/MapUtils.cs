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
            MyFactory,
            OtherFactory,
            DifferentCountryFactory,
            NoOwnerFactory,
            Supplier,
            GameinCustomer,
            MyDistributionCenter,
            OtherDistributionCenter,
            NoOwnerDistributionCenter
        }

        public AgentType MapAgentType;
        public Material MarkerMaterial;
        public Color TextColor;
    }

    public class OnMapMarker
    {
        public Vector2d Location;
        public GameObject SpawnedObject;
        public MapAgentMarker MapAgentMarker;
        public int Index; //This will be used as the index of this marker to map it to the proper team, shop, ...

        public OnMapMarker(Vector2d location, GameObject spawnedObject, MapAgentMarker mapAgentMarker, int index)
        {
            Location = location;
            SpawnedObject = spawnedObject;
            MapAgentMarker = mapAgentMarker;
            Index = index;
        }
    }

    [System.Serializable]
    public class MapLine
    {
        public enum LineType
        {
            FactoryToFactory,
            SupplyChain
        }

        public LineType MapLineType;
        public Material LineMaterial;
    }

    public class OnMapLine
    {
        public OnMapMarker Start;
        public OnMapMarker End;
        public LineRenderer LineRenderer;

        public OnMapLine(OnMapMarker start, OnMapMarker end, LineRenderer lineRenderer)
        {
            Start = start;
            End = end;
            LineRenderer = lineRenderer;
        }
    }
}
