#ifndef __DEF_PACKET
#define __DEF_PACKET

#include "../utils/BufferedWriter.h"
class Packet {
private:
    int metadata;
public:
    Packet() {
        this->metadata = 0;
    }

    Packet(int meta) {
        this->metadata = meta;
    }

    // writes this packet's data to the given char buffer
    virtual void write(BufferedWriter* buffer) { return; }
    virtual int getId() { return -1; }

    int getMetaData() {
        return this->metadata;
    }
};

#endif