
namespace Menelia.Client.AdminMenu
{
    public class Waypoint
    {
        public static Waypoint[] Waypoints = {
            new Waypoint("Airport Los Santos", -1042, -2744, 21, 327),
            new Waypoint("Arena", -282, -1918, 30, 322),
            new Waypoint("Police Station 1", 427, -980, 30, 85),
            new Waypoint("Casino", 926, 44, 80, 57),
            new Waypoint("Prison", 1856, 2608, 45, 269),
            new Waypoint("Airport Sandy Shores", 1738, 3281, 40, 193),
            new Waypoint("Mountain", 502.59f, 5603.7f, 797.91f, 78),
            new Waypoint("Military Base", -2052, 3154, 32, 227)
        };
        public string Name;
        public float X;
        public float Y;
        public float Z;
        public float Heading;
        private Waypoint(string name, float x, float y, float z, float heading)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
        }

        
    }
}
