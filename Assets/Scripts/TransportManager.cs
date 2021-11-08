using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportManager : MonoBehaviour
{
    public static TransportManager Instance;

    [HideInInspector] public List<Utils.Transport> Transports;
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnGetTeamTransportsResponseEvent += OnGetTransportsResponseReceived;
        EventManager.Instance.OnTransportStateChangedResponseEvent += OnTransportStateChangedResponseReceived;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGetTeamTransportsResponseEvent -= OnGetTransportsResponseReceived;
        EventManager.Instance.OnTransportStateChangedResponseEvent -= OnTransportStateChangedResponseReceived;
    }

    private void OnGetTransportsResponseReceived(GetTeamTransportsResponse getTeamTransportsResponse)
    {
        Transports = getTeamTransportsResponse.myTeamTransports;

        //update in a list or something
    }

    private void OnTransportStateChangedResponseReceived(TransportStateChangedResponse transportStateChangedResponse)
    {
        //TODO
    }
}
