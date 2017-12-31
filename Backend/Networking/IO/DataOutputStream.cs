﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Networking.IO
{
    /**
    * A data output stream lets an application write primitive Java data
    * types to an output stream in a portable way. An application can
    * then use a data input stream to read the data back in.
    *
    * @author  unascribed
    * @see     java.io.DataInputStream
    * @since   JDK1.0
    */
    public class DataOutputStream : IDisposable
    {
        /**
        * The number of bytes written to the data output stream so far.
        * If this counter overflows, it will be wrapped to Integer.MAX_VALUE.
        */
        protected int written;

        /**
        * bytearr is initialized on demand by writeUTF
        */
        private byte[] bytearr;
        private readonly BinaryWriter @out;

        /**
        * Creates a new data output stream to write data to the specified
        * underlying output stream. The counter <code>written</code> is
        * set to zero.
        *
        * @param   out   the underlying output stream, to be saved for later
        *                use.
        * @see     java.io.FilterOutputStream#out
        */
        public DataOutputStream(MemoryStream @out)
        {
            this.@out = new BinaryWriter(@out);
        }

        /**
        * Increases the written counter by the specified value
        * until it reaches Integer.MAX_VALUE.
        */
        private void IncCount(int value)
        {
            int temp = written + value;
            if (temp < 0) {
                temp = int.MaxValue;
            }
            written = temp;
        }

        /**
        * Writes the specified byte (the low eight bits of the argument
        * <code>b</code>) to the underlying output stream. If no exception
        * is thrown, the counter <code>written</code> is incremented by
        * <code>1</code>.
        * <p>
        * Implements the <code>write</code> method of <code>OutputStream</code>.
        *
        * @param      b   the <code>byte</code> to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void Write(int b)
        {
            @out.Write((byte)b);
            IncCount(1);
        }


        public void Write(long b)
        {
            WriteLong(b);
        }

        /**
        * Writes <code>len</code> bytes from the specified byte array
        * starting at offset <code>off</code> to the underlying output stream.
        * If no exception is thrown, the counter <code>written</code> is
        * incremented by <code>len</code>.
        *
        * @param      b     the data.
        * @param      off   the start offset in the data.
        * @param      len   the number of bytes to write.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void Write(byte[] b, int off, int len)
        {
            @out.Write(b, off, len);
            IncCount(len);
        }


        public void Write(byte[] b)
        {
            @out.Write(b);
            IncCount(b.Length);
        }

        /**
        * Flushes this data output stream. This forces any buffered output
        * bytes to be written out to the stream.
        * <p>
        * The <code>flush</code> method of <code>DataOutputStream</code>
        * calls the <code>flush</code> method of its underlying output stream.
        *
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        * @see        java.io.OutputStream#flush()
        */
        public void Flush()
        {
            @out.Flush();
        }

        /**
        * Writes a <code>boolean</code> to the underlying output stream as
        * a 1-byte value. The value <code>true</code> is written out as the
        * value <code>(byte)1</code>; the value <code>false</code> is
        * written out as the value <code>(byte)0</code>. If no exception is
        * thrown, the counter <code>written</code> is incremented by
        * <code>1</code>.
        *
        * @param      v   a <code>boolean</code> value to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteBoolean(bool v)
        {
            Write(v ? 1 : 0);
            IncCount(1);
        }

        /**
        * Writes out a <code>byte</code> to the underlying output stream as
        * a 1-byte value. If no exception is thrown, the counter
        * <code>written</code> is incremented by <code>1</code>.
        *
        * @param      v   a <code>byte</code> value to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteByte(int v)
        {
            @out.Write(v);
            IncCount(1);
        }

        /**
        * Writes a <code>short</code> to the underlying output stream as two
        * bytes, high byte first. If no exception is thrown, the counter
        * <code>written</code> is incremented by <code>2</code>.
        *
        * @param      v   a <code>short</code> to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteShort(int v)
        {
            Write((v >> 8) & 0xFF);
            Write((v >> 0) & 0xFF);
            IncCount(2);
        }

        /**
        * Writes a <code>char</code> to the underlying output stream as a
        * 2-byte value, high byte first. If no exception is thrown, the
        * counter <code>written</code> is incremented by <code>2</code>.
        *
        * @param      v   a <code>char</code> value to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteChar(int v)
        {
            Write((v >> 8) & 0xFF);
            Write((v >> 0) & 0xFF);
            IncCount(2);
        }

        /**
        * Writes an <code>int</code> to the underlying output stream as four
        * bytes, high byte first. If no exception is thrown, the counter
        * <code>written</code> is incremented by <code>4</code>.
        *
        * @param      v   an <code>int</code> to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteInt(int v)
        {
            Write((byte)((v >> 24) & 0xFF));
            Write((byte)((v >> 16) & 0xFF));
            Write((byte)((v >> 8) & 0xFF));
            Write((byte)((v >> 0) & 0xFF));
            IncCount(4);
        }


        private byte[] writeBuffer = new byte[8];

        /**
        * Writes a <code>long</code> to the underlying output stream as eight
        * bytes, high byte first. In no exception is thrown, the counter
        * <code>written</code> is incremented by <code>8</code>.
        *
        * @param      v   a <code>long</code> to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteLong(long v)
        {
            Write((byte)(((ulong)v) >> 56));
            Write((byte)(((ulong)v) >> 48));
            Write((byte)(((ulong)v) >> 40));
            Write((byte)(((ulong)v) >> 32));
            Write((byte)(((ulong)v) >> 24));
            Write((byte)(((ulong)v) >> 16));
            Write((byte)(((ulong)v) >> 8));
            Write((byte)(((ulong)v) >> 0));
            IncCount(8);
        }


        public unsafe static int FloatToUInt32Bits(float f)
        {
            return *(int*)&f;
        }

        /**
        * Converts the float argument to an <code>int</code> using the
        * <code>floatToIntBits</code> method in class <code>Float</code>,
        * and then writes that <code>int</code> value to the underlying
        * output stream as a 4-byte quantity, high byte first. If no
        * exception is thrown, the counter <code>written</code> is
        * incremented by <code>4</code>.
        *
        * @param      v   a <code>float</code> value to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        * @see        java.lang.Float#floatToIntBits(float)
        */
        public void WriteFloat(float v)
        {
            WriteInt(FloatToUInt32Bits(v));
        }

        /**
        * Converts the double argument to a <code>long</code> using the
        * <code>doubleToLongBits</code> method in class <code>Double</code>,
        * and then writes that <code>long</code> value to the underlying
        * output stream as an 8-byte quantity, high byte first. If no
        * exception is thrown, the counter <code>written</code> is
        * incremented by <code>8</code>.
        *
        * @param      v   a <code>double</code> value to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        * @see        java.lang.Double#doubleToLongBits(double)
        */
        public void WriteDouble(double v)
        {
            WriteLong(BitConverter.DoubleToInt64Bits(v));
        }

        /**
        * Writes out the string to the underlying output stream as a
        * sequence of bytes. Each character in the string is written out, in
        * sequence, by discarding its high eight bits. If no exception is
        * thrown, the counter <code>written</code> is incremented by the
        * length of <code>s</code>.
        *
        * @param      s   a string of bytes to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteBytes(String s)
        {
            int len = s.Length;
            for (int i = 0; i < len; i++) {
                @out.Write((byte)s[(i)]);
            }
            IncCount(len);
        }

        /**
        * Writes a string to the underlying output stream as a sequence of
        * characters. Each character is written to the data output stream as
        * if by the <code>writeChar</code> method. If no exception is
        * thrown, the counter <code>written</code> is incremented by twice
        * the length of <code>s</code>.
        *
        * @param      s   a <code>String</code> value to be written.
        * @exception  IOException  if an I/O error occurs.
        * @see        java.io.DataOutputStream#writeChar(int)
        * @see        java.io.FilterOutputStream#out
        */
        public void WriteChars(String s)
        {
            int len = s.Length;
            for (int i = 0; i < len; i++) {
                int v = s[(i)];
                @out.Write((v >> 8) & 0xFF);
                @out.Write((v >> 0) & 0xFF);
            }
            IncCount(len * 2);
        }


        /**
        * Writes a string to the underlying DataOutput using
        * <a href="DataInput.html#modified-utf-8">modified UTF-8</a>
        * encoding in a machine-independent manner.
        * <p>
        * First, two bytes are written to out as if by the <code>writeShort</code>
        * method giving the number of bytes to follow. This value is the number of
        * bytes actually written out, not the length of the string. Following the
        * length, each character of the string is output, in sequence, using the
        * modified UTF-8 encoding for the character. If no exception is thrown, the
        * counter <code>written</code> is incremented by the total number of
        * bytes written to the output stream. This will be at least two
        * plus the length of <code>str</code>, and at most two plus
        * thrice the length of <code>str</code>.
        *
        * @param      str   a string to be written.
        * @param      out   destination to write to
        * @return     The number of bytes written out.
        * @exception  IOException  if an I/O error occurs.
        */
        public int WriteUTF(String str)
        {
            int strlen = str.Length;
            int utflen = 0;
            int c, count = 0;

            /* use charAt instead of copying String to char array */
            for (int k = 0; k < strlen; k++) {
                c = str[(k)];
                if ((c >= 0x0001) && (c <= 0x007F)) {
                    utflen++;
                } else if (c > 0x07FF) {
                    utflen += 3;
                } else {
                    utflen += 2;
                }
            }

            if (utflen > 65535)
                throw new Exception(
                "encoded string too long: " + utflen + " bytes");

            DataOutputStream dos = this;
            if (dos.bytearr == null || (dos.bytearr.Length < (utflen + 2)))
                dos.bytearr = new byte[(utflen * 2) + 2];
            bytearr = dos.bytearr;

            bytearr[count++] = (byte)((utflen >> 8) & 0xFF);
            bytearr[count++] = (byte)((utflen >> 0) & 0xFF);

            int i = 0;
            for (i = 0; i < strlen; i++) {
                c = str[(i)];
                if (!((c >= 0x0001) && (c <= 0x007F))) break;
                bytearr[count++] = (byte)c;
            }

            for (; i < strlen; i++) {
                c = str[(i)];
                if ((c >= 0x0001) && (c <= 0x007F)) {
                    bytearr[count++] = (byte)c;

                } else if (c > 0x07FF) {
                    bytearr[count++] = (byte)(0xE0 | ((c >> 12) & 0x0F));
                    bytearr[count++] = (byte)(0x80 | ((c >> 6) & 0x3F));
                    bytearr[count++] = (byte)(0x80 | ((c >> 0) & 0x3F));
                } else {
                    bytearr[count++] = (byte)(0xC0 | ((c >> 6) & 0x1F));
                    bytearr[count++] = (byte)(0x80 | ((c >> 0) & 0x3F));
                }
            }
            @out.Write(bytearr, 0, utflen + 2);
            return utflen + 2;
        }

        /**
        * Returns the current value of the counter <code>written</code>,
        * the number of bytes written to this data output stream so far.
        * If the counter overflows, it will be wrapped to Integer.MAX_VALUE.
        *
        * @return  the value of the <code>written</code> field.
        * @see     java.io.DataOutputStream#written
        */
        public int Size()
        {
            return written;
        }

        public void Dispose()
        {
            @out.Dispose();
        }
    }
}
