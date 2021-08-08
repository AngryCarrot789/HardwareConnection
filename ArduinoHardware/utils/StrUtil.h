#ifndef __DEF_STRUTILS
#define __DEF_STRUTILS

static char itoch(int value) {
    if (value < 0 || value > 9) {
        return '0';
    }

    return (char)(value + 48);
}

static char* itostr(int value, char* str) {
    int i = (int)log10((double)value);
    while (value > 0) {
        str[i] = (value % 10) + '0';
        value = value / 10;
        i = i - 1;
    }
    str[i] = '\0';
}

#endif // !__DEF_STRUTILS