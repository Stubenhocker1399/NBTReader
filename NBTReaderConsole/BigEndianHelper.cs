using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace NBTReaderConsole
{
    public static class BigEndianHelper
    {
        private class ValueType
        {
            public readonly int Size;
            public readonly object ConvertAction;

            public ValueType(Type type)
            {
                Size = Marshal.SizeOf(type);
                if (type == typeof(byte))
                {
                    ConvertAction = (Func<byte[], int, byte>) ((buffer, index) => buffer[index]);
                    return;
                }

                ConvertAction = typeof(BitConverter)
                    .GetMethod($"To{type.Name}", BindingFlags.Static | BindingFlags.Public)
                    .CreateDelegate(typeof(Func<,,>).MakeGenericType(typeof(byte[]), typeof(int), type));
            }
        }

        private static Dictionary<Type, ValueType> _sValueTypes = new Dictionary<Type, ValueType>();
        private static ValueType GetValueType<T>()
        {
            ValueType valueType;
            if (_sValueTypes.TryGetValue(typeof(T), out valueType)) return valueType;

            valueType = new ValueType(typeof(T));
            _sValueTypes.Add(typeof(T), valueType);
            return valueType;
        }

        [ThreadStatic]
        private static byte[] _sReadBuffer;
        public static T ReadBigEndian<T>(BinaryReader reader)
            where T : struct
        {
            var valueType = GetValueType<T>();
            if (_sReadBuffer == null) _sReadBuffer = new byte[8];

            reader.Read(_sReadBuffer, 0, valueType.Size);

            var half = valueType.Size >> 1;
            for (var i = 0; i < half; ++i)
            {
                var temp = _sReadBuffer[i];
                var opposite = valueType.Size - i - 1;
                _sReadBuffer[i] = _sReadBuffer[opposite];
                _sReadBuffer[opposite] = temp;
            }

            return ((Func<byte[], int, T>)valueType.ConvertAction)(_sReadBuffer, 0);
        }
    }
}
