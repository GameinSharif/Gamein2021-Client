using System;

[Serializable]
public class Packet
{
    public Packet(int packetNumber, byte[] data)
    {
        this.packetNumber = packetNumber;
        this.data = data;
        checkSum = CalculateCheckSum();
    }
    

    public Packet(byte[] segment)
    {
        packetNumber = ((segment[0] & 0xFF) << 24) + ((segment[1] & 0xFF) << 16) + ((segment[2] & 0xFF) << 8)
                       + (segment[3] & 0xFF);
        checkSum = ((segment[4] & 0xFF) << 8) + (segment[5] & 0xFF);
        data = new byte[segment.Length - 6];
        Array.Copy(segment, 6, data, 0, data.Length);
    }

    public int checkSum;
    public int packetNumber;
    public byte[] data;

    public byte[] GetByteArray()
    {
        var segment = new byte[data.Length + 6];
        segment[0] = (byte) (packetNumber >> 24);
        segment[1] = (byte) (packetNumber >> 16);
        segment[2] = (byte) (packetNumber >> 8);
        segment[3] = (byte) packetNumber;
        segment[4] = (byte) (checkSum >> 8);
        segment[5] = (byte) checkSum;
        Array.Copy(data, 0, segment, 6, data.Length);
        return segment;
    }

    public int CalculateCheckSum()
    {
        var segment = GetByteArray();
        int sum = 0, i;
        for (i = 0; i < segment.Length - 1; i += 2)
        {
            if (i == 4) continue;
            sum += ((segment[i] & 0xFF) << 8) + (segment[i + 1] & 0xFF);
            sum = (sum & 65535) + (sum >> 16);
        }

        if (i != segment.Length - 1) return sum;
        sum += segment[i] << 8;
        sum = (sum & 65535) + (sum >> 16);
        return sum;
    }
}