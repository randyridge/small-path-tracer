namespace SmallPathTracer {
    public sealed class Point {
        // --- Public Constructors ---
        public Point(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
        }

        // --- Public Properties ---
        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }
    }
}