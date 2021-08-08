using System;
using HardwareConnection.Packets.Listeners.Filter;
using HardwareConnection.Packets.Packets;

namespace HardwareConnection.Packets.Listeners {
    /// <summary>
    /// A class which holds a reference to a callback method that should be called
    /// if a packet received is accepted by the contained <see cref="PacketFilter"/>
    /// <para>
    /// By default, the <see cref="TryReceivePacket(Packet)"/> function returns <see langword="false"/>,
    /// so in the eyes of the <see cref="SerialConnection"/>, packets are never handled
    /// </para>
    /// </summary>
    public class PacketListener {
        /// <summary>
        /// The filter that will be used to see if this <see cref="PacketListener"/> is allowed to be notified of
        /// a <see cref="Packet"/> received by the <see cref="SerialConnection"/>
        /// </summary>
        public PacketFilter Filter { get; }

        /// <summary>
        /// The callback function that is called if the <see cref="PacketFilter"/> allows the <see cref="Packet"/>
        /// </summary>
        public Action<Packet> OnPackedReceived { get; }

        public PacketListener(PacketFilter filter, Action<Packet> onPacketReceived) {
            if (filter == null) {
                throw new NullReferenceException("Filter cannot be null");
            }

            if (onPacketReceived == null) {
                throw new NullReferenceException("Packet received callback cannot be null");
            }

            this.Filter = filter;
            this.OnPackedReceived = onPacketReceived;
        }

        /// <summary>
        /// Checks if the contained <see cref="PacketFilter"/> will accept a <see cref="Packet"/>,
        /// and if so it calls the callback function (<see cref="OnPackedReceived"/>).
        /// </summary>
        /// <param name="packet">The packet to try and send to this listener</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Packet"/> is fully handled and shouldn't be processed/sent to any other <see cref="PacketListener"/>.
        /// <see langword="false"/> if the same <see cref="Packet"/> should be sent to other <see cref="PacketListener"/>s (by the <see cref="SerialConnection"/>)
        /// <returns>True if the packet was accepted by the filter</returns>
        public virtual bool TryReceivePacket(Packet packet) {
            if (this.Filter.Accept(packet)) {
                this.OnPackedReceived.Invoke(packet);
                return true;
            }

            return false;
        }
    }
}