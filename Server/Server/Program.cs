using Server.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static bool s_Running = false;

        public static World World { get; private set; }

        static void Main(string[] args)
        {
            Console.Title = "TSE Server";
            s_Running = true;

            World = new World();

            Thread main = new Thread(new ThreadStart(MainThread));
            main.Start();

            Server.Start(2, 26950);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main Thread started. Running at {Constants.TICKS_PER_SEC} ticks per second");
            DateTime nextLoop = DateTime.Now;

            while(s_Running)
            {
                while(nextLoop < DateTime.Now)
                {
                    Logic.Update();

                    if(World.HasEnteredGoal())
                    {
                        // NOTE: Sometimes the packet for ending the game is missed by the spectator player
                        // This code ensures that the packet is sent every tick until that player responds
                        if (!World.IsGameFinished())
                        {
                            ServerSend.GameWon(World.GetGoalEntererID());
                        }
                        else
                        {
                            // NOTE: Once we are sure that the game has fully ended and both clients are disconnected,
                            // we can reset the World ready for the next players (so that we don't have to keep closing
                            // and reopening the server)
                            World.Reset();
                        }
                    }

                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
