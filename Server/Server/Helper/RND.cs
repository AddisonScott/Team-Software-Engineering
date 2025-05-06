using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helper
{
    public class RND
    {
        public static Random Random = new Random();

        public static float RandomFloat()
        {
            int val = Random.Next(0, 101);

            float f = (float)(val / 100.0f);

            return f;
        }

        public static float RandomFloat(float min, float max)
        {
            return RandomFloat() * (max - min) + min;
        }
    }
}
