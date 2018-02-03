using System;

namespace Leeward.Core
{
    internal class PlayerRejectedException : Exception
    {
        public readonly Player Player;
        
        public PlayerRejectedException(Player player, String message = null) :
            base(message)
        {
            this.Player = player;
        }
    }
}