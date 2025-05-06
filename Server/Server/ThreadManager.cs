using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ThreadManager
    {
        private static readonly List<Action> m_ExecuteOnMainThread = new List<Action>();
        private static readonly List<Action> m_ExecuteCopiedOnMainThread = new List<Action>();
        private static bool m_ActionToExecuteOnMainThread = false;

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }

            lock (m_ExecuteOnMainThread)
            {
                m_ExecuteOnMainThread.Add(_action);
                m_ActionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        public static void UpdateMain()
        {
            if (m_ActionToExecuteOnMainThread)
            {
                m_ExecuteCopiedOnMainThread.Clear();
                lock (m_ExecuteOnMainThread)
                {
                    m_ExecuteCopiedOnMainThread.AddRange(m_ExecuteOnMainThread);
                    m_ExecuteOnMainThread.Clear();
                    m_ActionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < m_ExecuteCopiedOnMainThread.Count; i++)
                {
                    m_ExecuteCopiedOnMainThread[i]();
                }
            }
        }
    }
}
