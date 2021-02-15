#pragma once
#include "Exceptions.hpp"
#include "FieldFlags.hpp"
#include "FieldName.hpp"
#include "typetoken.h"
#include "WaveMember.hpp"


class WaveClass;
class WaveType;

class WaveField : WaveMember
{
public:
    WaveField(WaveClass* owner, FieldName* fullName, FieldFlags flags, WaveType* fieldType) : WaveMember(fullName->FullName)
    {
        Owner = owner;
        FullName = fullName;
        Flags = flags;
        Type = fieldType;
    }

    FieldName* FullName;
    WaveType* Type;
    FieldFlags Flags;
    WaveClass* Owner;


    WaveMemberKind GetKind() override
    {
        return Field;
    }

    bool IsLiteral() { return (Flags & Literal) != 0; }
    bool IsStatic() { return (Flags & Static) != 0; }
    bool IsPublic() { return (Flags & Public) != 0; }
    bool IsPrivate() { return !IsPublic(); }

    
    template<class T>
    [[nodiscard]] T* GetLiteral() noexcept(false)
    {
        if (!IsLiteral())
            throw InvalidOperationException("Cannot get literal value from non-literal field.");
        if (literalType == TYPE_I2)
            return static_cast<T*>(static_cast<void*>(&i2));
        if (literalType == TYPE_I4)
            return static_cast<T*>(static_cast<void*>(&i4));
        if (literalType == TYPE_I8)
            return static_cast<T*>(static_cast<void*>(&i8));
        throw NotImplementedException("TODO LITERAL TYPED.");
    }

protected:
    int literalType;

    union {
        int16_t i2;
        int32_t i4;
        int64_t i8;
    };
};
