using System.IO;

namespace HardwareConnection.Packets.Packets {
    public class Packet1DigitalWrite : Packet {
        public override int ID => 1;

        public bool State { get; }

        public Packet1DigitalWrite(int pin, bool state) : base(pin) {
            this.State = state;
        }

        public override void Write(TextWriter writer) {
            writer.Write(this.State ? "t" : "f");
        }
    }
}