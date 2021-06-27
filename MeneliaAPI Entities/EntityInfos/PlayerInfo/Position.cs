using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeneliaAPI.Entities
{
    public class Position
    {
        public float X;
        public float Y;
        public float Z;
        public float Heading;

        public Position(float X, float Y, float Z, float Heading)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Heading = Heading;
        }
    }
}
