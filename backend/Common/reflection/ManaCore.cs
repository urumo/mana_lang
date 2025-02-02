namespace mana.runtime
{
    using System;
    using System.Collections.Generic;

    public static class ManaCore
    {
        public static ManaClass ObjectClass;
        public static ManaClass ValueTypeClass;
        public static ManaClass VoidClass;
        public static ManaClass StringClass;
        public static ManaClass ByteClass;
        public static ManaClass SByteClass;
        public static ManaClass Int32Class;
        public static ManaClass Int16Class;
        public static ManaClass Int64Class;
        public static ManaClass UInt32Class;
        public static ManaClass UInt16Class;
        public static ManaClass UInt64Class;
        public static ManaClass HalfClass;
        public static ManaClass FloatClass;
        public static ManaClass DoubleClass;
        public static ManaClass DecimalClass;
        public static ManaClass BoolClass;
        public static ManaClass CharClass;
        public static ManaClass ArrayClass;
        public static ManaClass ExceptionClass;

        public static List<ManaClass> All => new()
        {
            ObjectClass,
            ValueTypeClass,
            VoidClass,
            StringClass,
            ByteClass,
            SByteClass,
            Int32Class,
            Int64Class,
            Int16Class,
            UInt32Class,
            UInt64Class,
            UInt16Class,
            HalfClass,
            FloatClass,
            DoubleClass,
            DecimalClass,
            BoolClass,
            CharClass,
            ArrayClass,
            ExceptionClass
        };

        public static void Init()
        {
            var asmName = "corlib%";
            var cormodule = new ManaModule("corlib", new Version(1, 0, 0));
            ObjectClass = new ManaClass($"{asmName}global::mana/lang/Object", null, cormodule) { TypeCode = ManaTypeCode.TYPE_OBJECT };
            ValueTypeClass = new ManaClass($"{asmName}global::mana/lang/ValueType", null, cormodule) { TypeCode = ManaTypeCode.TYPE_OBJECT };
            VoidClass = new ManaClass($"{asmName}global::mana/lang/Void", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_VOID };
            StringClass = new ManaClass($"{asmName}global::mana/lang/String", ObjectClass, cormodule) { TypeCode = ManaTypeCode.TYPE_STRING };
            ByteClass = new ManaClass($"{asmName}global::mana/lang/Byte", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_U1 };
            SByteClass = new ManaClass($"{asmName}global::mana/lang/SByte", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_I1 };
            Int16Class = new ManaClass($"{asmName}global::mana/lang/Int16", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_I2 };
            Int32Class = new ManaClass($"{asmName}global::mana/lang/Int32", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_I4 };
            Int64Class = new ManaClass($"{asmName}global::mana/lang/Int64", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_I8 };
            UInt16Class = new ManaClass($"{asmName}global::mana/lang/UInt16", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_U2 };
            UInt32Class = new ManaClass($"{asmName}global::mana/lang/UInt32", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_U4 };
            UInt64Class = new ManaClass($"{asmName}global::mana/lang/UInt64", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_U8 };
            HalfClass = new ManaClass($"{asmName}global::mana/lang/Half", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_R2 };
            FloatClass = new ManaClass($"{asmName}global::mana/lang/Float", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_R4 };
            DoubleClass = new ManaClass($"{asmName}global::mana/lang/Double", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_R8 };
            DecimalClass = new ManaClass($"{asmName}global::mana/lang/Decimal", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_R16 };
            BoolClass = new ManaClass($"{asmName}global::mana/lang/Boolean", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_BOOLEAN };
            CharClass = new ManaClass($"{asmName}global::mana/lang/Char", ValueTypeClass, cormodule) { TypeCode = ManaTypeCode.TYPE_CHAR };
            ArrayClass = new ManaClass($"{asmName}global::mana/lang/Array", ObjectClass, cormodule) { TypeCode = ManaTypeCode.TYPE_ARRAY };
            ExceptionClass = new ManaClass($"{asmName}global::mana/lang/Exception", ObjectClass, cormodule) { TypeCode = ManaTypeCode.TYPE_CLASS };
        }
    }
}
