using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientHandle
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int id = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.Instance.ID = id;
        ClientSend.WelcomeReceived();
    }

    public static void SendLevel(Packet packet)
    {
        int levelID = packet.ReadInt();

        SceneManager.LoadScene(levelID);
        ClientSend.LevelReceived();
    }

    public static void SendPlayerPosition(Packet packet)
    {
        Vector3 position = packet.ReadVector3();
        Client.Instance.WorldManager.CreatePlayer(position);
    }

    public static void SendOtherPlayerPosition(Packet packet)
    {
        Vector3 position = packet.ReadVector3();
        Client.Instance.WorldManager.CreateOtherPlayer(position);
    }

    public static void PlayerJoined(Packet packet)
    {
        Vector3 position = packet.ReadVector3();
        Client.Instance.WorldManager.CreateOtherPlayer(position);
    }

    public static void UpdateOtherPlayer(Packet packet)
    {
        Vector3 pos = packet.ReadVector3();
        Client.Instance.WorldManager.PositionOtherPlayer(pos);
    }
}