﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace NickAc.Backend.Objects
{
    public class DeckImage
    {
        public static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = null;
            using (MemoryStream stream = new MemoryStream()) {
                img.Save(stream, ImageFormat.Png);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public Bitmap Bitmap { get; set; }
        public byte[] InternalBitmap { get; set; }

        public DeckImage(Bitmap bmp)
        {
            Bitmap = bmp;
            InternalBitmap = ImageToByte(bmp);
        }
        public DeckImage(byte[] bmp)
        {
            using (var ms = new MemoryStream(bmp)) {
                Bitmap = Image.FromStream(ms) as Bitmap;
            }
            InternalBitmap = bmp;
        }
    }
}
