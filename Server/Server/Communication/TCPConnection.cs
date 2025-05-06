using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TCPConnection
    {
        public TcpClient Socket { get; private set; }

        private readonly int m_ID;
        private NetworkStream m_Stream;
        private Packet m_ReceivedData;
        private byte[] m_ReceiveBuffer;

        public TCPConnection(int id)
        {
            m_ID = id;
        }

        /// <summary>
        /// Connects a TcpClient instance to this TCPConnection, thereby linking the two
        /// </summary>
        /// <param name="socket">The TcpClient instance to connect</param>
        public void Connect(TcpClient socket)
        {
            Socket = socket;
            Socket.ReceiveBufferSize = Constants.DATA_BUFFER_SIZE;
            Socket.SendBufferSize = Constants.DATA_BUFFER_SIZE;

            m_Stream = Socket.GetStream();

            m_ReceivedData = new Packet();
            m_ReceiveBuffer = new byte[Constants.DATA_BUFFER_SIZE];

            m_Stream.BeginRead(m_ReceiveBuffer, 0, Constants.DATA_BUFFER_SIZE, ReceiveCallback, null);
            ServerSend.Welcome(m_ID, "Welcome to the game!");
        }

        /// <summary>
        /// Send a packet to the client this connection refers to
        /// </summary>
        /// <param name="packet">The packet to send</param>
        public void SendData(Packet packet)
        {
            try
            {
                if(Socket != null)
                {
                    m_Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error sending data to client {m_ID} via TCP: {e}");
            }
        }

        /// <summary>
        /// Called by NetworkStream when data is received from a client
        /// </summary>
        /// <param name="result">Unused</param>
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = m_Stream.EndRead(result);
                if (byteLength <= 0)
                {
                    Server.Clients[m_ID].Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(m_ReceiveBuffer, data, byteLength);

                m_ReceivedData.Reset(HandleData(data));
                m_Stream.BeginRead(m_ReceiveBuffer, 0, Constants.DATA_BUFFER_SIZE, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving TCP data: {e}");

                Server.Clients[m_ID].Disconnect();
            }
        }

        /// <summary>
        /// Reads and performs actions on the incoming packet
        /// </summary>
        /// <param name="data">The byte data of the incoming packet</param>
        /// <returns>True if the packet has been read to completion, otherwise false</returns>
        private bool HandleData(byte[] data)
        {
            int packetLength = 0;
            m_ReceivedData.SetBytes(data);

            if(m_ReceivedData.LengthLeft() >= 4)
            {
                packetLength = m_ReceivedData.ReadInt();
                if(packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= m_ReceivedData.LengthLeft())
            {
                byte[] packetBytes = m_ReceivedData.Read(packetLength);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using(Packet packet = new Packet(packetBytes))
                    {
                        int packetID = packet.ReadInt();
                        Server.PacketHandlers[packetID](m_ID, packet);
                    }
                });

                packetLength = 0;
                if(m_ReceivedData.LengthLeft() >= 4)
                {
                    packetLength = m_ReceivedData.ReadInt();
                    if(packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            return packetLength <= 1;
        }

        /// <summary>
        /// Closes the TCP socket and cleans up the remaining data
        /// </summary>
        public void Disconnect()
        {
            Socket.Close();
            m_Stream = null;
            m_ReceivedData = null;
            m_ReceiveBuffer = null;
            Socket = null;
        }
    }
}
