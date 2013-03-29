using System;

namespace SmallPathTracer {
    public sealed class Vector {
        // --- Public Static Readonly Fields ---
        public static readonly Vector Zero = new Vector(0, 0, 0);

        // --- Public Constructors ---
        public Vector(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
        }

        // --- Public Properties ---
        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }

        // --- Public Static Operators ---
        public static Vector operator %(Vector left, Vector right) {
            return new Vector(left.Y * right.Z - left.Z * right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
        }

        public static Vector operator -(Vector left, Vector right) {
            return new Vector(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector operator *(Vector left, double right) {
            return new Vector(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector operator +(Vector left, Vector right) {
            return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        // --- Public Methods ---
        public double Dot(Vector vector) {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        public Vector Normalize() {
            return this * (1 / Math.Sqrt(X * X + Y * Y + Z * Z));
        }
    }
}