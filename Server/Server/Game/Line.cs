using Server.Helper.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class Line
    {
        public List<Vector2> Points;
        public int CurrentSegment;

        public Line()
        {
            Points = new List<Vector2>();
            CurrentSegment = 0;
        }

        public Line(List<Vector2> points)
        {
            Points = points;
            CurrentSegment = 0;
        }

        public Vector2 GetNextSegmentPoint()
        {
            if(CurrentSegment >= 0 && CurrentSegment < Points.Count)
            {
                CurrentSegment++;
                return Points[CurrentSegment--];
            }

            return null;
        }
    }
}
