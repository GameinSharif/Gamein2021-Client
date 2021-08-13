using System;

[Serializable]
public class ResponseObject<T>
{
    public ResponseTypeConstants ResponseType;
    public T ResponseData;
}