#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace pxl
{
    /// <summary>
    /// Reads *.blend file produced by Blender (http://www.blender.org).
    /// </summary>
    public class BlendFile
    {
        private BlendFile()
        {
            ;
        }


        public string DNAString { get { return GetDNAString(); } }
        public string fileBlockString { get { return GetFileBlockString(); } }

        public static string GetFilepath( string path )
        {
            string abspath = null;

            if (path != null)
            {
                abspath = path.Replace("//", "./");
            }

            return abspath;
        }

        internal BinaryReader m_br = null;
        List<FileBlock> m_fileBlocks = new List<FileBlock>();
        Dictionary<ulong, FileBlock> m_fileBlockByOldPointer = new Dictionary<ulong, FileBlock>();
        Dictionary<string, FileBlock> m_fileBlockByName = new Dictionary<string, FileBlock>();
        Dictionary<string, List<string>> m_childrenNames = new Dictionary<string, List<string>>();
        static Dictionary<string, IBlendLoader> m_loaders = new Dictionary<string, IBlendLoader>();
        DNA1 m_dna1 = null;

        int pointerSize;
        Endianness endianness;
        string version;

        internal FileBlock GetFileBlockByOldPointer(ulong pointer)
        {
            FileBlock fb = null;
            if (m_fileBlockByOldPointer.ContainsKey(pointer))
            {
                fb = m_fileBlockByOldPointer[pointer];
            }
            return fb;
        }

        internal FileBlock GetFileBlockByName(string name )
        {
            FileBlock fb = null;
            if (m_fileBlockByName.ContainsKey(name))
            {
                fb = m_fileBlockByName[name];
            }
            return fb;
        }

        public FileBlock[] fileBloks { get { return m_fileBlocks.ToArray(); } }

        /// <summary>
        /// Resgister a datablock loader.
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="code"></param>
        public static void Register(IBlendLoader loader, string code)
        {
            m_loaders[code] = loader;
        }

        /// <summary>
        /// Load the datablock that has this name.
        /// </summary>
        /// <param name="datablock"></param>
        /// <returns></returns>
        public Object Load(string datablock)
        {
            Object data = null;

            IBlendLoader loader = GetLoaderFor(datablock);
            if (loader != null)
            {
                BlendVar bvar = GetVar(datablock);
                if (bvar != null)
                {
                    data = loader.Load(bvar);
                }
            }
            return data;
        }
        /// <summary>
        /// Returns the datablock with the name as a BlendVar.
        /// </summary>
        /// <param name="name">Datablock name</param>
        /// <returns>null if the .blend has no datablock named name</returns>

        public BlendVar this[string name] { get { return GetVar(name); } }

        /// <summary>
        /// List all the datablock names contained in the currently opened .blend file. 
        /// </summary>
        public string[] datablockNames
        {
            get
            {
                string[] keys = null;
                if (m_fileBlockByName != null)
                {
                    keys = new string[m_fileBlockByName.Keys.Count];
                    m_fileBlockByName.Keys.CopyTo(keys, 0);
                }
                return keys;
            }
        }

        /// <summary>
        /// Return the BinaryReader attached to the BlenderFile.
        /// </summary>
        public BinaryReader binaryReader { get { return m_br; } }

        static IBlendLoader GetLoaderFor(string name)
        {
            string code = name.Substring(0, 2);
            return m_loaders[code];
        }

        internal BlendVar GetVar(string name)
        {
            BlendVar bvar = null;
            FileBlock fb = GetFileBlockByName(name);
            if (fb != null)
            {
                bvar = new BlendVar(this, fb.dataPosition, fb.SDNAIndex);
                bvar.m_fileBlockIndex = fb.fileBlockIndex;
            }
            return bvar;
        }


        internal BlendVar GetVarByOldPointer( ulong pointer )
        {
            BlendVar bvar = null;
            FileBlock fb = GetFileBlockByOldPointer(pointer);
            if (fb != null)
            {
                bvar = new BlendVar(this, fb.dataPosition, fb.SDNAIndex);
                bvar.m_fileBlockIndex = fb.fileBlockIndex;
            }
            return bvar;
        }

        internal BlendVar[] GetVarsByOldPointer(ulong pointer)
        {
            List<BlendVar> bvars = new List<BlendVar>();
            FileBlock fb = GetFileBlockByOldPointer(pointer);
            if (fb != null)
            {
                for (int i = 0; i < fb.count; ++i)
                {
                    BlendVar varfb = new BlendVar(this, fb.dataPosition + i * fb.elementSize, fb.SDNAIndex);
                    varfb.m_fileBlockIndex = fb.m_fileBlockIndex;
                    bvars.Add(varfb);
                }
            }
            return bvars.ToArray();
        }




        /// <summary>
        /// Open the blend file located at filepath and returns a BlendFile object.
        /// </summary>
        public static BlendFile Open(string filepath)
        {
            BlendFile bf = new BlendFile();
            Header hdr = new Header();
            BinaryReader reader = new BinaryReader(File.Open(filepath, FileMode.Open));
            bf.m_br = reader;
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


            if (bf.endianness == Endianness.BigEndian)
            {
                long position = bf.m_br.BaseStream.Position;
                bf.m_br.Close(); bf.m_br = null;
                EndiannessSaveBinaryReader esbr = new EndiannessSaveBinaryReader(File.Open(filepath, FileMode.Open));
                esbr.sourceEndianness = bf.endianness;
                esbr.BaseStream.Position = position;
                bf.m_br = esbr;
            }

            //Console.Write( string.Format("{0} ptrsize={1} {2} ", id , bf.pointerSize, bf.endianness ) );

            bf.ReadFileBlocks();
            bf.ReadDNAStruct();
            bf.CreateNameIndex();
            bf.SearchObjectChildren();

            return bf;
        }
        /// <summary>
        /// Close the .blend file. The BlendFile object should not be used after this call. Set it to null. 
        /// </summary>
        public void Close()
        {
            m_br.Close();
            m_br = null;
            m_dna1 = null;
            m_fileBlockByName = null;
            m_fileBlockByOldPointer = null;
            m_fileBlocks = null;
            m_childrenNames.Clear();
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

        private void CreateNameIndex()
        {
            foreach( FileBlock fb in m_fileBlocks)
            {
                if (fb.oldPointer == 0)
                    continue;

                BlendVar var = GetVarByOldPointer(fb.oldPointer);
                
                BlendVar id = var["id"];
                if (id != null )
                {
                    var name = id["name"];
                    if (name != null)
                    {
                        string strname = (string)name;
                        m_fileBlockByName[(string)name] = fb;
                    }
                }
            }

        }

        private void ReadFileBlocks()
        {
            // Read file blocks
            int i = 0;
            while (m_br.BaseStream.Position < m_br.BaseStream.Length) // until EOF
            {
                FileBlock fb = ReadFileBlock();
                fb.m_fileBlockIndex = i;
                m_fileBlocks.Add(fb);
                if( fb.oldPointer != 0 )
                    m_fileBlockByOldPointer[fb.oldPointer] = fb;
                ++i;
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

        public string[] GetChildrenNames(string objectName)
        {
            string[] childrenNames = null;
            if (m_childrenNames.ContainsKey(objectName))
            {
                childrenNames = m_childrenNames[objectName].ToArray();
            }
            return childrenNames;
        }

        private void SearchObjectChildren()
        {
            for (int i = 0; i < datablockNames.Length; ++i)
            {
                string name = datablockNames[i];
                if (name.StartsWith("OB"))
                {
                    BlendVar ob  = this[name];
                    BlendVar par = ob["parent"];
                    if (par != null)
                    {
                        string parentName = par["id"]["name"];
                        string childName  =  ob["id"]["name"];

                        if (!m_childrenNames.ContainsKey(parentName))
                        {
                            m_childrenNames[parentName] = new List<string>();
                        }
                        m_childrenNames[parentName].Add(childName);
                    }
 
                }
            }
        }

        internal class Header
        {
            public byte[] identifier;
            public byte pointerSize;
            public byte endiannes;
            public byte[] version;
        }

        /// <summary>
        /// Represents a Blender File Block.
        /// </summary>
        public class FileBlock
        {
            private BlendFile m_bf;

            public FileBlock(BlendFile blendFile)
            {
                m_bf = blendFile;
            }

            public string code;
            public int size;
            public UInt64 oldPointer;
            public int SDNAIndex;
            public int count;
            public long position;
            public long dataPosition;

            internal int m_fileBlockIndex;

            public int fileBlockIndex { get { return m_fileBlockIndex; } }

            /// <summary>
            /// Set the binaryReader stream position at the the position of this fileblock data.
            /// This is used to access the binary data directly by using binaryReader.
            /// </summary>
            public void Seek()
            {
                if (m_bf != null && m_bf.m_br != null)
                    m_bf.m_br.BaseStream.Position = dataPosition;
            }

            /// <summary>
            /// Set the binaryReader stream position at the the position of the ith data element.
            /// This is used to access the binary data directly by using binaryReader.
            /// </summary>
            public void Seek( int i)
            {
                if (m_bf != null && m_bf.m_br != null)
                {
                    m_bf.m_br.BaseStream.Position = dataPosition + i*elementSize;
                }
            }


            public override string ToString()
            {
                return string.Format("{0}@{1} size={2} old@{3} SDNAindex={4} count={5}", code, position, size, oldPointer.ToString("x16"), SDNAIndex, count  );
            }

            public int elementSize
            {
                get
                {
                    DNA1 dna1 = m_bf.m_dna1;

                    Int16 typeIndex = dna1.GetSDNAbyIndex(SDNAIndex).typeIndex;
                    int elementSize = dna1.lengths[typeIndex];
                    return elementSize;

                }
            }
            public string type
            {
                get
                {
                    DNA1 dna1 = m_bf.m_dna1;
                    Int16 typeIndex = dna1.GetSDNAbyIndex(SDNAIndex).typeIndex;
                    return dna1.types[typeIndex];
                }


            }
        }

        internal class DNA1
        {
            public string name;

            List<SDNAStruct> m_SDNAStructs = new List<SDNAStruct>();
            Dictionary<Int16, SDNAStruct> m_sdnaByTypeIndex = new Dictionary<short,SDNAStruct>();

            internal void Add( SDNAStruct sdna )
            {
                m_SDNAStructs.Add(sdna);
                m_sdnaByTypeIndex[sdna.typeIndex] = sdna;
                //m_sdnaByOldAddress[sdna]
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

        internal class SDNAStruct
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

        internal class DNAField
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
                this.binaryReader.BaseStream.Position = fb.dataPosition;

                //byte[] data = this.binaryReader.ReadBytes(fb.size);
                //foreach (byte b in data)
                //    str.Append( string.Format("{0}.", b.ToString("x2")) );

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

                        for (int t = 0; t < indent; ++t)
                            str.Append("\t");

                        object value = blendVar.Read();
                        string indicator = "";

                        if( blendVar.isPointer ) indicator = "*";

                        if (blendVar.isFixedSizeArray)
                        {
                            indicator += string.Format("[{0}]", blendVar.count );
                        }
                        else if (blendVar.count > 1)
                        {
                            indicator += "[]";
                        }

                        str.Append(string.Format("{0}{1} {2}", blendVar.type, indicator , blendVar.m_name));
                        if (value != null)
                        {
                            str.Append(" = ");
                            Object[] values = blendVar;
                            if (values != null)
                            {
                                str.Append("[");
                                foreach (var v in values)
                                {
                                    str.Append(v);
                                    str.Append(" ");
                                }
                                str.Remove(str.Length - 1, 1);
                                str.Append("]");

                            }
                            else
                            {
                                str.Append(value);
                            }
                        }
                        str.Append("\n");

                        if (!blendVar.isPointer)
                        {
                            BlendVar[] submembers = blendVar.fields;
                            if (submembers != null)
                            {
                                int n = submembers.Length;
                                for (int m = 0; m < n; ++m)
                                {
                                    fifo.Push(submembers[n - 1 - m]);
                                    fifo_indent.Push(indent + 1);
                                }
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
                str.Append( string.Format("{0}\t{1}\t{2} bytes\n", i, dna.types[i], dna.lengths[i]) );
            }

            for (int i = 0; i < dna.sdnaStructs.Length; ++i )
            {
                SDNAStruct strc = dna.sdnaStructs[i];
                str.Append( string.Format("sdna_{2} {0} (type_{1} - {3} bytes)\n", strc.type, strc.typeIndex, i, dna.lengths[ strc.typeIndex ]) );
                foreach (DNAField f in strc.fields)
                {
                    str.Append( string.Format("\t({2}){0} {1} - {3} bytes\n", f.type, f.name, f.typeIndex,  dna.lengths[ f.typeIndex ]) );
                }
                str.Append( string.Format("\n\n") );
            }
            return str.ToString();
        }
        /// <summary>
        /// Generic object representing a piece of data read from a .blend file.
        /// </summary>
        public class BlendVar
        {
            /// <summary>
            /// BlendFile this BlendVar originates from.
            /// </summary>
            public BlendFile blendFile { get { return m_bf; } }

            internal bool m_isPointer;
            internal bool m_isPrimitive;
            internal bool m_isFixedSizeArray;

            internal BlendFile m_bf;
            internal int m_fileBlockIndex;
            private SDNAStruct m_sdna = null;
            private Int16 m_typeIndex;
            private long m_offset;
            private string m_type;
            private int m_typeSize;
            private int m_count;
            private BlendVar[] m_fields;
            private Dictionary<string, int> m_memberIndexByName = new Dictionary<string, int>();


            internal string m_name;

            internal int count { get { return m_count; } }

            internal BlendVar(BlendFile bf, long offset, int sdnaIndex)
            {
                m_bf = bf;
                m_offset = offset;
                m_typeIndex = bf.m_dna1.sdnaStructs[sdnaIndex].typeIndex;
                SetType(m_typeIndex, null);
                Init(bf, m_typeIndex, null);
            }

            internal BlendVar(BlendFile bf, long offset, Int16 typeIndex, string name)
            {
                m_bf        = bf;
                m_offset    = offset;
                m_typeIndex = typeIndex;  
                SetType(typeIndex, name);

                Init(bf, typeIndex, name);
            }

            private void Init(BlendFile bf, Int16 typeIndex, string name)
            {
                if (isPointer)
                {
                    m_typeSize = bf.pointerSize;
                }
                else
                {
                    m_typeSize = bf.m_dna1.lengths[typeIndex];
                }

                if (!isPrimitive)
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

                {
                    m_count = 1;
                    int startIndex = 0;
                    while( startIndex >= 0 && name != null)
                    {
                        int s = name.IndexOf('[', startIndex);
                        int e = name.IndexOf(']', startIndex);
                        if (s >= 0)
                        {
                            int l = e - (s + 1);
                            string num = name.Substring(s + 1, l);
                            m_count *= Convert.ToInt32(num);
                            startIndex = e + 1;
                            m_isFixedSizeArray = true;
                        }
                        else
                        {
                            startIndex = -1;
                        }
                    }
                }

                m_name = name;
                // clean up name
                if (m_name != null)
                {
                    int p = m_name.IndexOf('[');
                    if (p >= 0)
                    {
                        m_name = m_name.Remove(p);
                    }

                    if (m_name.Length > 0 )
                    {
                        m_name = m_name.Replace("*", "");
                    }
                }

            }

            /// <summary>
            /// The original type size in bytes.
            /// </summary>
            public int typeSize { get { return m_typeIndex; } }

            /// <summary>
            /// The Blender file block this BlendVar originates from.
            /// </summary>
            public FileBlock fileBlock
            {
                get
                {
                    FileBlock fb = null;
                    fb = m_bf.m_fileBlocks[m_fileBlockIndex];
                    return fb;
                }
            }

            /// <summary>
            /// Type of the BlendVar.
            /// </summary>
            public string type
            {
                get { return m_type; }
            }

            /// <summary>
            /// Set the binaryReader stream position at the the position of this BlendVar.
            /// This is used to access the binary data directly by using binaryReader.
            /// </summary>
            public void Seek()
            {
                if( m_bf != null && m_bf.m_br != null )
                    m_bf.m_br.BaseStream.Position = m_offset;
            }

            public bool isPrimitive         { get { return m_isPrimitive; } }
            public bool isPointer           { get { return m_isPointer; } }
            public bool isFixedSizeArray    { get { return m_isFixedSizeArray; } }

            internal static BlendVar[] GetListBase(BlendVar listbase)
            {
                List<BlendVar> vars = new List<BlendVar>();

                if (listbase != null && listbase.type == "ListBase")
                {
                    BlendVar next = listbase["first"];
                    BlendVar last = listbase["last"];

                    if (next != null && last != null)
                    {
                        while (next != null && next.m_offset != last.m_offset)
                        {
                            vars.Add(next);
                            next = next["next"];
                        }
                        vars.Add(last);
                    }

                }

                return vars.ToArray();
            }

            
            internal Object Read()
            {
                Object obj = null;
                Seek();

                if (isPrimitive && !isPointer && m_count == 1)
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
                else if (isPrimitive && !isPointer && m_count > 1)
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
                    
                    // TODO: This should not be here.
                    // We might need to get the array as a byte[].
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
                        obj = str; 
                    }
                }
                else if ( isPointer )
                {
                    ulong address = m_bf.ReadPointer();
                    if (address != 0)
                    {
                        obj = new BlendPointer(m_bf, address);
                        //obj = pointer;
                    }
                }
                
                return obj;
            }
            /// <summary>
            /// Returns an array containing the BlendVar fields
            /// </summary>
            public BlendVar[] fields
            {
                get
                {
                    
                    if (m_fields == null)
                    {
                        if (this.type == "Object")
                        {
                            ;
                        }

                        if (!isPrimitive && !m_isPointer)
                        {
                            long offset = m_offset;
                            List<BlendVar> members = new List<BlendVar>();
                            int m = 0;
                            foreach (DNAField f in m_sdna.fields)
                            {
                                BlendVar member = new BlendVar(m_bf, offset, f.typeIndex, f.name);
                                members.Add( member );

                                int typeLength = m_bf.m_dna1.lengths[f.typeIndex];
                                if (member.isPointer)
                                {
                                    typeLength = m_bf.pointerSize;
                                }
                                
                                offset += member.count * typeLength;
                                m_memberIndexByName[member.m_name] = m;
                                ++m;
                            }
                            m_fields = members.ToArray();
                        }
                    }
                    return m_fields;
                }
            }
            /// <summary>
            /// Returns a field by index.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public BlendVar this[string name]
            {
                get
                {
                    BlendVar member = null;
                    if (fields != null)
                    {
                        if (m_memberIndexByName.ContainsKey(name))
                        {
                            member = m_fields[m_memberIndexByName[name]];
                        }
                    }

                    if (member != null && member.isPointer && !member.isFixedSizeArray && member.count == 1  )
                    {
                        BlendPointer ptr = (BlendPointer)member;
                        if (ptr != null && ptr.address != 0)
                        {
                            member = ptr;
                        }
                        else
                        {
                            member = null;
                        }
                    }

                    return member;
                }
            }

            internal void SetType(int typeIndex, string name)
            {
                bool isPointer = false;
                if (name != null)
                {
                    if ( name.Length > 0 && name[0] == '*')
                    {
                        isPointer = true;
                    }

                    /*
                    if (name.Length > 0 && name.Last() == ']')
                    {
                        isArray = true;
                    }
                     */
                }

                m_isPrimitive = typeIndex < 12;
                m_isPointer  = isPointer;
            }
            /// <summary>
            /// ToString()
            /// </summary>
            /// <returns></returns>

            public override string ToString()
            {
                string str = null;
                if (m_name != null)
                {
                    str = m_name;
                }
                else
                {
                   str = base.ToString();
                }

                return str;
            }

            // primitive implicit casting
#pragma  warning disable 1591
            public static implicit operator byte(BlendVar v) { return (byte)v.Read(); }
            public static implicit operator Int16(BlendVar v) { return (Int16)v.Read(); }
            public static implicit operator Int32(BlendVar v) { return (Int32)v.Read(); }
            public static implicit operator Int64(BlendVar v) { return (Int64)v.Read(); }


            public static implicit operator float(BlendVar v) { return (float)v.Read(); }
            public static implicit operator double(BlendVar v) { return (double)v.Read(); }

            public static implicit operator string(BlendVar v) { return (string)v.Read(); }

            public static implicit operator BlendPointer(BlendVar v) { return (BlendPointer)v.Read(); }
            public static implicit operator FileBlock(BlendVar v)
            {
                FileBlock fb = null;
                if (v != null)
                {
                    fb = v.fileBlock;
                }
                return fb; 
            }

            
            /*
            public static implicit operator BlendPointer(BlendVar v)
            {
                BlendPointer ptr  = null;
                if (v != null && v.varType == BlendVarType.Pointer)
                {
                    ulong address = v;
                    ptr = new BlendPointer(v.m_bf, address);
                }
                return ptr;
            }
             */

            public static implicit operator BlendVar[] (BlendVar v)
            {
                if (v == null)
                    return null;

                BlendVar[] referenced = null;

                if ( v.isPointer && !v.isFixedSizeArray )
                {
                    referenced = v.m_bf.GetVarsByOldPointer( v.fileBlock.oldPointer );
                }
                else if (v.isPointer && v.isFixedSizeArray && v.type != "Link")
                {
                    List<BlendVar> bvars = new List<BlendVar>();
                    BlendPointer[] pointers = v;
                    foreach (var ptr in pointers)
                    {
                        BlendVar bvar = ptr;
                        if (bvar != null)
                        {
                            bvars.Add(bvar);
                        }
                    }
                    referenced = bvars.ToArray();
                }
                else if (v.type == "Link")
                {
                    FileBlock link = v.fileBlock;
                    //FileBlock next = v.blendFile.fileBloks[link.fileBlockIndex + 1];
                    //int count = (int)(next.position - link.dataPosition) /v.blendFile.pointerSize;
                    int count = (int)(link.size) /v.blendFile.pointerSize;

                    List<BlendVar> bvars = new List<BlendVar>();
                    for (int i = 0; i < count; ++i)
                    {
                        v.blendFile.binaryReader.BaseStream.Position = link.dataPosition + i * v.blendFile.pointerSize;
                        ulong address = v.blendFile.ReadPointer();
                        BlendVar pointed = v.blendFile.GetVarByOldPointer(address);
                        if (pointed != null)
                            bvars.Add(pointed);
                    }
                    referenced = bvars.ToArray();
                }
                else if (v.type == "ListBase")
                {
                    referenced = GetListBase(v);
                }



                return referenced;
            }

            public static implicit operator BlendPointer[](BlendVar v)
            {
                BlendPointer[] pointers = null;
                if(v.isPointer && v.isFixedSizeArray)
                {
                    List<BlendPointer> ptrs = new List<BlendPointer>();
                    v.Seek();
                    for (int i = 0; i < v.count; ++i)
                    {
                        ulong address = v.blendFile.ReadPointer();
                        ptrs.Add( new BlendPointer(v.blendFile, address) );
                    }
                    pointers = ptrs.ToArray();
                }
                return pointers;
            }




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

                if (v.isPrimitive)
                {
                    byte[] bytes = v;
                    Int16[] int16s = v;
                    Int32[] int32s = v;
                    Int64[] int64s = v;

                    float[] floats = v;
                    double[] doubles = v;

                    string[] strings = v;

                    if (bytes != null) foreach (var o in bytes) list.Add(o);
                    if (int16s != null) foreach (var o in int16s) list.Add(o);
                    if (int32s != null) foreach (var o in int32s) list.Add(o);
                    if (int64s != null) foreach (var o in int64s) list.Add(o);

                    if (doubles != null) foreach (var o in doubles) list.Add(o);
                    if (floats != null) foreach (var o in floats) list.Add(o);

                    if (strings != null) foreach (var o in strings) list.Add(o);
                } if ( v.isPointer && v.isFixedSizeArray )
                {
                    BlendPointer[] pointers = v;
                    foreach (var p in pointers)
                        list.Add(p);

                }

                if( list.Count > 0 )
                    objs = list.ToArray();

                return objs;
            }

#pragma  warning restore 1591

        }

        /// <summary>
        /// Represents a pointer.
        /// </summary>
        public class BlendPointer
        {
            private BlendFile m_bf;
            private ulong m_address;
            internal BlendPointer(BlendFile bf, ulong address)
            {
                m_bf = bf;
                m_address = address;
            }

            /// <summary>
            /// Returns the pointer's address as a string.
            /// </summary>
            /// <returns></returns>
            public override string  ToString()
            {
 	             return string.Format("@{0}", address.ToString("x16") );
            }

            /// <summary>
            /// Pointer's address.
            /// </summary>
            public ulong address{ get{ return m_address; } }

            /// <summary>
            /// Referenced BlendVar.
            /// </summary>
            /// <param name="ptr"></param>
            /// <returns></returns>
            public static implicit operator BlendVar(BlendPointer ptr)
            {
                BlendVar bvar = null;
                if (ptr != null)
                {
                    bvar = ptr.m_bf.GetVarByOldPointer(ptr.address);
                }
                return bvar;
            }

            /// <summary>
            /// Pointer's address.
            /// </summary>
            /// <param name="ptr"></param>
            /// <returns></returns>
            public static explicit operator UInt64( BlendPointer ptr )
            {
                return ptr.address;
            }
        }

        /// <summary>
        /// Interface of a datablock loader.
        /// </summary>
        public interface IBlendLoader
        {
            /// <summary>
            /// The method specifying how a datablock is loaded.
            /// </summary>
            /// <param name="bvar"></param>
            /// <returns></returns>
            Object Load(BlendVar bvar);
        }

        /// <summary>
        /// Thrown when reading a .blend file has failed.
        /// </summary>
        public class ErrorReadingBlendFileException : Exception { }

    }
}
