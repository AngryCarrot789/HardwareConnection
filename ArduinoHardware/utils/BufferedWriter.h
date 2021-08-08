#ifndef __DEF_BUFFERED_WRITER
#define __DEF_BUFFERED_WRITER

#include "StrUtil.h"

class BufferedWriter {
private:
    char* buffer;
    int size;
    int index;

public:
    BufferedWriter(int max_size) {
        this->index = 0;
        this->size = max_size;
        this->buffer = new char[max_size];
        for (int i = 0; i < max_size; i++) {
            this->buffer[i] = 0;
        }
    }

    BufferedWriter(char* buffer, int size, int startOffset = 0) {
        this->buffer = buffer;
        this->size = size;
        this->index = startOffset;
    }

    void appendChar(const char c) {
        if (!canWrite(1)) {
            return;
            //throw;
        }

        this->buffer[this->index++] = c;
    }

    void appendString(char* chars, const int count) {
        if (!canWrite(count)) {
            return;
            //throw;
        }

        int end = this->index + count;
        for (int i = this->index, j = 0; i < end; i++, j++) {
            this->buffer[i] = chars[j];
        }

        this->index = end;
    }

    void appendString(const char* chars, const int count) {
        if (!canWrite(count)) {
            return;
            //throw;
        }

        int end = this->index + count;
        for (int i = this->index, j = 0; i < end; i++, j++) {
            this->buffer[i] = chars[j];
        }

        this->index = end;
    }

    void appendInt(int i) {
        char* str = new char[10];
        String toStr = String(itostr(i, str));
        if (!canWrite(toStr.length())) {
            return;
            //throw;
        }

        appendString(toStr.c_str(), toStr.length());
    }

    void appendBool(bool value) {
        if (value) {
            appendChar('t');
        }
        else {
            appendChar('f');
        }
    }

    void clear() {
        this->index = 0;
    }

    bool canWrite(int extraBytes) {
        return (this->index + extraBytes) <= this->size;
    }

    // the max number of chars that can be appended to this buffer
    int totalBytes() {
        return this->size;
    }

    // the number of chars appended to this buffer
    int bytesWritten() {
        return this->index;
    }

    String toString() {
        return String(this->buffer);
    }

    char* c_str() {
        this->buffer[this->index] = '\0';
        return this->buffer;
    }

    void dispose() {
        delete[](this->buffer);
    }
};

#endif