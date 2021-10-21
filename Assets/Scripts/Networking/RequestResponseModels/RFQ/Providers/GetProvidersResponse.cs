using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetProvidersResponse : ResponseObject
{
    public List<Utils.Provider> myTeamProviders;
    public List<Utils.Provider> otherTeamsProviders;
}
