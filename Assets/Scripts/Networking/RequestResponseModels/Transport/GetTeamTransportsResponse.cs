using System.Collections.Generic;
using System;

[Serializable]
public class GetTeamTransportsResponse: ResponseObject
{
    public List<Utils.Transport> myTeamTransports;
}
