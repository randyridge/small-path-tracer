using System.Drawing.Imaging;
using System.Drawing;
using System;

namespace SmallPathTracer {
    public static class Program {
        // --- Private Static Methods ---
        private static double Clamp(double x) {
            return x < 0 ? 0 : x > 1 ? 1 : x;
        }

        private static int ToInt(double x) {
            return (int) (Math.Pow(Clamp(x), 1 / 2.2) * 255 + .5);
        }

        // --- Public Static Methods ---
        public static void Main(string[] args) {
            var width = int.Parse(args[0]); // width
            var height = int.Parse(args[1]); // height
            var samples = int.Parse(args[2]); // # samples
            var outputFileName = args[3];
            var renderer = new Renderer(width, height, samples, int.Parse(args[4]));
            var output = renderer.Render();

            var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for(var y = 0; y < height; y++) {
                for(var x = 0; x < width; x++) {
                    var i = (1 + y - 1) * width + x;
                    bitmap.SetPixel(x, y, Color.FromArgb(ToInt(output[i].X), ToInt(output[i].Y), ToInt(output[i].Z)));
                }
            }
            bitmap.Save(outputFileName, ImageFormat.Png);
        }
    }
}