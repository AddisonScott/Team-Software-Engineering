using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> Clients { get; private set; }
        public delegate void PacketHandler(int fromClient, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers { get; private set; }

        private static TcpListener m_Listener;

        /// <summary>
        /// Initialise the server and start waiting for incoming client connections
        /// </summary>
        /// <param name="maxPlayers">The maximum number of players that the server will allow</param>
        /// <param name="port">The port that the server should run on</param>
        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;

            Console.WriteLine("Starting server...");
            InitialiseServerData();

            m_Listener = new TcpListener(IPAddress.Any, Port);
            m_Listener.Start();
            m_Listener.BeginAcceptTcpClient(TCPAttemptConnect, null);

            Console.WriteLine($"Server started on port {Port}");
        }

        /// <summary>
        /// Called by TcpListener when a TcpClient connection request comes in
        /// </summary>
        /// <param name="result">Unused</param>
        private static void TCPAttemptConnect(IAsyncResult result)
        {
            TcpClient client = m_Listener.EndAcceptTcpClient(result);
            m_Listener.BeginAcceptTcpClient(TCPAttemptConnect, null);
            Console.WriteLine($"Connection attempt from {client.Client.RemoteEndPoint}");

            for(int i = 1; i <= MaxPlayers; i++)
            {
                if (Clients[i].TCP.Socket == null)
                {
                    Clients[i].TCP.Connect(client);
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full");
        }

        /// <summary>
        /// Initialises internal server data
        /// </summary>
        private static void InitialiseServerData()
        {
            Clients = new Dictionary<int, Client>();
            for(int i = 1; i <= MaxPlayers; i++)
            {
                Clients.Add(i, new Client(i));
            }

            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.LevelReceived, ServerHandle.LevelReceived },
                { (int)ClientPackets.PlayerSync, ServerHandle.PlayerSync },
            };
            Console.WriteLine("Initialised Server");
        }
    }
}
