using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using HardwareConnection.Connections;
using HardwareConnection.Packets.Listeners;
using HardwareConnection.Packets.Packets;
using HardwareConnection.Serial;

namespace HardwareConnection.Packets {
    /// <summary>
    /// Defines behaviour for sending packets, and registering listeners (<see cref="PacketListener"/>) to listen to when packets are received
    /// </summary>
    public class SerialPacketTransporter {
        /// <summary>
        /// A list of all the <see cref="PacketListener"/>s that should be notified when a new <see cref="Packet"/> arrives
        /// </summary>
        public List<PacketListener> Listeners { get; }

        /// <summary>
        /// The transceiver used for
        /// </summary>
        public SerialTransceiver Transceiver { get; }

        private readonly StringWriter PacketWriteBuffer;

        protected SerialPacketTransporter(SerialTransceiver transceiver) {
            this.Transceiver = transceiver;
            this.Listeners = new List<PacketListener>(16);
            this.PacketWriteBuffer = new StringWriter(new StringBuilder(128));
        }

        /// <summary>
        /// Adds a listener to the list of listeners that are notified whenever a new packet is received
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(PacketListener listener) {
            this.Listeners.Add(listener);
        }

        /// <summary>
        /// Removes a listener from the listeners list. The given listener wont be notified when packets are received anymore
        /// </summary>
        /// <param name="listener"></param>
        public void UnregisterListener(PacketListener listener) {
            this.Listeners.Remove(listener);
        }

        /// <summary>
        /// Sends a packet through the network
        /// </summary>
        /// <param name="packet"></param>
        /// <returns>
        /// <see langword="true"/> the packet was successfully send, or <see langword="false"/> if the packet failed to send.
        /// </returns>
        public void SendPacket(Packet packet) {
            Packet.WritePacket(PacketWriteBuffer, packet);
            this.Transceiver.WriteLine(PacketWriteBuffer.ToString());
            this.PacketWriteBuffer.GetStringBuilder().Clear();
        }
    }
}