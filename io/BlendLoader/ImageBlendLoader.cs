#pragma  warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace pxl
{
    public class ImageBlendLoader : BlendFile.IBlendLoader
    {
        public Object Load(BlendFile.BlendVar bvar)
        {
            Bitmap img = null;
            string impath = bvar["name"];
            Stream stream = null;
            BlendFile.BlendVar packedFile = bvar["packedfile"];

            if (packedFile != null)
            {
                int size = packedFile["size"];
                //int seek = packedFile["seek"];
                BlendFile.BlendVar data = packedFile["data"];
                data.Seek();
                byte[] bytes = data.blendFile.binaryReader.ReadBytes(size);
                stream = new MemoryStream(bytes);
            }
            else
            {
                string cleanpath = BlendFile.GetFilepath(impath);
                stream = new FileStream(cleanpath, System.IO.FileMode.Open);
            }

            if (stream != null)
            {
                img = new Bitmap(stream);
                stream.Close();
            }


            return img;
        }
    }
}
