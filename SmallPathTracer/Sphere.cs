using System;

namespace SmallPathTracer {
    public class Sphere {
        public double rad; // radius
        public Vec p, e, c; // position, emission, color
        public Refl_t refl; // reflection type (DIFFuse, SPECular, REFRactive)

        public Sphere(double rad_, Vec p_, Vec e_, Vec c_, Refl_t refl_) {
            rad = rad_;
            p = p_;
            e = e_;
            c = c_;
            refl = refl_;
        }

        public double intersect(Ray r) { // returns distance, 0 if nohit
            Vec op = p - r.o; // Solve t^2*d.d + 2*t*(o-p).d + (o-p).(o-p)-R^2 = 0
            double t, eps = 1e-4, b = op.dot(r.d), det = b * b - op.dot(op) + rad * rad;
            if(det < 0)
                return 0;
            else
                det = Math.Sqrt(det);
            return (t = b - det) > eps ? t : ((t = b + det) > eps ? t : 0);
        }
    }
}