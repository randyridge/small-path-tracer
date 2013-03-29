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

        // --- Public Static Operators ---
        public static Vector operator -(Point finish, Point start) {
            return new Vector(finish.X - start.X, finish.Y - start.Y, finish.Z - start.Z);
        }

        public static Point operator +(Point point, Vector vector) {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }
    }
}