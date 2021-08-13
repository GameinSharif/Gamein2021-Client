using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PacketReceiver
{
    private readonly UdpClient _socket;
    private IPEndPoint _ipEndPoint;

    public PacketReceiver(UdpClient socket, IPEndPoint ipEndPoint)
    {
        _socket = socket;
        _ipEndPoint = ipEndPoint;
    }

    public void ReceivePacket()
    {
        while (true)
        {
            var packet = new Packet(_socket.Receive(ref _ipEndPoint));
            if (packet.CalculateCheckSum() != packet.checkSum) continue;
            var sequenceNumber = packet.packetNumber;
            NetworkTransport.GetInstance().ReceivedPackets[sequenceNumber % Constants.WindowSize] = packet;
            NetworkTransport.GetInstance().ReceivedAcks[sequenceNumber % Constants.WindowSize] = true;
            if (packet.packetNumber != NetworkTransport.GetInstance().ReceivedSequenceNumber) continue;
            while (NetworkTransport.GetInstance().ReceivedAcks[sequenceNumber % Constants.WindowSize])
            {
                var receivedPacket = NetworkTransport.GetInstance()
                    .ReceivedPackets[sequenceNumber % Constants.WindowSize];
                NetworkTransport.GetInstance().SendDataThreads[sequenceNumber % Constants.WindowSize].SetDone();
                Debug.Log("received -> " + receivedPacket.packetNumber + ": " + Encoding.UTF8.GetString(packet.data));
                RequestHandler.HandleServerResponse(Encoding.UTF8.GetString(receivedPacket.data));
                NetworkTransport.GetInstance().ReceivedAcks[sequenceNumber % Constants.WindowSize] = false;
                sequenceNumber++;
            }

            NetworkTransport.GetInstance().ReceivedSequenceNumber = sequenceNumber;
        }
    }
}    