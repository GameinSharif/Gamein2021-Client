using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkTransport
{
    private static NetworkTransport _instance;
    private readonly UdpClient _socket;
    private byte[] _data;
    private int _seqNumber = -1;
    private Thread _receiverThread;
    private List<Thread> _senderThreads = new List<Thread>();
        
    private NetworkTransport()
    {
        _socket = new UdpClient(Constants.ReceivePort);
        _socket.Client.ReceiveBufferSize = 4096;
        _socket.Client.SendBufferSize = 4096;
        var packetReceiver = new PacketReceiver(_socket, new IPEndPoint(IPAddress.Any, Constants.ReceivePort));
        _receiverThread = new Thread(packetReceiver.ReceivePacket);
        _receiverThread.Start();
    }

    public Packet[] ReceivedPackets { get; } = new Packet[Constants.WindowSize];

    public bool[] ReceivedAcks { get; } = new bool[Constants.WindowSize];

    public PacketSender[] SendDataThreads { get; } = new PacketSender[Constants.WindowSize];

    public int ReceivedSequenceNumber { get; set; } = 0;

    private void IncrementSequenceNumber()
    {
        _seqNumber++;
    }


    public static NetworkTransport GetInstance()
    {
        if (_instance == null)
            _instance = new NetworkTransport();
        return _instance;
    }

    public void GenerateData<T>(RequestObject requestObject)
    {
        if (_seqNumber - ReceivedSequenceNumber >= Constants.WindowSize) return;
        IncrementSequenceNumber();
        // TODO encrypt enc data in request object
        var packet = new Packet(_seqNumber, Encoding.Default.GetBytes(JsonUtility.ToJson(requestObject)));
        var packetSender = new PacketSender(_socket, packet);
        var thread = new Thread(packetSender.SendPacket);
        Debug.Log("request -> " + _seqNumber + ": " + JsonUtility.ToJson(requestObject));
        SendDataThreads[_seqNumber % Constants.WindowSize] = packetSender;
        ReceivedAcks[_seqNumber % Constants.WindowSize] = false;
        _senderThreads.Add(thread);
        thread.Start();
    }
    
    public void OnApplicationQuit()
    {
        _socket.Close();
        _receiverThread.Abort();
        foreach (var senderThread in _senderThreads)
        {
            senderThread.Abort();
        }
    }
}