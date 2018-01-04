using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Xml.Serialization;

namespace NickAc.Backend.Objects
{
    [Serializable]
    public class DeckImage
    {
        private const int MAX_IMAGE_SIZE = 350;
        public DeckImage()
        {

        }
        public static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = null;
            using (MemoryStream stream = new MemoryStream()) {
                img.Save(stream, ImageFormat.Png);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        [XmlIgnore]
        public Bitmap Bitmap { get; set; }
        [XmlIgnore]
        public byte[] InternalBitmap { get; set; }

        [XmlElement("Bitmap")]
        public byte[] BitmapSerialized {
            get { // serialize
                if (Bitmap == null) return null;
                using (MemoryStream ms = new MemoryStream()) {
                    Bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            set { // deserialize
                if (value == null) {
                    Bitmap = null;
                } else {
                    MemoryStream ms = new MemoryStream(value);
                    Bitmap = new Bitmap(ms);
                    PrepareSmallBitmap(Bitmap);
                }
            }
        }

        public DeckImage(Bitmap bmp)
        {
            Bitmap = bmp;
            PrepareSmallBitmap(bmp);
        }

        private void PrepareSmallBitmap(Bitmap bmp)
        {
            using (Bitmap bmpSmall = new Bitmap(bmp, MAX_IMAGE_SIZE, MAX_IMAGE_SIZE)) {
                InternalBitmap = ImageToByte(bmp.Width > MAX_IMAGE_SIZE ? bmpSmall : bmp);
            }
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
