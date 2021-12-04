using System;

[Serializable]
public class EditProviderRequest : RequestObject
{
    public int providerId;
    public int newCapacity;
    public float newPrice;

    public EditProviderRequest(int providerId, int newCapacity, float newPrice) : base(RequestTypeConstant.EDIT_PROVIDER)
    {
        this.providerId = providerId;
        this.newCapacity = newCapacity;
        this.newPrice = newPrice;
    }
}