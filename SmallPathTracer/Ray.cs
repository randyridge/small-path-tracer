namespace SmallPathTracer {
    public struct Ray {
        // --- Private Readonly Fields ---
        private readonly Vector direction;
        private readonly Point origin;

        // --- Public Static Readonly Fields ---
        public static readonly Ray Zero = new Ray(Point.Zero, Vector.Zero);

        // --- Public Constructors ---
        public Ray(Point origin, Vector direction) {
            this.origin = origin;
            this.direction = direction;
        }

        // --- Public Properties ---
        public Vector Direction {
            get { return direction; }
        }

        public Point Origin {
            get { return origin; }
        }

        // --- Public Static Operators ---
        public static bool operator !=(Ray left, Ray right) {
            return !(left == right);
        }

        public static bool operator ==(Ray left, Ray right) {
            return left.direction == right.direction && left.origin == right.origin;
        }
    }
}