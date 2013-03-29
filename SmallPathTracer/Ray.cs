namespace SmallPathTracer {
    public sealed class Ray {
        // --- Public Constructors ---
        public Ray(Point origin, Vector direction) {
            Origin = origin;
            Direction = direction;
        }

        // --- Public Properties ---
        public Vector Direction { get; private set; }

        public Point Origin { get; private set; }
    }
}