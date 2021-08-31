using UnityEngine;

public class RequestHandler
{
    public static void HandleServerResponse(string responseJson)
    {
        ResponseObject responseObject = JsonUtility.FromJson<ResponseObject>(responseJson);
        ResponseTypeConstant responseTypeConstant = (ResponseTypeConstant)responseObject.responseTypeConstant;
        switch (responseTypeConstant)
        {
            case ResponseTypeConstant.LOGIN:
                LoginResponse loginResponseObject = JsonUtility.FromJson<LoginResponse>(responseJson);
                EventManager.Instance.OnLoginResponse(loginResponseObject);
                break;
        }
    }
}