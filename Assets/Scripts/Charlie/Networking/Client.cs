using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public static Client Instance;
    public static int DataBufferSize = 4096;

    public string Host;
    public string Username;
    public int Port = 26950;
    public int ID = 0;
    public TCPConnection TCP;

    [HideInInspector] public WorldManager WorldManager;
    [HideInInspector] public bool FirstPlayer;

    private bool m_IsConnected = false;

    public delegate void PacketHandler(Packet packet);
    public static Dictionary<int, PacketHandler> PacketHandlers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // NOTE: Maybe these should have some default values?
            Host = "";
            Username = "";
        }
        else
        {
            Debug.Log("Instance of Client already exists, destroying object");
            Destroy(this);
        }
    }

    private void Start()
    {
        TCP = new TCPConnection();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Connect(string host, string username)
    {
        Host = host;
        Username = username;
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        InitializeClientData();

        m_IsConnected = true;
        TCP.Connect();
    }

    private void InitializeClientData()
    {
        PacketHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.Welcome, ClientHandle.Welcome },
            { (int)ServerPackets.SendLevel, ClientHandle.SendLevel },
            { (int)ServerPackets.SendPlayerPosition, ClientHandle.SendPlayerPosition },
            { (int)ServerPackets.SendOtherPlayerPosition, ClientHandle.SendOtherPlayerPosition },
            { (int)ServerPackets.PlayerJoined, ClientHandle.PlayerJoined },
            { (int)ServerPackets.UpdateOtherPlayer, ClientHandle.UpdateOtherPlayer },
            { (int)ServerPackets.LineCreate, ClientHandle.LineCreate },
        };
        Debug.Log("Initialized packets.");
    }

    public void Disconnect()
    {
        if (m_IsConnected)
        {
            m_IsConnected = false;
            TCP.Socket.Close();

            Debug.Log("Disconnected from the server");
        }
    }
}
