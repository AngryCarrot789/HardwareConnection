using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Connections {
    /// <summary>
    /// A connection that handles sending packet
    /// </summary>
    public interface IConnection {
        void SendPacket(Packet packet);
    }
}