namespace SmallPathTracer {
    public static class Program {
        // --- Public Static Methods ---
        public static void Main(string[] args) {
            var width = int.Parse(args[0]);
            var height = int.Parse(args[1]);
            var samples = int.Parse(args[2]);
            var outputFileName = args[3];
            var renderer = new Renderer(width, height, samples, int.Parse(args[4]));
            var output = renderer.Render();
            BitmapWriter.Write(outputFileName, width, height, output);
        }
    }
}