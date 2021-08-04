using System;

namespace HardwareConnection.Exceptions {
    public class PacketCreationException : Exception {
        public PacketCreationException(string message) : base(message) {
        }
    }
}