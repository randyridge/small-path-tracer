namespace SmallPathTracer {
    public sealed class Ray {
        // --- Public Readonly Fields ---
        public readonly Vector Direction;
        public readonly Vector Origin;

        // --- Public Constructors ---
        public Ray(Vector origin, Vector direction) {
            Origin = origin;
            Direction = direction;
        }
    }
}