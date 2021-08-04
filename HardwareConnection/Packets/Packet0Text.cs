using System.IO;

namespace HardwareConnection.Packets {
    /// <summary>
    /// A simply demo packet, which contains text. See the base packet class on how it is registered
    /// </summary>
    public class Packet0Text : Packet {
        public int TextColour { get; }
        public string Text { get; }

        public Packet0Text(int textColour, string text) {
            this.TextColour = textColour;
            this.Text = text;
        }

        public override void Write(TextWriter writer) {
            writer.Write(this.Text);
        }

        // e

    }
}