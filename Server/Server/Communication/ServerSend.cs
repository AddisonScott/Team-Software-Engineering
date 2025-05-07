using Server.Game;
using Server.Game.Players;
using Server.Helper.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerSend
    {
        /// <summary>
        /// Send packet to a single client
        /// </summary>
        /// <param name="toClient">The client ID to send to</param>
        /// <param name="packet">The packet to send</param>
        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[toClient].TCP.SendData(packet);
        }

        /// <summary>
        /// Send packet to all clients
        /// </summary>
        /// <param name="packet">The packet to send</param>
        private static void SendTCPDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.Clients[i].TCP.SendData(packet);
            }
        }

        /// <summary>
        /// Send packet to all except one client
        /// </summary>
        /// <param name="exceptClient">The client ID to not send to</param>
        /// <param name="packet">The packet to send</param>
        private static void SendTCPDataToAllExcept(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for(int i = 1; i <= Server.MaxPlayers; i++)
            {
                if(i != exceptClient)
                {
                    Server.Clients[i].TCP.SendData(packet);
                }
            }
        }

        /// <summary>
        /// Sends the Welcome packet to a client
        /// </summary>
        /// <param name="toClient">The client to send to</param>
        /// <param name="msg">The welcome message to send</param>
        public static void Welcome(int toClient, string msg)
        {
            using(Packet packet = new Packet((int)ServerPackets.Welcome))
            {
                packet.WriteString(msg);
                packet.WriteInt(toClient);

                SendTCPData(toClient, packet);
            }
        }

        public static void SendLevel(int toClient, int levelID)
        {
            using(Packet packet = new Packet((int)ServerPackets.SendLevel))
            {
                packet.WriteInt(levelID); // The ID of the level to switch to
                packet.WriteBool(Program.World.GetPlayerCount() == 1); // Whether or not this is the first player
                packet.WriteInt(toClient);
                SendTCPData(toClient, packet);
            }
        }

        public static void SendPlayerPosition(int toClient)
        {
            using (Packet packet = new Packet((int)ServerPackets.SendPlayerPosition))
            {
                Player player = Program.World.GetPlayer(toClient);

                packet.WriteVector3(player.Position);
                packet.WriteInt(toClient);

                SendTCPData(toClient, packet);
            }
        }

        public static void SendOtherPlayerPosition(int toClient, Player player)
        {
            using (Packet packet = new Packet((int)ServerPackets.SendOtherPlayerPosition))
            {
                packet.WriteVector3(player.Position);
                packet.WriteInt(toClient);

                SendTCPData(toClient, packet);
            }
        }

        public static void PlayerJoined(int clientID)
        {
            using (Packet packet = new Packet((int)ServerPackets.PlayerJoined))
            {
                Player player = Program.World.GetPlayer(clientID);

                packet.WriteVector3(player.Position);
                packet.WriteInt(clientID);

                SendTCPDataToAllExcept(clientID, packet);
            }
        }

        public static void UpdateOtherPlayer(int clientID, Vector3 newPosition)
        {
            using (Packet packet = new Packet((int)ServerPackets.UpdateOtherPlayer))
            {
                packet.WriteVector3(newPosition);
                packet.WriteInt(clientID);

                SendTCPDataToAllExcept(clientID, packet);
            }
        }

        public static void LineCreate(int clientID, int lineIndex)
        {
            List<Vector2> positions = Program.World.GetLineFromIndex(lineIndex).Points;
            using (Packet packet = new Packet((int)ServerPackets.LineCreate))
            {
                packet.WriteInt(positions.Count);
                for(int i = 0; i < positions.Count; i++)
                {
                    packet.WriteVector2(positions[i]);
                }

                packet.WriteInt(clientID);
                SendTCPDataToAllExcept(clientID, packet);
            }
        }

        public static void LineRemove(int clientID, int lineIndex)
        {
            using (Packet packet = new Packet((int)ServerPackets.LineRemove))
            {
                packet.WriteInt(lineIndex);
                packet.WriteInt(clientID);
                SendTCPDataToAllExcept(clientID, packet);
            }
        }
    }
}
