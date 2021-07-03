﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                if (playerInfos.ElementAtOrDefault(i) == null)
                    return i;
        }

        public static PlayerInfo GetPlayerInfo(int Id)
        {
            return playerInfos[Id];
        }

        public String ToJson()
        {
            return JsonConvert.SerializeObject(this); 
        }

        public static PlayerInfo FromJson(String json)
        {
            return JsonConvert.DeserializeObject<PlayerInfo>(json);
        }

        public static String ListToJson()
        {
            return JsonConvert.SerializeObject(playerInfos);
        }

        public static List<PlayerInfo> ListFromJson(String json)
        {
            return JsonConvert.DeserializeObject<List<PlayerInfo>>(json);
        }
    }
}

