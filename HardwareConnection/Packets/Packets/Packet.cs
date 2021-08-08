using System;
using System.IO;
using HardwareConnection.Exceptions;

namespace HardwareConnection.Packets.Packets {
    /// <summary>
    /// <para>
    /// A packet is essentially a wrapper for data
    /// </para>
    /// <para>
    /// To write data, see <see cref="Write"/>
    /// </para>
    /// <para>
    /// Reading data is done automatically (see <see cref="RegisterCreator{T}"/> to register a creator)
    /// </para>
    /// </summary>
    public abstract class Packet {
        /// <summary>
        /// An array of packet creators (taking the metadat and packet data (non-null)) and returns a packet instance
        /// <para>Index is the packet ID</para>
        /// </summary>
        private static readonly Func<int, string, Packet>[] PacketCreators;

        static Packet() {
            PacketCreators = new Func<int, string, Packet>[100];
            RegisterCreator(0, (meta, data) => new Packet0Text(meta, data));
            // doesn't need to be registered because it's not a receivable packet, only sendable
            // software tells hardware to digitalWrite, not the other way around ;)
            // RegisterCreator(1, (meta, data) => new Packet1DigitalWrite(meta, data == "t"));
        }

        /// <summary>
        /// The ID of this packet
        /// <para>
        /// Must NOT be below 0, or above 99 (100 possibly IDs)
        /// </para>
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// Extra data for this packet (that is sent after the ID)
        /// <para>
        /// This cannot be below 0 or above 99 (100 possible metadata values)
        /// </para>
        /// </summary>
        public int MetaData { get; }

        /// <summary>
        /// Creates a packet (with the optional metadata)
        /// </summary>
        /// <param name="metaData"></param>
        protected Packet(int metaData = 0) {
            this.MetaData = metaData;
        }

        /// <summary>
        /// Writes the data in this packet to the given <see cref="TextWriter"/>
        /// </summary>
        /// <param name="writer"></param>
        public abstract void Write(TextWriter writer);

        /// <summary>
        /// Registers a packet creator for a specific packet ID, with the given creator
        /// </summary>
        /// <param name="id">The packet ID that the creator creates</param>
        /// <param name="creator">The function that creates the packet</param>
        /// <typeparam name="T">The type of packet to create</typeparam>
        public static void RegisterCreator<T>(int id, Func<int, string, T> creator) where T : Packet {
            PacketCreators[id] = creator;
        }

        /// <summary>
        /// Creates a packet from the given string (which contains a serialised packet)
        /// </summary>
        /// <param name="packetData">The full packet data (containing the ID, Meta and (optionally) custom packet data)</param>
        /// <returns>A packet instance (non-null)</returns>
        /// <exception cref="NullReferenceException">If the given packet data is null</exception>
        /// <exception cref="InvalidDataException">If the packet header data was corrupted (id or meta)</exception>
        /// <exception cref="MissingPacketCreatorException">If the packet creator for the specific Id was missing (aka null)</exception>
        /// <exception cref="PacketCreationException">Thrown if the packet couldn't be created (unknown metadata or corrupted packet data maybe)</exception>
        public static Packet CreatePacket(string packetData) {
            if (packetData == null)
                throw new NullReferenceException("The data cannot be null!");
            if (packetData.Length < 4)
                throw new InvalidDataException($"The data wasn't long enough, It must be 5 or above (it was {packetData.Length})");

            string stringId = packetData.Substring(0, 2);
            if (int.TryParse(stringId, out int id)) {
                string stringMeta = packetData.Substring(2, 2);
                if (int.TryParse(stringMeta, out int meta)) {
                    Func<int, string, Packet> creator = PacketCreators[id];
                    if (creator == null) {
                        throw new MissingPacketCreatorException(id);
                    }

                    return creator(meta, packetData.Substring(4));
                }

                throw new InvalidDataException($"The value ({stringMeta}) was not parsable as the packet's MetaData");
            }

            throw new InvalidDataException($"The value ({stringId}) was not parsable as the packet's ID");
        }

        /// <summary>
        /// Writes the given packet's ID, Meta to the given writer, and then calls the given packet's <see cref="Packet.Write(TextWriter)"/> method
        /// </summary>
        /// <param name="writer">The text writer to write the packet data to</param>
        /// <param name="packet">The packet that is to be written</param>
        /// <exception cref="InvalidDataException">If the packet's ID or Meta was invalid (below 0 or above 99)</exception>
        public static void WritePacket(TextWriter writer, Packet packet) {
            int id = packet.ID;
            if (id < 0 || id > 99) {
                throw new InvalidDataException("ID was not between 0 and 99! " + id);
            }

            int meta = packet.MetaData;
            if (meta < 0 || meta > 99) {
                throw new InvalidDataException("MetaData was not between 0 and 99! " + id);
            }

            if (id < 10) {
                writer.Write("0");
                writer.Write(id);
            }
            else {
                writer.Write(id);
            }

            if (meta < 10) {
                writer.Write("0");
                writer.Write(meta);
            }
            else {
                writer.Write(meta);
            }

            packet.Write(writer);
        }
    }
}