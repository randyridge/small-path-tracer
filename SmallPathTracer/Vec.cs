using System;

namespace SmallPathTracer {
    public class Vec {
        public double x, y, z; // position, also color (r,g,b)

        public Vec(double x_ = 0, double y_ = 0, double z_ = 0) {
            x = x_;
            y = y_;
            z = z_;
        }

        public static Vec operator +(Vec a, Vec b) {
            return new Vec(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec operator -(Vec a, Vec b) {
            return new Vec(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec operator *(Vec a, double b) {
            return new Vec(a.x * b, a.y * b, a.z * b);
        }

        public Vec mult(Vec b) {
            return new Vec(x * b.x, y * b.y, z * b.z);
        }

        public Vec norm() {
            return this * (1 / Math.Sqrt(x * x + y * y + z * z));
        }

        public double dot(Vec b) {
            return x * b.x + y * b.y + z * b.z;
        }

        // cross:
        public static Vec operator %(Vec a, Vec b) {
            return new Vec(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }
    }
}