namespace SmallPathTracer {
    public sealed class Ray {
        // --- Public Constructors ---
        public Ray(Vector origin, Vector direction) {
            Origin = origin;
            Direction = direction;
        }

        // --- Public Properties ---
        public Vector Direction { get; private set; }

        public Vector Origin { get; private set; }
    }
}