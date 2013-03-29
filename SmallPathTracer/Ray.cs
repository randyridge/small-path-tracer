namespace SmallPathTracer {
    public struct Ray {
        // --- Private Readonly Fields ---
        private readonly Vector direction;
        private readonly Point origin;

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
    }
}