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
        BinaryReader m_br = null;
        List<FileBlock> m_fileBlocks = new List<FileBlock>();
        DNAStruct m_DNAStruct = null;

        int pointerSize;
        Endianness endianness;
        string version;

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
            string strdna = bf.GetDNAString();
            File.WriteAllText( "dna.txt", strdna);
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
            return System.Text.Encoding.ASCII.GetString( m_br.ReadBytes( count) );
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

        private Int64 ReadPointer()
        {
            Int64 ptr = 0;
            if (pointerSize == 4)
            {
                ptr = m_br.ReadInt32();
            }

            if (pointerSize == 8)
            {
                ptr = m_br.ReadInt64();
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
            FileBlock fb = new FileBlock();

            AligneAt4Bytes();

            fb.position     = m_br.BaseStream.Position;
            fb.id           = System.Text.Encoding.ASCII.GetString(m_br.ReadBytes(4));
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
                if (fb.id == "DNA1")
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


            DNAStruct dna = new DNAStruct();
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
                RawDNAStructure strc = new RawDNAStructure();
                strc.typeIndex = m_br.ReadInt16();
                Int16 fieldCount = m_br.ReadInt16();
                for (int j = 0; j < fieldCount; ++j)
                {
                    RawDNAField field = new RawDNAField();
                    field.typeIndex = m_br.ReadInt16();
                    field.nameIndex = m_br.ReadInt16();
                    strc.fields.Add(field);
                }
                dna.structures.Add(strc);
            }
            m_DNAStruct = dna;
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
            public string id;
            public int size;
            public Int64 oldPointer;
            public int SDNAIndex;
            public int count;
            public long position;
            public long dataPosition;

            public override string ToString()
            {
                return string.Format("{0}@{1} size={2} old@{3} SDNAindex={4} count={5}",id, position, size,oldPointer, SDNAIndex, count  );
            }
        }

        class DNAStruct
        {
            public string name;
            public List<string> names = new List<string>();
            public List<string> types = new List<string>();
            public List<Int16> lengths = new List<short>();
            public List<RawDNAStructure> structures = new List<RawDNAStructure>();
        }

        class RawDNAStructure
        {
            public Int16 typeIndex;
            public List<RawDNAField> fields = new List<RawDNAField>();
        }

        class RawDNAField
        {
            public Int16 typeIndex;
            public Int16 nameIndex;
        }

        string GetDNAString()
        {
            string str = "";
            DNAStruct dna = m_DNAStruct;

            foreach (RawDNAStructure strc in dna.structures)
            {
                str += string.Format("{0} (type_{1})\n", dna.types[strc.typeIndex], strc.typeIndex);
                foreach (var f in strc.fields)
                {
                    str += string.Format("\t({2}){0} {1}\n", dna.types[f.typeIndex], dna.names[f.nameIndex], f.typeIndex);
                }
                str += string.Format("\n\n");
            }


            return str;
        }

        class BlendVar
        {
            internal BlendFile m_blendfile;

            /*
            public long ReadPointer();
            public byte readByte();

            public Int16 readInt16();
            public Int32 readInt32();
            public Int64 readInt64();
            */


        }

        public class ErrorReadingBlendFileException : Exception { }

    }
}
