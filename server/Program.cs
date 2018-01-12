using Leeward.Core;
using Leeward.Net;

namespace Leeward
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GameServer server = new GameServer(port: 5127);
            server.Listen();
        }
    }
}