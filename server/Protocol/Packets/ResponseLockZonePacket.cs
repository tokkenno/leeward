using System.IO;

namespace Leeward.Protocol.Packets
{
    /// <summary>
    /// Send zone lock status to player
    /// </summary>
    internal class ResponseLockZonePacket : ResponsePacket
    {
        public readonly bool LockStatus;
        
        public ResponseLockZonePacket(bool lockStatus = true) : base(PacketType.ResponseLockZone)
        {
            this.LockStatus = lockStatus;
        }

        public override string ToString()
        {
            return $"Packet(ResponseLockZone) => LockStatus: {this.LockStatus}"; 
        }

        protected override void Write(BinaryWriter outWriter)
        {
            outWriter.Write(this.LockStatus);
        }
    }
}