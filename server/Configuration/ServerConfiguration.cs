using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Leeward.Configuration
{
    [DataContract]  
    internal class ServerConfiguration : Configuration
    {
        private const string Filename = "server";
        
        [DataMember(Name = "admins")] 
        public readonly List<string> Admins = new List<string>();

        public override void Save() => Configuration.Save<ServerConfiguration>(this);
        public static ServerConfiguration Load() => Configuration.LoadOrDefault<ServerConfiguration>(Filename);
    }
}