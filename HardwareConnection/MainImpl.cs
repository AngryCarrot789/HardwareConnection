using System;
using HardwareConnection.Arduino;
using HardwareConnection.Connections;
using HardwareConnection.Packets.Packets;
using HardwareConnection.Serial;

namespace HardwareConnection {
    public class MainImpl {
        public static void Main(string[] args) {
            ArduinoConnection arduino = new ArduinoConnection("COM4");
            arduino.Transceiver.LineReceived += TransceiverOnLineReceived;
            arduino.Open();
            arduino.SendPacket(new Packet1DigitalWrite(13, true));
        }

        private static void TransceiverOnLineReceived(SerialTransceiver transceiver, string line) {
            Console.WriteLine(line);
        }
    }
}