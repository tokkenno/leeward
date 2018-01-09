using System;
using System.ComponentModel;
using System.Linq;

namespace Leeward.Image
{
    public class Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }

        public Color() : this(0x00, 0x00, 0x00)
        {
        }

        public Color(byte r, byte g, byte b, byte a = 0x00)
        {
            this.Red = r;
            this.Green = g;
            this.Blue = b;
            this.Alpha = a;
        }

        public float RedPercentaje
        {
            get => byte.MaxValue / (float)Red;
            set => Red = (byte) ((int) byte.MaxValue * (value % 1));
        }

        public float GreenPercentaje
        {
            get => byte.MaxValue / (float)Green;
            set => Green = (byte) ((int) byte.MaxValue * (value % 1));
        }

        public float BluePercentaje
        {
            get => byte.MaxValue / (float)Blue;
            set => Blue = (byte) ((int) byte.MaxValue * (value % 1));
        }

        public float AlphaPercentaje
        {
            get => byte.MaxValue / (float)Alpha;
            set => Alpha = (byte) ((int) byte.MaxValue * (value % 1));
        }
        
        /// <summary>
        /// Set colors by percentaje (Between 0.0 and 1.0)
        /// </summary>
        /// <param name="r">Red color %</param>
        /// <param name="g">Green color %</param>
        /// <param name="b">Blue color %</param>
        /// <param name="a">Alpha channel %</param>
        public static Color FromPercentaje(float r, float g, float b, float a = 0)
        {
            return new Color
            {
                RedPercentaje = r,
                GreenPercentaje = g,
                BluePercentaje = b,
                AlphaPercentaje = a
            };
        }
    }
}