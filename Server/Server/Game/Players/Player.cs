using Server.Helper.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Players
{
    public class Player
    {
        public int ClientID;
        public Vector3 Position;

        public Player(int clientID)
        {
            ClientID = clientID;
            Position = Program.World.GetNextSpawnpoint();
        }
    }
}
