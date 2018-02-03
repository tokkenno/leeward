using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Leeward.Image;

namespace Leeward.Configuration
{
    [DataContract]  
    internal class FactionConfiguration
    {
        [DataMember(Name = "capAnywhere")] 
        private bool _capAnywhere = false;
        
        [DataMember(Name = "tp")] 
        private int _tp = 0;
        
        [DataMember(Name = "followers")] 
        private int _followers = 0;
        
        [DataMember(Name = "affinity")] 
        private String _affinity = String.Empty;
        
        [DataMember(Name = "rep")] 
        private int _reputation = 0;
        
        [DataMember(Name = "symbol")] 
        private String _symbol = String.Empty;
        
        [DataMember(Name = "pennant")] 
        private String _pennant = String.Empty;
        
        [DataMember(Name = "color")] 
        private Color _color = default(Color);
        
        [DataMember(Name = "pennant0")] 
        private Color _pennant0 = default(Color);
        
        [DataMember(Name = "pennant1")] 
        private Color _pennant1 = default(Color);
        
        [DataMember(Name = "hull0")] 
        private Color _hull0 = default(Color);
        
        [DataMember(Name = "hull1")] 
        private Color _hull1 = default(Color);
        
        [DataMember(Name = "sail0")] 
        private Color _sail0 = default(Color);
        
        [DataMember(Name = "sail1")] 
        private Color _sail1 = default(Color);
        
        [DataMember(Name = "name0")] 
        private Color _name0 = default(Color);
        
        [DataMember(Name = "name1")] 
        private Color _name1 = default(Color);
        
        [DataMember(Name = "name2")] 
        private Color _name2 = default(Color);
        
        [DataMember(Name = "Skills")] 
        private List<String> _skills = new List<String>();
        
        [DataMember(Name = "Multipliers")] 
        private Dictionary<String, double> _multipliers = new Dictionary<string, double>();
    }
}