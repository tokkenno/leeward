using System;

namespace Leeward.Protocol
{
    public class UnrecognizedPacketException : Exception
    {
        public UnrecognizedPacketException(uint code, long length) :
            base("Unrecognized package recepted. Command: (" + code + ") " + BitConverter.ToString(BitConverter.GetBytes(code)) +
                 ". Length: " + length + ".")
        {
        }
    }
}