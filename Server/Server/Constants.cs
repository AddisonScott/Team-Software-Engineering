using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Constants
    {
        public const int DATA_BUFFER_SIZE = 4096;
        public const int MAX_PLAYERS_PER_LOBBY = 2;
        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;
    }
}
