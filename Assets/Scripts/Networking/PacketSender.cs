using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class PacketSender
{
    private readonly byte[] _packet;
    private readonly UdpClient _socket;
    private readonly int _timeout = Constants.Timeout;
    private bool _isDone;

    public PacketSender(UdpClient socket, Packet packet)
    {
        _socket = socket;
        _packet = packet.GetByteArray();
    }

    public void SendPacket()
    {
        while (!_isDone)
        {
            _socket.Send(_packet, _packet.Length, Constants.GatewayIp, Constants.GatewayPort);
            Thread.Sleep(_timeout);
        }
    }

    public void SetDone()
    {
        _isDone = true;
    }
}