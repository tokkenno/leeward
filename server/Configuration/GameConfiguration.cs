using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Leeward.Configuration
{
    [DataContract]  
    internal class GameConfiguration : Configuration
    {
        private const string Filename = "game";
        
        [DataMember(Name = "symbols")] 
        private List<string> _symbols = new List<string>();

        [DataMember(Name = "factions")] 
        private Dictionary<String, FactionConfiguration> _factions = new Dictionary<string, FactionConfiguration>();

        [DataMember(Name = "war")] 
        private bool _war = false;

        [DataMember(Name = "playerShips")] 
        private bool _playerShips = true;

        [DataMember(Name = "challenge")] 
        private double _challenge = 0.5;
        
        [DataMember(Name = "damage")] 
        private double _damage = 0.5;

        [DataMember(Name = "capStats")] 
        private bool _capStats = true;
        
        public override void Save() => Configuration.Save<GameConfiguration>(this);
        public static GameConfiguration Load() => Configuration.LoadOrDefault<GameConfiguration>(Filename);
    }
}