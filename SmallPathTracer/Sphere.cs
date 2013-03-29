using System;

namespace SmallPathTracer {
    public struct Sphere {
        // --- Private Readonly Fields ---
        private readonly Color color;
        private readonly Vector emission;
        private readonly Point position;
        private readonly double radius;
        private readonly ReflectionType reflectionType;

        // --- Public Static Readonly Fields ---
        public static readonly Sphere Unknown = new Sphere(0, Point.Zero, Vector.Zero, Color.Black, ReflectionType.Unknown);

        // --- Public Constructors ---
        public Sphere(double radius, Point position, Vector emission, Color color, ReflectionType reflectionType) {
            this.radius = radius;
            this.position = position;
            this.emission = emission;
            this.color = color;
            this.reflectionType = reflectionType;
        }

        // --- Public Properties ---
        public Color Color {
            get { return color; }
        }

        public Vector Emission {
            get { return emission; }
        }

        public Point Position {
            get { return position; }
        }

        public ReflectionType ReflectionType {
            get { return reflectionType; }
        }

        // --- Public Static Operators ---
        public static bool operator !=(Sphere left, Sphere right) {
            return !(left == right);
        }

        public static bool operator ==(Sphere left, Sphere right) {
            return
                left.color == right.color &&
                left.emission == right.emission &&
                left.position == right.position &&
                left.radius == right.radius &&
                left.reflectionType == right.reflectionType;
        }

        // --- Public Methods ---
        public Intersection Intersect(Ray ray) {
            var op = Position - ray.Origin; // Solve t^2*Direction.Direction + 2*t*(Origin-Position).Direction + (Origin-Position).(Origin-Position)-R^2 = 0
            double t;
            const double Epsilon = 1e-4;
            var b = op.Dot(ray.Direction);
            var det = b * b - op.Dot(op) + radius * radius;
            if(det < 0) {
                return Intersection.Miss;
            }
            det = Math.Sqrt(det);
            return (t = b - det) > Epsilon ? new Intersection(ray, this, t) : ((t = b + det) > Epsilon ? new Intersection(ray, this, t) : Intersection.Miss);
        }
    }
}