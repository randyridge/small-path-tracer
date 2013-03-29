using System.Drawing.Imaging;
using System.Drawing;
using System;

namespace SmallPathTracer {
    public static class BitmapWriter {
        // --- Private Static Methods ---
        private static int ToInt(double x) {
            return (int) (Math.Pow(x.ToClosedUnitInterval(), 1 / 2.2) * 255 + .5);
        }

        // --- Public Static Methods ---
        public static void Write(string fileName, int width, int height, Color[] pixelData) {
            using(var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb)) {
                for(var y = 0; y < height; y++) {
                    for(var x = 0; x < width; x++) {
                        var i = (1 + y - 1) * width + x;
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(ToInt(pixelData[i].Red), ToInt(pixelData[i].Green), ToInt(pixelData[i].Blue)));
                    }
                }
                bitmap.Save(fileName, ImageFormat.Png);
            }
        }
    }
}