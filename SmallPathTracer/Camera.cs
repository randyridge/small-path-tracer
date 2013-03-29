namespace SmallPathTracer {
    public sealed class Camera {
        // --- Private Readonly Fields ---
        private readonly Vector direction;
        private readonly Point position;

        // --- Public Constructors ---
        public Camera(Point position, Vector direction) {
            this.position = position;
            this.direction = direction;
        }

        // --- Public Properties ---
        public Vector Direction {
            get { return direction; }
        }

        public Point Position {
            get { return position; }
        }
    }
}