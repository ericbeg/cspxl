using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace pxl
{
    public class BlendFile
    {
        internal BinaryReader m_br = null;
        List<FileBlock> m_fileBlocks = new List<FileBlock>();
        DNA1 m_dna1 = null;

        int pointerSize;
        Endianness endianness;
        string version;

        internal BlendVar GetVarByOldPointer( long pointer )
        {
            BlendVar bvar = null;

            return bvar;
        }

        private BlendFile()
        {
            ;
        }

        public static BlendFile Open(string filepath)
        {
            BlendFile bf = new BlendFile();
            Header hdr = new Header();
            EndiannessSaveBinaryReader esbr = new EndiannessSaveBinaryReader(File.Open(filepath, FileMode.Open));
            bf.m_br = esbr;
            hdr.identifier  = bf.m_br.ReadBytes(7);
            hdr.pointerSize = bf.m_br.ReadByte();
            hdr.endiannes   = bf.m_br.ReadByte();
            hdr.version     = bf.m_br.ReadBytes(3);
            string id =  System.Text.Encoding.ASCII.GetString( hdr.identifier ); 
            if (id != "BLENDER")
            {
                throw new ErrorReadingBlendFileException();

            }


            if (hdr.pointerSize == '_')
            {
                bf.pointerSize = 4;
            }
            else if (hdr.pointerSize == '-')
            {
                bf.pointerSize = 8;
            }
            else
            {
                throw new ErrorReadingBlendFileException();
            }
                

            if (hdr.endiannes == 'v')
            {
                bf.endianness = Endianness.LittleEndian;

            }
            else if (hdr.endiannes == 'V')
            {
                bf.endianness = Endianness.LittleEndian;

            }
            else
            {
                throw new ErrorReadingBlendFileException();
            }

            bf.version = System.Text.Encoding.ASCII.GetString(hdr.version);

            esbr.sourceEndianness = bf.endianness;

            Console.Write( string.Format("{0} ptrsize={1} {2} ", id , bf.pointerSize, bf.endianness ) );

            bf.ReadFileBlocks();
            bf.ReadDNAStruct();

            File.WriteAllText("dna.txt", bf.GetDNAString());
            File.WriteAllText("data.txt", bf.GetFileBlockString());

            return bf;
        }


        private void AligneAt4Bytes()
        {
            if (m_br != null)
            {

                long jump = m_br.BaseStream.Position % 4;
                // 0 - 0
                // 1 - 3
                // 2 - 2 
                // 3 - 1
                m_br.BaseStream.Position += (4 - jump) % 4;
            }
        }

        private string ReadBytesAsString(int count)
        {
            string str = System.Text.Encoding.ASCII.GetString(m_br.ReadBytes(count));
            while (str.Last() == '\0')
            {
                str = str.Remove(str.Length - 1);
            }
            return str;
        }

        private string ReadNullTerminatedString()
        {
            List<byte> bytes = new List<byte>();
            byte b = m_br.ReadByte();
            while (b != '\0')
            {
                bytes.Add(b);
                b = m_br.ReadByte();
            }
            return System.Text.Encoding.ASCII.GetString( bytes.ToArray() );
;
        }

        private UInt64 ReadPointer()
        {
            UInt64 ptr = 0;
            if (pointerSize == 4)
            {
                ptr = m_br.ReadUInt32();
            }

            if (pointerSize == 8)
            {
                ptr = m_br.ReadUInt64();
            }

            return ptr;
        }

        private void ReadFileBlocks()
        {
            // Read file blocks
            while (m_br.BaseStream.Position < m_br.BaseStream.Length) // until EOF
            {
                FileBlock fb = ReadFileBlock();
                m_fileBlocks.Add(fb);
            }
        }

        private FileBlock ReadFileBlock()
        {
            FileBlock fb = new FileBlock( this );

            AligneAt4Bytes();

            fb.position     = m_br.BaseStream.Position;
            fb.code         = ReadBytesAsString(4);
            fb.size         = m_br.ReadInt32();
            fb.oldPointer   = ReadPointer();
            fb.SDNAIndex    = m_br.ReadInt32();
            fb.count        = m_br.ReadInt32();
            fb.dataPosition = m_br.BaseStream.Position;

            // Advances stream position to next file block
            m_br.BaseStream.Position += fb.size;
            //Console.WriteLine( fb.ToString() );
            return fb;
        }



        private void ReadDNAStruct()
        {
            FileBlock dna1 = null;
            foreach (FileBlock fb in m_fileBlocks)
            {
                if (fb.code == "DNA1")
                {
                    dna1 = fb;
                    break;
                }
            }

            if (dna1 == null)
            {
                throw new ErrorReadingBlendFileException();
            }

            m_br.BaseStream.Position = dna1.dataPosition;


            DNA1 dna = new DNA1();
            string id = ReadBytesAsString(4);
            if (id != "SDNA")
            {
                throw new ErrorReadingBlendFileException();
            }
            dna.name = ReadBytesAsString(4);
            
            Int32 nameCount = m_br.ReadInt32();
            for (int i = 0; i < nameCount; ++i)
            {
                dna.names.Add(ReadNullTerminatedString());    
            }

            AligneAt4Bytes();
            string typeid = ReadBytesAsString(4);
            if (typeid != "TYPE")
            {
                throw new ErrorReadingBlendFileException();
            }


            Int32 typeCount = m_br.ReadInt32();
            for (int i = 0; i < typeCount; ++i)
            {
                dna.types.Add(ReadNullTerminatedString());
            }

            AligneAt4Bytes();
            string tlenid = ReadBytesAsString(4);
            if (tlenid != "TLEN")
            {
                throw new ErrorReadingBlendFileException();
            }

            for (int i = 0; i < typeCount; ++i)
            {
                dna.lengths.Add(m_br.ReadInt16());
            }


            AligneAt4Bytes();
            string structid = ReadBytesAsString(4);
            if (structid != "STRC")
            {
                throw new ErrorReadingBlendFileException();
            }

            Int32 structCount = m_br.ReadInt32();
            for (int i = 0; i < structCount; ++i)
            {
                SDNAStruct strc = new SDNAStruct( dna );
                strc.typeIndex = m_br.ReadInt16();
                Int16 fieldCount = m_br.ReadInt16();
                for (int j = 0; j < fieldCount; ++j)
                {
                    DNAField field = new DNAField(dna );
                    field.typeIndex = m_br.ReadInt16();
                    field.nameIndex = m_br.ReadInt16();
                    strc.fields.Add(field);
                }
                dna.Add(strc);
            }
            m_dna1 = dna;
        }


        class Header
        {
            public byte[] identifier;
            public byte pointerSize;
            public byte endiannes;
            public byte[] version;
        }

        class FileBlock
        {
            private BlendFile m_fb;

            public FileBlock(BlendFile blendFile)
            {
                m_fb = blendFile;
            }

            public string code;
            public int size;
            public UInt64 oldPointer;
            public int SDNAIndex;
            public int count;
            public long position;
            public long dataPosition;

            public override string ToString()
            {
                return string.Format("{0}@{1} size={2} old@{3} SDNAindex={4} count={5}", code, position, size, oldPointer, SDNAIndex, count  );
            }

            public int elementSize
            {
                get
                {
                    DNA1 dna1 = m_fb.m_dna1;

                    Int16 typeIndex = dna1.GetSDNAbyIndex(SDNAIndex).typeIndex;
                    int elementSize = dna1.lengths[typeIndex];
                    return elementSize;

                }
            }
            public string type
            {
                get
                {
                    DNA1 dna1 = m_fb.m_dna1;
                    Int16 typeIndex = dna1.GetSDNAbyIndex(SDNAIndex).typeIndex;
                    return dna1.types[typeIndex];
                }


            }
        }

        class DNA1
        {
            public string name;

            List<SDNAStruct> m_SDNAStructs = new List<SDNAStruct>();
            Dictionary<Int16, SDNAStruct> m_sdnaByTypeIndex = new Dictionary<short,SDNAStruct>();

            internal void Add( SDNAStruct sdna )
            {
                m_SDNAStructs.Add(sdna);
                m_sdnaByTypeIndex[sdna.typeIndex] = sdna;
            }

            public SDNAStruct[] sdnaStructs
            {
                get
                {
                    return m_SDNAStructs.ToArray();
                }
            }

            public SDNAStruct GetSDNAbyIndex( int sdnaIndex )
            {
                return m_SDNAStructs[ sdnaIndex ];
            }

            public SDNAStruct GetSDNAbyType(Int16 typeIndex)
            {
                SDNAStruct sdna = null;
                if (m_sdnaByTypeIndex.ContainsKey(typeIndex))
                {
                    sdna = m_sdnaByTypeIndex[typeIndex]; 
                }
                return sdna;
            }



            public List<string> names = new List<string>();
            public List<string> types = new List<string>();
            public List<Int16> lengths = new List<short>();


        }

        class SDNAStruct
        {
            private DNA1 m_dna1;
            internal SDNAStruct(DNA1 dna1)
            {
                m_dna1 = dna1;
            }

            public string type
            {
                get
                {
                    return m_dna1.types[typeIndex];

                }
            }
            public Int16 typeIndex;
            public List<DNAField> fields = new List<DNAField>();
        }

        class DNAField
        {
            private DNA1 m_dna1;
            internal DNAField(DNA1 dna1)
            {
                m_dna1 = dna1;
            }

            public string name
            {
                get
                {
                    return m_dna1.names[nameIndex];
                }
            }

            public string type
            {
                get
                {
                    return m_dna1.types[typeIndex];
                }
            }

            public Int16 typeIndex;
            public Int16 nameIndex;
        }

        string GetFileBlockString()
        {
            StringBuilder str = new StringBuilder();
            foreach (FileBlock fb in m_fileBlocks)
            {
                str.Append( string.Format("{0}\n", fb.ToString()));
                for (int i = 0; i < fb.count; ++i)
                {
                    str.Append( string.Format("{0}:{1}/{2}\n", fb.type, i + 1, fb.count) );

                    Stack<BlendVar> fifo = new Stack<BlendVar>();
                    Stack<int> fifo_indent = new Stack<int>();
                    BlendVar varfb = new BlendVar(this, fb.dataPosition + i * fb.elementSize, fb.SDNAIndex);
                    
                    fifo.Push(varfb);
                    fifo_indent.Push(1);
                    while (fifo.Count > 0)
                    {

                        BlendVar blendVar = fifo.Pop();
                        int indent = fifo_indent.Pop();

                        for (int t = 0; t < indent; ++t )
                            str.Append( "\t" );

                        object value = blendVar.Read();
                        str.Append( string.Format("{0} {1}", blendVar.type, blendVar.name) );
                        if (value != null)
                        {
                            str.Append(" = ");
                            Object[] values = blendVar;
                            if (values != null )
                            {
                                str.Append("[");
                                foreach (var v in values)
                                {
                                    str.Append(v);
                                    str.Append(", ");
                                }
                                str.Remove(str.Length - 2, 2);
                                str.Append("]");

                            }
                            else
                            {
                                str.Append( value );
                            }
                        }
                        str.Append("\n");

                        BlendVar[] submembers = blendVar.members;
                        if (submembers != null)
                        {
                            int n = submembers.Length;
                            for (int m = 0; m < n; ++m )
                            {
                                fifo.Push(submembers[n -1- m]);
                                fifo_indent.Push(indent + 1);
                            }
                        }
                    }

                    str.Append( "\n" );
                }
            }
            return str.ToString();
        }

        string GetDNAString()
        {
            StringBuilder str = new StringBuilder();
            DNA1 dna = m_dna1;

            for (int i = 0; i < dna.types.Count; ++i)
            {
                str.Append( string.Format("{0}\t{1}\t{2}\n", i, dna.types[i], dna.lengths[i]) );
            }

            for (int i = 0; i < dna.sdnaStructs.Length; ++i )
            {
                SDNAStruct strc = dna.sdnaStructs[i];
                str.Append( string.Format("sdna_{2} {0} (type_{1})\n", strc.type, strc.typeIndex, i) );
                foreach (DNAField f in strc.fields)
                {
                    str.Append( string.Format("\t({2}){0} {1}\n", f.type, f.name, f.typeIndex) );
                }
                str.Append( string.Format("\n\n") );
            }
            return str.ToString();
        }

        public class BlendVar
        {
            internal BlendFile m_bf;

            private SDNAStruct m_sdna = null;
            private BlendVarType m_varType;
            private Int16 m_typeIndex;
            private long m_offset;
            private string m_type;
            private int m_count;
            private BlendVar[] m_members;

            public string name;

            public BlendVarType varType{ get{ return m_varType;}}
            public int count { get { return m_count; } }

            internal BlendVar(BlendFile bf, long offset, int sdnaIndex)
            {
                m_bf = bf;
                m_offset = offset;
                m_typeIndex = bf.m_dna1.sdnaStructs[sdnaIndex].typeIndex;
                m_varType = GetType(m_typeIndex, null);

                Init(bf, m_typeIndex, null);
            }

            internal BlendVar(BlendFile bf, long offset, Int16 typeIndex, string name)
            {
                m_bf        = bf;
                m_offset    = offset;
                m_typeIndex = typeIndex;  
                m_varType   = GetType(typeIndex, name);
                this.name = name;

                Init(bf, typeIndex, name);
            }

            private void Init(BlendFile bf, Int16 typeIndex, string name)
            {
                if (m_varType == BlendVarType.Structure)
                {
                    m_sdna = bf.m_dna1.GetSDNAbyType(typeIndex);
                    if (m_sdna != null)
                    {
                        m_type = m_sdna.type;
                    }
                }
                else
                {
                    m_type = bf.m_dna1.types[ typeIndex ];
                }

                if (m_varType == BlendVarType.Array || m_varType == BlendVarType.List)
                {
                    m_count = 1;
                    int startIndex = 0;
                    while( startIndex >= 0)
                    {
                        int s = name.IndexOf('[', startIndex);
                        int e = name.IndexOf(']', startIndex);
                        if (s >= 0)
                        {
                            int l = e - (s + 1);
                            string num = name.Substring(s + 1, l);
                            m_count *= Convert.ToInt32(num);
                            startIndex = e + 1;
                        }
                        else
                        {
                            startIndex = -1;
                        }
                    }
                }
                else
                {
                    m_count = 1;
                }
            }

            public string type
            {
                get { return m_type; }
            }

            public Object Read()
            {
                Object obj = null;
                m_bf.m_br.BaseStream.Position = m_offset;


                if (varType == BlendVarType.Primitive)
                {
                    switch (type)
                    {
                        case "char":    obj = m_bf.m_br.ReadByte();     break;
                        case "short":   obj = m_bf.m_br.ReadInt16();    break;
                        case "int":     obj = m_bf.m_br.ReadInt32();    break;
                        case "float":   obj = m_bf.m_br.ReadSingle();   break;
                        case "double":  obj = m_bf.m_br.ReadDouble();   break;
                    }
                }
                else if( varType == BlendVarType.Array )
                {
                    int count = m_count;
                    switch (type)
                    {
                        case "char":    obj = new byte[count];   break;
                        case "short":   obj = new Int16[count];  break;
                        case "int":     obj = new Int32[count];  break;
                        case "float":   obj = new float[count];  break;
                        case "double":  obj = new double[count]; break;
                    }

                    for (int i = 0; i < count; ++i)
                    {
                        switch (type)
                        {
                            case "char":    ((byte[])   obj)[i]  = m_bf.m_br.ReadByte();     break;
                            case "short":   ((Int16[])  obj)[i]  = m_bf.m_br.ReadInt16();    break;
                            case "int":     ((Int32[])  obj)[i]  = m_bf.m_br.ReadInt32();    break;
                            case "float":   ((float[])  obj)[i]  = m_bf.m_br.ReadSingle();   break;
                            case "double":  ((double[]) obj)[i]  = m_bf.m_br.ReadDouble();   break;
                        }

                    }
                    
                    if (type == "char")
                    {
                        List<byte> bstr = new List<byte>();
                        byte[] bytes = ((byte[])obj);

                        foreach (byte b in bytes )
                        {
                            if (b == '\0')
                                break;
                            bstr.Add(b);
                        }
                        string str = System.Text.Encoding.ASCII.GetString( bstr.ToArray() );
                        obj = string.Format("\"{0}\"", str); 
                    }
                }
                else if (varType == BlendVarType.Pointer)
                {
                    obj = m_bf.ReadPointer();
                }
                
                return obj;
            }

            public BlendVar[] members
            {
                get
                {
                    if (m_members == null)
                    {
                        if (varType == BlendVarType.Structure)
                        {
                            long offset = m_offset;
                            List<BlendVar> members = new List<BlendVar>();
                            foreach (DNAField f in m_sdna.fields)
                            {
                                BlendVar member = new BlendVar(m_bf, offset, f.typeIndex, f.name);
                                members.Add( member );

                                int typeLength = m_bf.m_dna1.lengths[f.typeIndex];
                                if (member.m_varType == BlendVarType.Pointer || member.m_varType == BlendVarType.List)
                                {
                                    typeLength = m_bf.pointerSize;
                                }
                                
                                offset += member.count * typeLength;
                            }
                            m_members = members.ToArray();
                        }
                    }
                    return m_members;
                }
            }

            public BlendVar this[int index]
            {
                get
                {
                    return null;
                }   
            }

            public BlendVar this[string name]
            {
                get
                {
                    return null;
                }
            }

            public static BlendVarType GetType(int typeIndex, string name)
            {
                BlendVarType type = BlendVarType.Structure;
                bool isPointer = false;
                bool isArray   = false;
                if (name != null)
                {
                    if ( name.Length > 0 && name[0] == '*')
                    {
                        isPointer = true;
                    }

                    if (name.Length > 0 && name.Last() == ']')
                    {
                        isArray = true;
                    }
                }

                if (!isArray && !isPointer)
                {
                    if (typeIndex < 12)
                    {
                        type = BlendVarType.Primitive;
                    }
                }
                else if (isArray && !isPointer)
                {
                    type = BlendVarType.Array;
                }
                else if (isPointer)
                {
                    type = BlendVarType.Pointer;
                }

                return type;
            }

            // primitive implicit casting
            public static implicit operator byte(BlendVar v) { return (byte)v.Read(); }
            public static implicit operator Int16(BlendVar v) { return (Int16)v.Read(); }
            public static implicit operator Int32(BlendVar v) { return (Int32)v.Read(); }
            public static implicit operator Int64(BlendVar v) { return (Int64)v.Read(); }

            public static implicit operator float(BlendVar v) { return (float)v.Read(); }
            public static implicit operator double(BlendVar v) { return (double)v.Read(); }

            public static implicit operator string(BlendVar v) { return (string)v.Read(); }

            // array of primitive implicit casting
            public static implicit operator byte[](BlendVar v) { return v.Read() as byte[]; }
            public static implicit operator Int16[](BlendVar v) { return v.Read() as Int16[]; }
            public static implicit operator Int32[](BlendVar v) { return v.Read() as Int32[]; }
            public static implicit operator Int64[](BlendVar v) { return v.Read() as Int64[]; }

            public static implicit operator float[](BlendVar v) { return v.Read() as float[]; }
            public static implicit operator double[](BlendVar v) { return v.Read() as double[]; }

            public static implicit operator string[](BlendVar v) { return v.Read() as string[]; }

            public static implicit operator object[](BlendVar v)
            {
                Object[] objs = null;
                List<Object> list = new List<Object>();
                
                byte[] bytes = v;
                Int16[] int16s = v;
                Int32[] int32s = v;
                Int64[] int64s = v;

                float[] floats  = v;
                double[] doubles = v;

                string[] strings = v;

                if (bytes != null) foreach (var o in bytes) list.Add(o);
                if (int16s != null) foreach (var o in int16s) list.Add(o);
                if (int32s != null) foreach (var o in int32s) list.Add(o);
                if (int64s != null) foreach (var o in int64s) list.Add(o);

                if (doubles != null) foreach (var o in doubles) list.Add(o);
                if (floats != null) foreach (var o in floats) list.Add(o);

                if (strings != null) foreach (var o in strings) list.Add(o);

                if( list.Count > 0 )
                    objs = list.ToArray();

                return objs;
            }



            public enum BlendVarType
            {
                Primitive,
                Pointer,
                List,
                Array,
                Structure
            }
        }

        public class ErrorReadingBlendFileException : Exception { }

    }
}
