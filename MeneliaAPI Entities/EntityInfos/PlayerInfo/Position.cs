
namespace Menelia.Entities
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
