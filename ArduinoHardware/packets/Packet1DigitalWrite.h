#ifndef __DEF_PACKET1DIGITALWRITE
#define __DEF_PACKET1DIGITALWRITE

#include "Packet.h"

class Packet1DigitalWrite : public Packet {
private:
    bool state;
    int pin;

public:
    Packet1DigitalWrite(int pin, bool state) : Packet(pin) {
        this->pin = pin;
        this->state = state;
    }

    void write(BufferedWriter* buffer) override {
        buffer->appendChar(this->state ? 't' : 'f');
    }

    int getId() override {
        return 1;
    }

    bool getState() {
        return this->state;
    }

    int getPin() {
        return this->pin;
    }
};

#endif