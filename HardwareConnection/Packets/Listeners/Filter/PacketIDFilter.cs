using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Packets.Listeners.Filter {
    public class PacketIDFilter : PacketFilter {
        public int AcceptedID { get; }

        public PacketIDFilter(int acceptedId) {
            this.AcceptedID = acceptedId;
        }

        public bool Accept(Packet packet) {
            return packet.ID == this.AcceptedID;
        }
    }
}