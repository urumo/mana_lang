#pragma once
#include "compatibility.h"
#include "api/EEPROM.h"
#define DEBUG 1

#ifdef DEBUG
#define d_print(x) Serial.print(x)
#define f_print(x) do {  Serial.print(#x);Serial.print(" ");Serial.println(x); } while(0)
#define w_print(x) Serial.println(x)
#define d_init() Serial.begin(9600)

#ifndef ARDUINO
#include <iostream>
#undef d_print
#undef f_print
#undef w_print
#undef d_init
#define d_init()
#define d_print(x) std::cout << x
#define f_print(x) std::cout << #x << " " << x << "\n"
#define w_print(x) std::cout << x << "\n"
#endif

#else
#define d_print(x)
#define f_print(x)
#define w_print(x)
#define d_init()
#endif



class Shifter 
{
    public:
        Shifter(int start) 
        {
            index = start;
        }
        int next()
        {
            prev = index;
            index -= 4;
            if (index < 0) index = 0;
            return prev;
        }
    private:
        int prev;
        int index;
};

String ulonglongToStr(int64_t l, int base)
{
    char buff[67]; // length of MAX_ULLONG in base 2
    buff[66] = 0;
    char *p = buff + 66;
    const char _zero = '0';

    if (base != 10) {
        while (l != 0) {
            int c = l % base;

            --p;

            if (c < 10)
                *p = '0' + c;
            else
                *p = c - 10 + 'a';

            l /= base;
        }
    }
    else {
        while (l != 0) {
            int c = l % base;

            *(--p) = _zero + c;

            l /= base;
        }
    }

    return p;
}

String longlongtoStr(int64_t l, int base)
{
   auto res = ulonglongToStr(l<0 ? -l: l, base);
   if (l < 0)
     res = "-" + res;
   return res;
}

#define bump_t(x, y) Serial.print(y); Serial.println(x, HEX);
#define alloc_control_operator(x) auto _shift = new Shifter(x)




#if defined(ARDUINO)
__uint16_t deconstruct(int64_t addr)
{
    alloc_control_operator(32);
    auto x1 = (__uint16_t)((addr & 0xF) >> 0); bump_t(x1, "x1 ");
    auto x2 = (__uint16_t)((addr & 0xF0) >> 4); bump_t(x2, "x2 ");
    auto x3 = (__uint16_t)((addr & 0xF00) >> 8); bump_t(x3, "x3 ");
    auto x4 = (__uint16_t)((addr & 0xF000) >> 12); bump_t(x4, "x4 ");

    bump_t((__uint16_t)((x1 << 4) | x2), "result");
    return (__uint16_t)((x2 << 4) | x1);
}
#endif



/*
 * Makes a token based on a table and an index
 */
#define wave_metadata_make_token(table,idx) (((table) << 24)| idx)

/*
 * Returns the table index that this token encodes.
 */
#define wave_metadata_token_table(token) ((token) >> 24)

/*
 * Returns the index that a token refers to
 */
#define wave_metadata_token_index(token) ((token & 0xffffff))


#define wave_metadata_token_code(token) ((token & 0xff000000))




