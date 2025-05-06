using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Logic
    {
        /// <summary>
        /// Updates the internal logic of the server
        /// </summary>
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
