using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeneliaAPI.Entities
{
    public class PlayerInfo : EntityInfo
    {
        public static List<PlayerInfo> playerInfos = new List<PlayerInfo>();

        public int Id;
        public List<String> Identifiers;
        public Position Position;
        public Banking Banking;

        public PlayerInfo(List<String> Identifiers, String Name, float X, float Y, float Z, float Heading)
        {
            this.Id = NextId();
            this.Identifiers = Identifiers;
            this.Name = Name;
            this.Position = new Position(X, Y, Z, Heading);
            this.Banking = new Banking();
        }

        public static int NextId()
        {
            for(int i = 0; ; i++)
                if (playerInfos[i] == null)
                    return i;
        }

        public static PlayerInfo GetPlayerInfo(int Id)
        {
            return playerInfos[Id];
        }
    }
}

