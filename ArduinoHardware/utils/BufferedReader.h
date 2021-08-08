#ifndef __DEF_BUFFERED_READER
#define __DEF_BUFFERED_READER

class BufferedReader {
private:
    char* buffer;
    int size;
    int index;
    char pop() {
        return this->buffer[this->index++];
    }

public:
    BufferedReader(char* buffer, int size, int startOffset = 0) {
        this->buffer = buffer;
        this->size = size;
        this->index = startOffset;
    }

    void putBuffer(char* buffer, int size, int startOffset = 0) {
        delete[](this->buffer);
        this->buffer = buffer;
        this->size = size;
        this->index = startOffset;
    }

    void clear() {
        this->index = 0;
    }

    int totalBytes() {
        return this->size;
    }

    int bytesRead() {
        return this->index;
    }

    int bytesLeft() {
        return totalBytes() - bytesRead();
    }

    char readChar() {
        if (!canRead(1)) {
            return 0;
            //throw;
        }

        return pop();
    }

    short readInt16() {
        if (!canRead(2)) {
            return 0;
            //throw;
        }

        return (short)(((int)pop()) | ((int)pop() << 8));
    }

    int readInt32() {
        if (!canRead(4)) {
            return 0;
            //throw;
        }

        return (int)(((int)pop()) | ((int)pop() << 8) | ((int)pop() << 16) | ((int)pop() << 24));
    }

    float readFloat() {
        if (!canRead(4)) {
            return 0;
            //throw;
        }

        return *(float*)((unsigned int)(((int)pop()) | ((int)pop() << 8) | ((int)pop() << 16) | ((int)pop() << 24)));
    }

    String readString(int len) {
        if (!canRead(len)) {
            return "";
            //throw;
        }

        String str;
        for (int i = this->index, end = this->index + len; i < end; i++) {
            str += pop();
        }
        return str;
    }

    // Reads the given number of characters from this reader and puts them into the given buffer
    // bufferStartIndex specifies the start index to start writing into the given buffer (not the reader's buffer)
    char* readBuffer(char* buffer, int len, int bufferStartIndex = 0) {
        if (!canRead(len)) {
            return buffer;
            //throw;
        }

        for (int i = this->index, end = this->index + len, j = bufferStartIndex; i < end; i++, j++) {
            buffer[j] = pop();
        }

        return buffer;
    }

    bool canRead(int bytesToRead) {
        return (this->index + bytesToRead) <= this->size;
    }

    void dispose() {
        delete[](this->buffer);
    }
};

#endif