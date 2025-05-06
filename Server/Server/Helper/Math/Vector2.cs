using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helper.Math
{
    public class Vector2 : Vector
    {
        private float _x;
        private float _y;

        public float X { get { return _x; } set { _x = value; ChangeValues(); } }
        public float Y { get { return _y; } set { _y = value; ChangeValues(); } }

        public Vector2() : base(2)
        {
            
        }

        public Vector2(float x, float y) : base(2)
        {
            _x = x;
            _y = y;
            ChangeValues();
        }

        protected override void ValuesUpdated()
        {
            _x = GetValues()[0];
            _y = GetValues()[1];
        }
        
        private void ChangeValues()
        {
            UpdateValues(new float[2]{ _x, _y });
        }
    }
}
