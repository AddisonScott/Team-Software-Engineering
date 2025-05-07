using Server.Helper.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public enum ServerPackets
    {
        Welcome = 1,
        SendLevel = 2,
        SendPlayerPosition = 3,
        SendOtherPlayerPosition = 4,
        PlayerJoined = 5,
        UpdateOtherPlayer = 6,
        LineCreate = 7,
        LineRemove = 8,
        GameWon = 9,
        DeadPlayer = 10,
        PlayerDisconnected = 11,
    }

    public enum ClientPackets
    {
        WelcomeReceived = 1,
        LevelReceived = 2,
        PlayerSync = 3,
        CreateLine = 4,
        RemoveLine = 5,
        EnteredGoal = 6,
        FinishedGame = 7,
        PlayerDied = 8,
    }

    class Packet : IDisposable
    {
        private byte[] m_Readable;
        private List<byte> m_Buffer;
        private int m_ReadPos;

        public Packet()
        {
            m_Buffer = new List<byte>();
            m_ReadPos = 0;
        }

        public Packet(int id)
        {
            m_Buffer = new List<byte>();
            m_ReadPos = 0;

            WriteInt(id);
        }

        public Packet(byte[] data)
        {
            m_Buffer = new List<byte>();
            m_ReadPos = 0;

            SetBytes(data);
        }

        #region Read
        /// <summary>
        /// Read a single byte from this packet
        /// </summary>
        /// <returns>The read byte</returns>
        /// <exception cref="Exception">Thrown if byte can't be read due to reaching the end of the packet</exception>
        public byte Read()
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                byte value = m_Readable[m_ReadPos];
                m_ReadPos += 1;
                return value;
            }
            else
            {
                throw new Exception("Could not read the value of byte");
            }
        }

        /// <summary>
        /// Read a byte array from this packet
        /// </summary>
        /// <param name="length">The number of bytes to read</param>
        /// <returns>The read byte array</returns>
        /// <exception cref="Exception">Thrown if bytes can't be read due to reaching the end of the packet</exception>
        public byte[] Read(int length)
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                byte[] values = m_Buffer.GetRange(m_ReadPos, length).ToArray();
                m_ReadPos += length;
                return values;
            }
            else
            {
                throw new Exception("Could not read the value of bytes");
            }
        }

        /// <summary>
        /// Read an int from this packet
        /// </summary>
        /// <returns>The read int</returns>
        /// <exception cref="Exception">Thrown if int can't be read due to reaching the end of the packet</exception>
        public int ReadInt()
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                int value = BitConverter.ToInt32(m_Readable, m_ReadPos);
                m_ReadPos += 4;
                return value;
            }
            else
            {
                throw new Exception("Could not read the value of int");
            }
        }

        /// <summary>
        /// Read a float from this packet
        /// </summary>
        /// <returns>The read float</returns>
        /// <exception cref="Exception">Thrown if float can't be read due to reaching the end of the packet</exception>
        public float ReadFloat()
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                float value = BitConverter.ToSingle(m_Readable, m_ReadPos);
                m_ReadPos += 4;
                return value;
            }
            else
            {
                throw new Exception("Could not read the value of float");
            }
        }

        /// <summary>
        /// Read a long from this packet
        /// </summary>
        /// <returns>The read long</returns>
        /// <exception cref="Exception">Thrown if long can't be read due to reaching the end of the packet</exception>
        public long ReadLong()
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                long value = BitConverter.ToInt64(m_Readable, m_ReadPos);
                m_ReadPos += 8;
                return value;
            }
            else
            {
                throw new Exception("Could not read the value of long");
            }
        }

        /// <summary>
        /// Read a short from this packet
        /// </summary>
        /// <returns>The read short</returns>
        /// <exception cref="Exception">Thrown if short can't be read due to reaching the end of the packet</exception>
        public short ReadShort()
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                short value = BitConverter.ToInt16(m_Readable, m_ReadPos);
                m_ReadPos += 2;
                return value;
            }
            else
            {
                throw new Exception("Could not read the value of short");
            }
        }

        /// <summary>
        /// Read a bool from this packet
        /// </summary>
        /// <returns>The read bool</returns>
        /// <exception cref="Exception">Thrown if bool can't be read due to reaching the end of the packet</exception>
        public bool ReadBool()
        {
            if (m_Buffer.Count > m_ReadPos)
            {
                bool value = BitConverter.ToBoolean(m_Readable, m_ReadPos);
                m_ReadPos += 1;
                return value;
            }
            else
            {
                throw new Exception("Could not read the value of bool");
            }
        }

        /// <summary>
        /// Read a string from this packet
        /// </summary>
        /// <returns>The read string</returns>
        /// <exception cref="Exception">Thrown if string can't be read due to reaching the end of the packet</exception>
        public string ReadString()
        {
            try
            {
                int length = ReadInt();
                string value = Encoding.ASCII.GetString(m_Readable, m_ReadPos, length);
                m_ReadPos += length;
                return value;
            }
            catch
            {
                throw new Exception("Could not read the value of string");
            }
        }

        /// <summary>
        /// Read a Vector2 from this packet
        /// </summary>
        /// <returns>The read Vector2</returns>
        public Vector2 ReadVector2()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            return new Vector2(x, y);
        }

        /// <summary>
        /// Read a Vector3 from this packet
        /// </summary>
        /// <returns>The read Vector2</returns>
        public Vector3 ReadVector3()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Read a Vector4 from this packet
        /// </summary>
        /// <returns>The read Vector2</returns>
        public Vector4 ReadVector4()
        {
            float x = ReadFloat();
            float y = ReadFloat();
            float z = ReadFloat();
            float w = ReadFloat();
            return new Vector4(x, y, z, w);
        }
        #endregion

        #region Write
        /// <summary>
        /// Write a byte into this packet
        /// </summary>
        /// <param name="data">The byte to write</param>
        public void Write(byte data)
        {
            m_Buffer.Add(data);
        }

        /// <summary>
        /// Write a byte array into this packet
        /// </summary>
        /// <param name="data">The byte array to write</param>
        public void Write(byte[] data)
        {
            m_Buffer.AddRange(data);
        }

        /// <summary>
        /// Write an int into this packet
        /// </summary>
        /// <param name="data">The int to write</param>
        public void WriteInt(int data)
        {
            m_Buffer.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Write a float into this packet
        /// </summary>
        /// <param name="data">The float to write</param>
        public void WriteFloat(float data)
        {
            m_Buffer.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Write a long into this packet
        /// </summary>
        /// <param name="data">The long to write</param>
        public void WriteLong(long data)
        {
            m_Buffer.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Write a short into this packet
        /// </summary>
        /// <param name="data">The short to write</param>
        public void WriteShort(short data)
        {
            m_Buffer.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Write a bool into this packet
        /// </summary>
        /// <param name="data">The bool to write</param>
        public void WriteBool(bool data)
        {
            m_Buffer.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Write an ASCII string into this packet
        /// </summary>
        /// <param name="data">The string to write</param>
        public void WriteString(string data)
        {
            WriteInt(data.Length);
            m_Buffer.AddRange(Encoding.ASCII.GetBytes(data)); // NOTE: This only allows the sending of ASCII characters
        }

        /// <summary>
        /// Write a Vector2 into this packet
        /// </summary>
        /// <param name="data"></param>
        public void WriteVector2(Vector2 data)
        {
            WriteFloat(data.X);
            WriteFloat(data.Y);
        }

        /// <summary>
        /// Write a Vector3 into this packet
        /// </summary>
        /// <param name="data"></param>
        public void WriteVector3(Vector3 data)
        {
            WriteFloat(data.X);
            WriteFloat(data.Y);
            WriteFloat(data.Z);
        }

        /// <summary>
        /// Write a Vector4 into this packet
        /// </summary>
        /// <param name="data"></param>
        public void WriteVector4(Vector4 data)
        {
            WriteFloat(data.X);
            WriteFloat(data.Y);
            WriteFloat(data.Z);
            WriteFloat(data.W);
        }
        #endregion

        #region Utility
        /// <summary>
        /// Set the byte data for this packet
        /// </summary>
        /// <param name="_data">The byte data to use</param>
        public void SetBytes(byte[] _data)
        {
            Write(_data);
            m_Readable = m_Buffer.ToArray();
        }
        
        /// <summary>
        /// Insert the length of the byte data at the start of the packet
        /// </summary>
        public void WriteLength()
        {
            m_Buffer.InsertRange(0, BitConverter.GetBytes(m_Buffer.Count));
        }

        /// <summary>
        /// Insert an int at the start of the packet
        /// </summary>
        /// <param name="val">The int to insert</param>
        public void InsertInt(int val)
        {
            m_Buffer.InsertRange(0, BitConverter.GetBytes(val));
        }

        /// <summary>
        /// Convert the packet data to a byte array
        /// </summary>
        /// <returns>The byte array containing the packet data</returns>
        public byte[] ToArray()
        {
            m_Readable = m_Buffer.ToArray();
            return m_Readable;
        }

        /// <summary>
        /// Get the length of the packet data
        /// </summary>
        /// <returns>The number of bytes in the packet</returns>
        public int Length()
        {
            return m_Buffer.Count;
        }

        /// <summary>
        /// Get the length of the packet data that hasn't been read yet
        /// </summary>
        /// <returns>The number of remaining bytes in the packet</returns>
        public int LengthLeft()
        {
            return Length() - m_ReadPos;
        }

        /// <summary>
        /// Clear the data within the packet and reset it ready for re-use
        /// </summary>
        /// <param name="shouldReset">Whether or not the packet should be reset</param>
        public void Reset(bool shouldReset = true)
        {
            if(shouldReset)
            {
                m_Buffer.Clear();
                m_Readable = null;
                m_ReadPos = 0;
            }
            else
            {
                m_ReadPos -= 4;
            }
        }
        #endregion

        #region GC
        private bool m_Disposed = false;

        protected virtual void DisposePacket(bool disposing)
        {
            if(!m_Disposed)
            {
                if(disposing)
                {
                    m_Buffer = null;
                    m_Readable = null;
                    m_ReadPos = 0;
                }

                m_Disposed = true;
            }
        }

        public void Dispose()
        {
            DisposePacket(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
