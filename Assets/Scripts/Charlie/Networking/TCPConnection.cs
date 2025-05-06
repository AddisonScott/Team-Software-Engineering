using System.Net.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TCPConnection
{
    public TcpClient Socket;

    private NetworkStream m_Stream;
    private Packet m_ReceivedData;
    private byte[] m_ReceiveBuffer;

    public void Connect()
    {
        Socket = new TcpClient
        {
            ReceiveBufferSize = Client.DataBufferSize,
            SendBufferSize = Client.DataBufferSize
        };

        m_ReceiveBuffer = new byte[Client.DataBufferSize];
        Socket.BeginConnect(Client.Instance.Host, Client.Instance.Port, ConnectCallback, Socket);
    }

    private void ConnectCallback(IAsyncResult result)
    {
        Socket.EndConnect(result);

        if (!Socket.Connected)
        {
            return;
        }

        m_Stream = Socket.GetStream();

        m_ReceivedData = new Packet();

        m_Stream.BeginRead(m_ReceiveBuffer, 0, Client.DataBufferSize, ReceiveCallback, null);
    }

    public void SendData(Packet packet)
    {
        try
        {
            if (Socket != null)
            {
                m_Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error sending data to server via TCP: {e}");
        }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int byteLength = m_Stream.EndRead(result);
            if (byteLength <= 0)
            {
                Client.Instance.Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(m_ReceiveBuffer, data, byteLength);

            m_ReceivedData.Reset(HandleData(data));
            m_Stream.BeginRead(m_ReceiveBuffer, 0, Client.DataBufferSize, ReceiveCallback, null);
        }
        catch
        {
            Disconnect();
        }
    }

    private bool HandleData(byte[] data)
    {
        int packetLength = 0;

        m_ReceivedData.SetBytes(data);

        if (m_ReceivedData.LengthLeft() >= 4)
        {
            packetLength = m_ReceivedData.ReadInt();
            if (packetLength <= 0)
            {
                return true;
            }
        }

        while (packetLength > 0 && packetLength <= m_ReceivedData.LengthLeft())
        {
            byte[] packetBytes = m_ReceivedData.Read(packetLength);
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Client.PacketHandlers[packetId](packet);
                }
            });

            packetLength = 0;
            if (m_ReceivedData.LengthLeft() >= 4)
            {
                packetLength = m_ReceivedData.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }
        }

        if (packetLength <= 1)
        {
            return true;
        }

        return false;
    }

    private void Disconnect()
    {
        Client.Instance.Disconnect();

        m_Stream = null;
        m_ReceivedData = null;
        m_ReceiveBuffer = null;
        Socket = null;
    }
}