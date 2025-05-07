using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.TCP.SendData(packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.WelcomeReceived))
        {
            packet.WriteInt(Client.Instance.ID);
            packet.WriteString(Client.Instance.Username);

            SendTCPData(packet);
        }
    }

    public static void LevelReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.LevelReceived))
        {
            packet.WriteInt(Client.Instance.ID);

            SendTCPData(packet);
        }
    }

    public static void PlayerSync(Vector3 position)
    {
        using(Packet packet = new Packet((int)ClientPackets.PlayerSync))
        {
            packet.WriteInt(Client.Instance.ID);
            packet.WriteVector3(position);

            SendTCPData(packet);
        }
    }

    public static void CreateLine(PlayerLine line)
    {
        using (Packet packet = new Packet((int)ClientPackets.CreateLine))
        {
            packet.WriteInt(Client.Instance.ID);

            packet.WriteInt(line.GetPoints().Count);
            for(int i = 0; i < line.GetPoints().Count; i++)
            {
                packet.WriteVector2(line.GetPoints()[i]);
            }

            SendTCPData(packet);
        }
    }
    #endregion
}