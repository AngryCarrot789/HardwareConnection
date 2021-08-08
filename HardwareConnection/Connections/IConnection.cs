using HardwareConnection.Packets;
using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Serial {
    public interface IConnection {
        void SendPacket(Packet packet);
    }
}