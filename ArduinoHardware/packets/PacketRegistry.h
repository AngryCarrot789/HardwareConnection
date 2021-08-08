#ifndef __DEF_PACKET_REGISTRY
#define __DEF_PACKET_REGISTRY

#include "Packet.h"
#include "Packet1DigitalWrite.h"
#include "../utils/BufferedReader.h"

typedef Packet* (*pkt_creator)(int, BufferedReader*);

class PacketRegistry {
private:
    static pkt_creator packetCreators[100];
public:
    static Packet* createPacket(BufferedReader* buffer) {
        if (buffer->bytesLeft() < 4) {
            return nullptr;
            //throw;
        }

        char* idbuff = new char[2];
        int id = atoi(buffer->readBuffer(idbuff, 2, 0));
        if (id < 0 || id > 99) {
            return nullptr;
            //throw;
        }

        pkt_creator creator = packetCreators[id];
        if ((*creator) == nullptr) {
            return nullptr;
            //throw;
        }

        char* metabuff = new char[2];
        int meta = atoi(buffer->readBuffer(metabuff, 2, 0));
        if (meta < 0 || meta > 99) {
            return nullptr;
            //throw;
        }

        return (*creator)(meta, buffer);
    }
};

static Packet* createDigitalWritePacket(int meta, BufferedReader* reader) {
    return new Packet1DigitalWrite(meta, reader->readChar() == 't');
}

pkt_creator PacketRegistry::packetCreators[100] = {
    nullptr,
    createDigitalWritePacket,
    nullptr,
    nullptr,
    nullptr,
    nullptr,
    nullptr,
    nullptr,
};

#endif