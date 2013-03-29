using System;
using System.Diagnostics;
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
            var renderer = new Renderer(int.Parse(args[4], NumberStyles.Integer, CultureInfo.InvariantCulture));
            var stopwatch = Stopwatch.StartNew();
            var output = renderer.Render(width, height, samples);
            stopwatch.Stop();
            Console.WriteLine("Rendered {0}x{1}x{2} in {3:#,#}ms.", width, height, samples, stopwatch.ElapsedMilliseconds);
            BitmapWriter.Write(outputFileName, width, height, output);
        }
    }
}