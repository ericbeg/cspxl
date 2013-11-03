#pragma  warning disable 1591


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.IO
{
    enum Endianness
    {
        LittleEndian,
        BigEndian
    }

    class EndiannessSaveBinaryReader : BinaryReader
    {

        

        public Endianness sourceEndianness = Endianness.LittleEndian;
        public EndiannessSaveBinaryReader(Stream input)
            : base(input)
        {
        }

        public EndiannessSaveBinaryReader(Stream input, Encoding encoding)
            : base(input, encoding)
        {
        }


        private byte[] ReadReversedBytes(int count)
        {
            byte[] b = ReadBytes(count);
            b = b.Reverse().ToArray();
            return b;
        }

        public override short ReadInt16()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(2);
                return System.BitConverter.ToInt16(b, 0);
            }
            else
            {
                return base.ReadInt16();
            }
        }

        public override Int32 ReadInt32()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(4);
                return System.BitConverter.ToInt32(b, 0);
            }
            else
            {
                return base.ReadInt32();
            }
        }

        public override Int64 ReadInt64()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(8);
                return System.BitConverter.ToInt64(b, 0);
            }
            else
            {
                return base.ReadInt64();
            }
        }

        public override UInt16 ReadUInt16()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(2);
                return System.BitConverter.ToUInt16(b, 0);
            }
            else
            {
                return base.ReadUInt16();
            }
        }

        public override UInt32 ReadUInt32()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(4);
                return System.BitConverter.ToUInt32(b, 0);
            }
            else
            {
                return base.ReadUInt32();
            }
        }

        public override UInt64 ReadUInt64()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(8);
                return System.BitConverter.ToUInt64(b, 0);
            }
            else
            {
                return base.ReadUInt64();
            }
        }

        public override float ReadSingle()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(4);
                return System.BitConverter.ToSingle(b, 0);
            }
            else
            {
                return base.ReadSingle();
            }
        }

        public override double ReadDouble()
        {
            if (sourceEndianness == Endianness.BigEndian)
            {
                byte[] b = ReadReversedBytes(8);
                return System.BitConverter.ToDouble(b, 0);
            }
            else
            {
                return base.ReadDouble();
            }
        }

        public override bool ReadBoolean()
        {
            throw new NotImplementedException();
        }

        public override char ReadChar()
        {
            throw new NotImplementedException();
        }

        public override char[] ReadChars(int count)
        {
            throw new NotImplementedException();
        }

        public override string ReadString()
        {
            throw new NotImplementedException();
        }

        public override decimal ReadDecimal()
        {
            throw new NotImplementedException();
        }

       
    }
}
