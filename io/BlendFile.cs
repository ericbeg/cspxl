using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace pxl
{
    public class BlendFile
    {

        BinaryReader m_br = null;
        List<FileBlock> m_fileBlocks = new List<FileBlock>();
        int pointerSize;
        Endianness endianness;
        string version;

        private BlendFile()
        {
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
                throw new InvalidBlendFileException();
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
                throw new InvalidBlendFileException();
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
                throw new InvalidBlendFileException();
            }

            bf.version = System.Text.Encoding.ASCII.GetString(hdr.version);

            esbr.sourceEndianness = bf.endianness;

            Console.Write( string.Format("{0} ptrsize={1} {2} ", id , bf.pointerSize, bf.endianness ) );

            // Read file blocks
            while ( bf.m_br.BaseStream.Position < bf.m_br.BaseStream.Length ) // until EOF
            {
                FileBlock fb = bf.ReadFileBlock();
                bf.m_fileBlocks.Add(fb);
            }

            return bf;
        }

        private void AligneAt4Bytes()
        {
            if (m_br != null)
            {

                long jump = m_br.BaseStream.Position % 4;
                m_br.BaseStream.Position += jump;
            }
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

            Console.WriteLine( fb.ToString() );


            return fb;
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

        public class InvalidBlendFileException : Exception { }

    }
}
