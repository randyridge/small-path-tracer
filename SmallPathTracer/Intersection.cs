namespace SmallPathTracer {
    public struct Intersection {
        // --- Private Readonly Fields ---
        private readonly double distance;
        private readonly Ray ray;
        private readonly Sphere sphere;

        // --- Public Static Readonly Fields ---
        public static readonly Intersection Miss = new Intersection(Ray.Zero, Sphere.Unknown, double.PositiveInfinity);

        // --- Public Constructors ---
        public Intersection(Ray ray, Sphere sphere, double distance) {
            this.ray = ray;
            this.sphere = sphere;
            this.distance = distance;
        }

        // --- Public Properties ---
        public double Distance {
            get { return distance; }
        }

        public bool IsHit {
            get { return this != Miss; }
        }

        public bool IsMiss {
            get { return this == Miss; }
        }

        public Sphere Sphere {
            get { return sphere; }
        }

        // --- Public Static Operators ---
        public static bool operator !=(Intersection left, Intersection right) {
            return !(left == right);
        }

        public static bool operator ==(Intersection left, Intersection right) {
            return left.distance == right.distance && left.ray == right.ray && left.sphere == right.sphere;
        }

        // --- Public Methods ---
        public bool IsCloserThan(Intersection intersection) {
            return distance < intersection.distance;
        }
    }
}