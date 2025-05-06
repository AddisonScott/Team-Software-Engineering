using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helper.Math
{
    public abstract class Vector
    {
        private int m_ElementCount = 0;
        private float[] m_Values;

        protected Vector(int elementCount)
        {
            m_Values = new float[m_ElementCount];
            for(int i = 0; i < m_ElementCount; i++)
            {
                m_Values[i] = 0.0f;
            }
        }

        /// <summary>
        /// Get the number of elements for the current Vector
        /// </summary>
        /// <returns>An integer value for the number of elements</returns>
        public int Size()
        {
            return m_ElementCount;
        }

        /// <summary>
        /// Get the stored values for each element
        /// </summary>
        /// <returns>A float array of size ElementCount containing the data for the Vector</returns>
        public float[] GetValues()
        {
            return m_Values;
        }

        /// <summary>
        /// Updates the stored values for the Vector
        /// </summary>
        /// <param name="values">The float array to set the data to</param>
        protected void UpdateValues(float[] values)
        {
            if(values.Length != m_ElementCount)
            {
                return;
            }

            m_Values = values;
        }

        /// <summary>
        /// Run when the values are changed directly
        /// </summary>
        protected abstract void ValuesUpdated();
    }
}
