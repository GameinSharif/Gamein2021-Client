using UnityEngine;

public class RequestHandler
{
    public static void HandleServerResponse(string responseJson)
    {
        ResponseObject responseObject = JsonUtility.FromJson<ResponseObject>(responseJson);
        switch (responseObject.responseTypeConstant)
        {
            case ResponseTypeConstant.LOGIN:
                LoginResponse loginResponseObject = (LoginResponse) responseObject.responseData;
                EventManager.Instance.OnLoginResponse(loginResponseObject);
                break;
        }
    }
}