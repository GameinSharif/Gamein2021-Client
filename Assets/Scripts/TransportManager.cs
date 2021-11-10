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
    }

    private void OnTransportStateChangedResponseReceived(TransportStateChangedResponse transportStateChangedResponse)
    {
        Utils.Transport transport = GetTransportById(transportStateChangedResponse.transport.id);
        switch (transportStateChangedResponse.transport.transportState)
        {
            case Utils.TransportState.IN_WAY:
                transport.transportState = Utils.TransportState.IN_WAY;
                break;
            case Utils.TransportState.SUCCESSFUL:
                Transports.Remove(transport);
                //TODO notification or something
                break;
            case Utils.TransportState.CRUSHED:
                Transports.Remove(transport);
                //TODO notification or something
                break;
            case Utils.TransportState.PENDING:
                Transports.Add(transportStateChangedResponse.transport);
                break;
        }

        if (MapManager.IsInMap)
        {
            MapManager.Instance.UpdateLine(transportStateChangedResponse.transport);
        }
    }

    private Utils.Transport GetTransportById(int id)
    {
        return Transports.Find(t => t.id == id);
    }
}
