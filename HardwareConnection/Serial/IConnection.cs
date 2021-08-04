using HardwareConnection.Packets;

namespace HardwareConnection.Serial {
    public interface IConnection {
        void SendPacket(Packet packet);
    }
}