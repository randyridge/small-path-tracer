using System;

namespace SmallPathTracer {
    public sealed class Vector {
        // --- Public Readonly Fields ---
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        // --- Public Constructors ---
        public Vector(double x = 0, double y = 0, double z = 0) {
            X = x;
            Y = y;
            Z = z;
        }

        // --- Public Static Operators ---
        public static Vector operator %(Vector a, Vector b) {
            return new Vector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static Vector operator -(Vector a, Vector b) {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector operator *(Vector a, double b) {
            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector operator +(Vector a, Vector b) {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        // --- Public Methods ---
        public double dot(Vector b) {
            return X * b.X + Y * b.Y + Z * b.Z;
        }

        public Vector mult(Vector b) {
            return new Vector(X * b.X, Y * b.Y, Z * b.Z);
        }

        public Vector norm() {
            return this * (1 / Math.Sqrt(X * X + Y * Y + Z * Z));
        }
    }
}