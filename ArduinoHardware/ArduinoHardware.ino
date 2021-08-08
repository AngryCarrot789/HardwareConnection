#include "utils/BufferedReader.h"
#include "utils/BufferedWriter.h"
#include "packets/Packet.h"
#include "packets/PacketRegistry.h"
BufferedWriter* serial_in;
BufferedReader* reader;

void setup() {
	serial_in = new BufferedWriter(128);
	reader = new BufferedReader(serial_in->c_str(), 128, 0);
	Serial.begin(9600);
}

void processPacket() {
	Packet* packet = PacketRegistry::createPacket(reader);
	String msg;
	if (packet == nullptr) {
		msg = String("Failed to parse packet: ");
		msg += serial_in->c_str();
	}
	else if (packet->getId() == 1) {
		Packet1DigitalWrite* pktdw = (Packet1DigitalWrite*)packet;
		msg = String("ID = ") + String(pktdw->getId()) + String(", Meta: ") + String(pktdw->getMetaData()) + String(", Pin: ") + String(pktdw->getPin());
		//msg = "xd digitalwrite = gud";
	}

	Serial.println(msg);
}

void processSerialBuffer(void) {
	processPacket();
	serial_in->clear();
	reader->clear();
}

void loop() {
	int available_bytes = Serial.available();
	if (available_bytes > 0) {
		while(available_bytes > 0) {
			char c = (char) Serial.read();
			if (c == '\r') {
				continue;
			}
			if (c == '\n') {
				processSerialBuffer();
			}
			else {
				serial_in->appendChar(c);
			}

			available_bytes--;
		}
	}
}
