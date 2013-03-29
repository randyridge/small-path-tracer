namespace SmallPathTracer {
    public struct Point {
        // --- Private Readonly Fields ---
        private readonly double x;
        private readonly double y;
        private readonly double z;

        // --- Public Constructors ---
        public Point(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // --- Public Properties ---
        public double X {
            get { return x; }
        }

        public double Y {
            get { return y; }
        }

        public double Z {
            get { return z; }
        }

        // --- Public Static Operators ---
        public static Vector operator -(Point finish, Point start) {
            return new Vector(finish.X - start.X, finish.Y - start.Y, finish.Z - start.Z);
        }

        public static Point operator +(Point point, Vector vector) {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }
    }
}