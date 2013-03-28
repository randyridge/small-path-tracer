using System;
using System.Globalization;

namespace SmallPathTracer {
    public static class Program {
        // --- Public Static Methods ---
        public static void Main(string[] args) {
            if(args == null || args.Length != 5) {
                Console.WriteLine("Usage: SmallPathTracer.exe width height samples filename seed");
                return;
            }

            var width = int.Parse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            var height = int.Parse(args[1], NumberStyles.Integer, CultureInfo.InvariantCulture);
            var samples = int.Parse(args[2], NumberStyles.Integer, CultureInfo.InvariantCulture);
            var outputFileName = args[3];
            var renderer = new Renderer(width, height, samples, int.Parse(args[4], NumberStyles.Integer, CultureInfo.InvariantCulture));
            var output = renderer.Render();
            BitmapWriter.Write(outputFileName, width, height, output);
        }
    }
}