using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menelia.Client.AdminMenu
{
    public class Waypoints
    {
        public static Waypoints[] waypoints = {
            new Waypoints("Airport Los Santos", -1042, -2744, 21, 327),
            new Waypoints("Arena", -282, -1918, 30, 322),
            new Waypoints("Police Station 1", 427, -980, 30, 85),
            new Waypoints("Casino", 926, 44, 80, 57),
            new Waypoints("Prison", 1856, 2608, 45, 269),
            new Waypoints("Airport Sandy Shores", 1738, 3281, 40, 193),
            new Waypoints("Mountain", 502.59f, 5603.7f, 797.91f, 78),
            new Waypoints("Military Base", -2052, 3154, 32, 227)
        };
        public String Name;
        public float X;
        public float Y;
        public float Z;
        public float Heading;
        public Waypoints(String Name, float X, float Y, float Z, float Heading)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Heading = Heading;
        }

        
    }
}
