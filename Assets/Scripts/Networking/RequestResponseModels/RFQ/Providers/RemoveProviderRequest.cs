using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveProviderRequest : RequestObject
{
    public int providerId;

    public RemoveProviderRequest(RequestTypeConstant requestTypeConstant, int providerId) : base(requestTypeConstant)
    {
        this.providerId = providerId;
    }
}
