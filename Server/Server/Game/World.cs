using Server.Game.Players;
using Server.Helper.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class World
    {
        private int m_LevelIndex;

        private Dictionary<int, Player> m_PlayerMap;

        private List<Line> m_Lines;
        private Line m_CurrentLine;

        public World()
        {
            m_LevelIndex = 1;
            m_PlayerMap = new Dictionary<int, Player>();
            m_Lines = new List<Line>();
        }

        /// <summary>
        /// Get the ID for the current level loaded
        /// </summary>
        /// <returns>The ID for the current loaded level</returns>
        public int GetCurrentLevel()
        {
            return m_LevelIndex;
        }

        /// <summary>
        /// Stores the clientID and Player associated with that ID
        /// </summary>
        /// <param name="clientID">The ID of the client</param>
        /// <param name="player">The Player object to store</param>
        public void AddPlayer(int clientID, Player player)
        {
            if(!m_PlayerMap.ContainsKey(clientID))
            {
                m_PlayerMap.Add(clientID, player);
            }
        }

        /// <summary>
        /// Creates a new player from a clientID and stores it
        /// </summary>
        /// <param name="clientID">The clientID to use</param>
        public void NewPlayer(int clientID)
        {
            Player p = new Player(clientID);
            AddPlayer(clientID, p);
        }

        /// <summary>
        /// Retreives a player based on a clientID
        /// </summary>
        /// <param name="clientID">The clientID to use</param>
        /// <returns>The Player object for the clientID if the ID is found, otherwise null</returns>
        public Player GetPlayer(int clientID)
        {
            if(m_PlayerMap.ContainsKey(clientID))
            {
                return m_PlayerMap[clientID];
            }

            return null;
        }

        public Player GetPlayerFromIndex(int index)
        {
            if(index >= 0 && index < m_PlayerMap.Count)
            {
                return m_PlayerMap.Values.ToList()[index];
            }

            return null;
        }

        /// <summary>
        /// Removes a player from the stored data
        /// </summary>
        /// <param name="clientID">The client to remove</param>
        public void RemovePlayer(int clientID)
        {
            if(m_PlayerMap.ContainsKey(clientID))
            {
                m_PlayerMap.Remove(clientID);
            }
        }

        /// <summary>
        /// Gets the total number of players in the world
        /// </summary>
        /// <returns>An integer representing the total number of players in the world</returns>
        public int GetPlayerCount()
        {
            return m_PlayerMap.Count;
        }

        public Vector3 GetNextSpawnpoint()
        {
            return new Vector3(-3.0f, -1.0f, 0.0f);
        }

        public void StartNewLine()
        {
            m_CurrentLine = new Line();
        }

        public void StartNewLine(List<Vector2> points)
        {
            m_CurrentLine = new Line(points);
        }

        public void AddLinePoint(Vector2 point)
        {
            m_CurrentLine.Points.Add(point);
        }

        public void IncrementLineSegment()
        {
            m_CurrentLine.CurrentSegment++;
        }

        public Line GetCurrentLine()
        {
            return m_CurrentLine;
        }

        public int EndLine()
        {
            int index = m_Lines.Count;
            m_Lines.Add(m_CurrentLine);
            m_CurrentLine = null;
            return index;
        }

        public Line GetLineFromIndex(int index)
        {
            return (m_Lines[index]);
        }
    }
}
