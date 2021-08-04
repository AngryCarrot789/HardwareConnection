using System;
using System.IO;
using HardwareConnection.Exceptions;

namespace HardwareConnection.Packets {
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
            PacketCreators = new Func<int, string, Packet>[1000];
            RegisterCreator(0, (meta, data) => new Packet0Text(meta, data));
        }

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
            this.MetaData = 0;
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
        /// <param name="fullData">The full packet data (containing the ID, Meta and custom packet data)</param>
        /// <returns>A packet instance (non-null)</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidDataException">If the packet header data was corrupted (id or meta)</exception>
        /// <exception cref="MissingPacketCreatorException">If the packet creator for the specific Id was missing (aka null)</exception>
        /// <exception cref="PacketCreationException">Thrown if the packet couldn't be created (unknown metadata or corrupted packet data maybe)</exception>
        public static Packet CreatePacket(string fullData) {
            if (fullData == null)
                throw new NullReferenceException("The data cannot be null!");
            if (fullData.Length < 5)
                throw new InvalidDataException($"The data wasn't long enough, It must be above 5 (it was {fullData.Length} long)");

            string stringId = fullData.Substring(0, 3);
            if (int.TryParse(stringId, out int id)) {
                string stringMeta = fullData.Substring(3, 2);
                if (int.TryParse(stringMeta, out int meta)) {
                    Func<int, string, Packet> creator = PacketCreators[id];
                    if (creator == null) {
                        throw new MissingPacketCreatorException(id);
                    }

                    return creator(meta, fullData.Substring(5));
                }

                throw new InvalidDataException($"The value ({stringMeta}) was not parsable as the packet's MetaData");
            }

            throw new InvalidDataException($"The value ({stringId}) was not parsable as the packet's ID");
        }
    }
}