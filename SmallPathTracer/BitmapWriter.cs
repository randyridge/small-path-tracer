using System.Drawing.Imaging;
using System.Drawing;
using System;

namespace SmallPathTracer {
    public static class BitmapWriter {
        // --- Private Static Methods ---
        private static double Clamp(double x) {
            return x < 0 ? 0 : x > 1 ? 1 : x;
        }

        private static int ToInt(double x) {
            return (int) (Math.Pow(Clamp(x), 1 / 2.2) * 255 + .5);
        }

        // --- Public Static Methods ---
        public static void Write(string fileName, int width, int height, Vector[] data) {
            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for(var y = 0; y < height; y++) {
                for(var x = 0; x < width; x++) {
                    var i = (1 + y - 1) * width + x;
                    bitmap.SetPixel(x, y, Color.FromArgb(ToInt(data[i].X), ToInt(data[i].Y), ToInt(data[i].Z)));
                }
            }
            bitmap.Save(fileName, ImageFormat.Png);
        }
    }
}