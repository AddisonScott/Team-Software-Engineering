using Server.Game;
using Server.Helper.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerHandle
    {
        /// <summary>
        /// Handles the incoming WelcomeReceived packet from client
        /// </summary>
        /// <param name="fromClient">The ID of the client that send this packet</param>
        /// <param name="packet">The packet data itself</param>
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientID = packet.ReadInt();
            string username = packet.ReadString();

            if (fromClient != clientID)
            {
                Console.WriteLine($"\"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientID})!");
                return;
            }

            Console.WriteLine($"{Server.Clients[clientID].TCP.Socket.Client.RemoteEndPoint} connected successfully and is now player {clientID}");

            Server.Clients[clientID].Data.Username = username;
            Program.World.NewPlayer(clientID);

            ServerSend.PlayerJoined(clientID);

            Console.WriteLine($"Sending level to player {clientID}...");
            ServerSend.SendLevel(clientID, Program.World.GetCurrentLevel());
        }

        public static void LevelReceived(int fromClient, Packet packet)
        {
            int clientID = packet.ReadInt();

            if (fromClient != clientID)
            {
                Console.WriteLine($"\"{Server.Clients[clientID].Data.Username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientID})!");
                return;
            }

            ServerSend.SendPlayerPosition(fromClient);
            if(Program.World.GetPlayerCount() == 2)
            {
                ServerSend.SendOtherPlayerPosition(fromClient, Program.World.GetPlayerFromIndex(0));
            }
        }

        public static void PlayerSync(int fromClient, Packet packet)
        {
            int clientID = packet.ReadInt();

            if (fromClient != clientID)
            {
                Console.WriteLine($"\"{Server.Clients[clientID].Data.Username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientID})!");
                return;
            }

            Vector3 newPosition = packet.ReadVector3();
            Program.World.GetPlayer(clientID).Position = newPosition;
            ServerSend.UpdateOtherPlayer(clientID, newPosition);
        }

        public static void CreateLine(int fromClient, Packet packet)
        {
            int clientID = packet.ReadInt();

            if (fromClient != clientID)
            {
                Console.WriteLine($"\"{Server.Clients[clientID].Data.Username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientID})!");
                return;
            }

            int count = packet.ReadInt();
            Program.World.StartNewLine();

            for(int i = 0; i < count; i++)
            {
                Program.World.AddLinePoint(packet.ReadVector2());
            }

            int lineIndex = Program.World.EndLine();
            ServerSend.LineCreate(clientID, lineIndex);
        }
    }
}
