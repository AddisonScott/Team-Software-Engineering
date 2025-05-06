using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helper.Math
{
    public class Vector3 : Vector
    {
        private float _x;
        private float _y;
        private float _z;

        public float X { get { return _x; } set { _x = value; ChangeValues(); } }
        public float Y { get { return _y; } set { _y = value; ChangeValues(); } }
        public float Z { get { return _z; } set { _z = value; ChangeValues(); } }

        public Vector3() : base(3)
        {

        }

        public Vector3(float x, float y, float z) : base(3)
        {
            _x = x;
            _y = y;
            _z = z;
            ChangeValues();
        }

        protected override void ValuesUpdated()
        {
            _x = GetValues()[0];
            _y = GetValues()[1];
            _z = GetValues()[2];
        }

        private void ChangeValues()
        {
            UpdateValues(new float[3] { _x, _y, _z });
        }
    }
}
