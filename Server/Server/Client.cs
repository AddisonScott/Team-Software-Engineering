using Server.Data;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        public int ID { get; private set; }
        public ClientData Data { get; private set; }
        public TCPConnection TCP { get; private set; }

        public Client(int id)
        {
            ID = id;
            TCP = new TCPConnection(id);
            Data = new ClientData();
        }

        /// <summary>
        /// Disconnects the client from the server. Also automatically removes them from their lobby if they're in one
        /// </summary>
        public void Disconnect()
        {
            Console.WriteLine($"\"{Data.Username}\" (ID: {ID}) has disconnected");
            ServerSend.PlayerDisconnected(ID);
            Program.World.RemovePlayer(ID);
            TCP.Disconnect();
        }
    }
}
