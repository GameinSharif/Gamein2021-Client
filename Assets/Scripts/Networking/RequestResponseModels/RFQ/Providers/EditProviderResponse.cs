using System;

[Serializable]
public class EditProviderResponse : ResponseObject
{
    public string result;
    public Utils.Provider editedProvider;
}