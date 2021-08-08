using System;
using HardwareConnection.Exceptions;
using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Packets.Filter {
    public class PacketPredicateFilter : PacketFilter {
        public Predicate<Packet> Predicate { get; }

        public PacketPredicateFilter(Predicate<Packet> predicate) {
            this.Predicate = predicate;
        }

        public bool Accept(Packet packet) {
            return Predicate(packet);
        }
    }
}