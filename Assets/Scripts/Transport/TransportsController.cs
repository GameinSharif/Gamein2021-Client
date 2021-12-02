using System;
using System.Collections.Generic;
using UnityEngine;

public class TransportsController : MonoBehaviour
{
    public static TransportsController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Sprite> markerSprites;
    public List<Sprite> vehicleSprites;
    
    public Sprite GetSpriteByAgentType(MapUtils.MapAgentMarker.AgentType agentType)
    {
        //TODO
        return null;
    }

    public Sprite GetVehicleSprite(Utils.VehicleType transportVehicleType)
    {
        return vehicleSprites[(int) transportVehicleType];
    }
}