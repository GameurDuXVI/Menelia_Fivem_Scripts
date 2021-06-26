using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeneliaAPI.Entities
{
    public class PlayerInfo
    {
        public List<String> identifiers;
        public String name;
        public float x;
        public float y;
        public float z;
        public float heading;

        public PlayerInfo(List<String> identifiers, String name, float x, float y, float z, float heading)
        {
            this.identifiers = identifiers;
            this.name = name;
            this.x = x;
            this.y = y;
            this.z = z;
            this.heading = heading;
        }        
    }
}

