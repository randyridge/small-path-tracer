using System;
using System.Diagnostics.Contracts;

namespace SmallPathTracer {
    public sealed class Sphere {
        // --- Private Readonly Fields ---
        private readonly double radius;

        // --- Public Constructors ---
        public Sphere(double radius, Vector position, Vector emission, Vector color, ReflectionType reflectionType) {
            this.radius = radius;
            Position = position;
            Emission = emission;
            Color = color;
            ReflectionType = reflectionType;
        }

        // --- Public Properties ---
        public Vector Color { get; private set; }

        public Vector Emission { get; private set; }

        public Vector Position { get; private set; }

        public ReflectionType ReflectionType { get; private set; }

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