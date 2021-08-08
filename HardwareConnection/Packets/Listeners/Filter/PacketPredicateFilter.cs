using System;
using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Packets.Listeners.Filter {
    public class PacketPredicateFilter : PacketFilter {
        public Predicate<Packet> Predicate { get; }

        public PacketPredicateFilter(Predicate<Packet> predicate) {
            if (predicate == null) {
                throw new NullReferenceException("Predicate cannot be null");
            }

            this.Predicate = predicate;
        }

        public bool Accept(Packet packet) {
            return Predicate(packet);
        }
    }
}