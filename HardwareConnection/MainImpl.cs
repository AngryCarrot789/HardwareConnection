using System;
using HardwareConnection.Arduino;
using HardwareConnection.Packets;
using HardwareConnection.Serial;

namespace HardwareConnection {
    public class MainImpl {
        public static void Main(string[] args) {
            ArduinoConnection arduino = new ArduinoConnection("COM21");
            arduino.OnLineReceived += (connection, line) => {
                Console.WriteLine("okxd");
            };

            arduino.Open();
            arduino.SendPacket(new Packet0Text(1, "hello there dx"));
        }
    }
}