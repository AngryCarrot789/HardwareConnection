using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HardwareConnection.Packets.Listeners;
using HardwareConnection.Packets.Packets;
using HardwareConnection.Serial;

namespace HardwareConnection.Packets {
    /// <summary>
    /// Defines behaviour for sending packets, and registering listeners (<see cref="PacketListener"/>) to listen to when packets are received
    /// </summary>
    public class SerialConnection : ISerialConnection {
        /// <summary>
        /// A list of all the <see cref="PacketListener"/>s that should be notified when a new <see cref="Packet"/> arrives
        /// </summary>
        public List<PacketListener> Listeners { get; }

        /// <summary>
        /// The transceiver used for sending/receiving data
        /// </summary>
        public SerialTransceiver Transceiver { get; }

        public PacketSpooler Spooler { get; }

        private readonly StringWriter PacketWriteBuffer;

        protected SerialConnection(SerialTransceiver transceiver) {
            this.Listeners = new List<PacketListener>(16);
            this.PacketWriteBuffer = new StringWriter(new StringBuilder(128));
            this.Spooler = new PacketSpooler(this);
            this.Transceiver = transceiver;
            this.Transceiver.LineReceived += TransceiverOnLineReceived;
        }

        private void TransceiverOnLineReceived(SerialTransceiver transceiver, string line) {
            if (string.IsNullOrEmpty(line)) {
                Console.WriteLine($"Received a null or empty string! On port '{transceiver.Port.PortName}'");
                return;
            }

            if (line[0] == '#') {
                Packet packet;
                try {
                    packet = Packet.CreatePacket(line.Substring(1));
                }
                catch (Exception e) {
                    Console.WriteLine($"Failed to create a packet: {e.Message}");
                    return;
                }

                foreach (PacketListener listener in this.Listeners) {
                    listener.TryReceivePacket(packet);
                }
            }
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
        /// <param name="packet">The packet to immediately send</param>
        public void SendPacket(Packet packet) {
            PacketWriteBuffer.Write('#');
            Packet.WritePacket(PacketWriteBuffer, packet);
            this.Transceiver.WriteLine(PacketWriteBuffer.ToString());
            this.PacketWriteBuffer.GetStringBuilder().Clear();
        }

        /// <summary>
        /// Queues a packet to be sent through the network
        /// </summary>
        /// <param name="packet">The pack to eventually send</param>
        public void QueuePacket(Packet packet) {
            this.Spooler.QueuePacket(packet);
        }

        public void Open() {
            this.Transceiver.Open();
        }

        public void Close() {
            this.Transceiver.Close();
        }
    }
}