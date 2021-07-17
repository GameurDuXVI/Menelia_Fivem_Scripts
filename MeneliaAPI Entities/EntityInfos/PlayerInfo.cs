using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Menelia.Entities
{
    public class PlayerInfo : EntityInfo
    {
        public static List<PlayerInfo> playerInfos = new List<PlayerInfo>();

        public int Id;
        public List<string> Identifiers;
        public Position Position;
        public Banking Banking;
        public string FirstName;
        public string LastName;
        public int Cash;

        public PlayerInfo(List<string> identifiers, string name, float x, float y, float z, float heading)
        {
            var tempIdentiefiers = new List<string>();
            foreach (var identifier in identifiers)
                if (!identifier.Contains("ip:"))
                    tempIdentiefiers.Add(identifier);

            Id = nextId();
            Identifiers = tempIdentiefiers;
            Name = name;
            Position = new Position(x, y, z, heading);
            Banking = new Banking();
            Cash = 0;
        }

        public static int nextId()
        {
            for(int i = 0; ; i++)
                if (playerInfos.ElementAtOrDefault(i) == null)
                    return i;
        }

        public static PlayerInfo getPlayerInfo(int Id)
        {
            return playerInfos[Id];
        }

        public String toJson()
        {
            return JsonConvert.SerializeObject(this); 
        }

        public static PlayerInfo fromJson(String json)
        {
            return JsonConvert.DeserializeObject<PlayerInfo>(json);
        }

        public static String listToJson()
        {
            return JsonConvert.SerializeObject(playerInfos);
        }

        public static List<PlayerInfo> listFromJson(String json)
        {
            return JsonConvert.DeserializeObject<List<PlayerInfo>>(json);
        }
    }
}

