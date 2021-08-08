using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Packets.Filter {
    /// <summary>
    /// Used for checking if a packet is accepted
    /// </summary>
    public interface PacketFilter {
        /// <summary>
        /// Returns <see langword="true"/> if the given <see cref="Packet"/> is allowed by this <see cref="PacketFilter"/>
        /// </summary>
        /// <param name="packet">The packet to check</param>
        /// <returns></returns>
        bool Accept(Packet packet);
    }
}