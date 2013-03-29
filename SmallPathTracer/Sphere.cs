using System;

namespace SmallPathTracer {
    public struct Sphere {
        // --- Private Readonly Fields ---
        private readonly Color color;
        private readonly Vector emission;
        private readonly Point position;
        private readonly double radius;
        private readonly ReflectionType reflectionType;

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

        // --- Public Methods ---
        public double Intersect(Ray ray) { // returns distance, 0 if nohit
            var op = Position - ray.Origin; // Solve t^2*Direction.Direction + 2*t*(Origin-Position).Direction + (Origin-Position).(Origin-Position)-R^2 = 0
            double t;
            const double Epsilon = 1e-4;
            var b = op.Dot(ray.Direction);
            var det = b * b - op.Dot(op) + radius * radius;
            if(det < 0) {
                return 0;
            }
            det = Math.Sqrt(det);
            return (t = b - det) > Epsilon ? t : ((t = b + det) > Epsilon ? t : 0);
        }
    }
}