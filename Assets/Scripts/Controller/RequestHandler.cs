using UnityEngine;

public class RequestHandler
{
    public static void HandleServerResponse(string responseJson)
    {
        var responseObject = JsonUtility.FromJson<ResponseObject<Object>>(responseJson);
        switch (responseObject.ResponseType)
        {
            case ResponseTypeConstants.LOGIN:
                var loginResponseObject = JsonUtility.FromJson<ResponseObject<LoginResponse>>(responseJson);
                EventManager.Instance.OnLoginResponse(loginResponseObject.ResponseData);
                break;
        }
    }
}