using System;

namespace SmallPathTracer {
    public struct Vector {
        // --- Private Readonly Fields ---
        private readonly double x;
        private readonly double y;
        private readonly double z;

        // --- Public Static Readonly Fields ---
        public static readonly Vector UnitX = new Vector(1, 0, 0);
        public static readonly Vector UnitY = new Vector(0, 1, 0);
        public static readonly Vector Zero = new Vector(0, 0, 0);

        // --- Public Constructors ---
        public Vector(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // --- Public Properties ---
        public double X {
            get { return x; }
        }

        public double Y {
            get { return y; }
        }

        public double Z {
            get { return z; }
        }

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