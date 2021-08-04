using System.IO;

namespace HardwareConnection.Packets {
    public class Packet1DigitalWrite : Packet {
        public override int ID => 1;

        public int Pin {
            get => this.MetaData;
            set => this.MetaData = value;
        }

        public bool State { get; }

        public Packet1DigitalWrite(int pin, bool state) {
            this.Pin = pin;
            this.State = state;
        }

        public override void Write(TextWriter writer) {
            writer.Write(this.State ? "t" : "f");
        }
    }
}