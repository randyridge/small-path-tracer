using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace SmallPathTracer {
    public static class Program {
        private static Random random;
        private static Sphere[] spheres = new[] { //Scene: radius, position, emission, color, material 
            new Sphere(1e5, new Vector(1e5 + 1, 40.8, 81.6), new Vector(), new Vector(.75, .25, .25), ReflectionType.Diffuse), //Left
            new Sphere(1e5, new Vector(-1e5 + 99, 40.8, 81.6), new Vector(), new Vector(.25, .25, .75), ReflectionType.Diffuse), //Rght
            new Sphere(1e5, new Vector(50, 40.8, 1e5), new Vector(), new Vector(.75, .75, .75), ReflectionType.Diffuse), //Back
            new Sphere(1e5, new Vector(50, 40.8, -1e5 + 170), new Vector(), new Vector(), ReflectionType.Diffuse), //Frnt
            new Sphere(1e5, new Vector(50, 1e5, 81.6), new Vector(), new Vector(.75, .75, .75), ReflectionType.Diffuse), //Botm
            new Sphere(1e5, new Vector(50, -1e5 + 81.6, 81.6), new Vector(), new Vector(.75, .75, .75), ReflectionType.Diffuse), //Top
            new Sphere(16.5, new Vector(27, 16.5, 47), new Vector(), new Vector(1, 1, 1) * .999, ReflectionType.Specular), //Mirr
            new Sphere(16.5, new Vector(73, 16.5, 78), new Vector(), new Vector(1, 1, 1) * .999, ReflectionType.Refractive), //Glas
            new Sphere(600, new Vector(50, 681.6 - .27, 81.6), new Vector(12, 12, 12), new Vector(), ReflectionType.Diffuse) //Lite
        };

        private static double clamp(double x) {
            return x < 0 ? 0 : x > 1 ? 1 : x;
        }

        private static int toInt(double x) {
            return (int) (Math.Pow(clamp(x), 1 / 2.2) * 255 + .5);
        }

        private static bool intersect(Ray r, ref double t, ref int id) {
            double inf = t = 1e20, d;
            for(int i = spheres.Length - 1; i >= 0; i--) {
                d = spheres[i].Intersect(r);
                if(d > 0 && d < t) {
                    t = d;
                    id = i;
                }
            }
            return t < inf;
        }

        private static Vector radiance(Ray r, int depth) {
            double t = 0; // distance to intersection
            int id = 0; // id of intersected object
            if(!intersect(r, ref t, ref id))
                return new Vector(); // if miss, return black
            Sphere obj = spheres[id]; // the hit object
            Vector x = r.Origin + r.Direction * t, n = (x - obj.Position).norm(), nl = n.dot(r.Direction) < 0 ? n : n * -1, f = obj.Color;
            double p = f.X > f.Y && f.X > f.Z ? f.X : f.Y > f.Z ? f.Y : f.Z; // max ReflectionType
            if(depth > 100)
                return obj.Emission; // *** Added to prevent stack overflow
            if(++depth > 5)
                if(random.NextDouble() < p)
                    f = f * (1 / p);
                else
                    return obj.Emission; //R.R.
            if(obj.ReflectionType == ReflectionType.Diffuse) { // Ideal DIFFUSE reflection
                double r1 = 2 * Math.PI * random.NextDouble(), r2 = random.NextDouble(), r2s = Math.Sqrt(r2);
                Vector w = nl, u = ((Math.Abs(w.X) > .1 ? new Vector(0, 1) : new Vector(1)) % w).norm(), v = w % u;
                Vector d = (u * Math.Cos(r1) * r2s + v * Math.Sin(r1) * r2s + w * Math.Sqrt(1 - r2)).norm();
                return obj.Emission + f.mult(radiance(new Ray(x, d), depth));
            }
            else if(obj.ReflectionType == ReflectionType.Specular) // Ideal SPECULAR reflection
                return obj.Emission + f.mult(radiance(new Ray(x, r.Direction - n * 2 * n.dot(r.Direction)), depth));
            Ray reflRay = new Ray(x, r.Direction - n * 2 * n.dot(r.Direction)); // Ideal dielectric REFRACTION
            bool into = n.dot(nl) > 0; // Ray from outside going in?
            double nc = 1, nt = 1.5, nnt = into ? nc / nt : nt / nc, ddn = r.Direction.dot(nl), cos2t;
            if((cos2t = 1 - nnt * nnt * (1 - ddn * ddn)) < 0) // Total internal reflection
                return obj.Emission + f.mult(radiance(reflRay, depth));
            Vector tdir = (r.Direction * nnt - n * ((into ? 1 : -1) * (ddn * nnt + Math.Sqrt(cos2t)))).norm();
            double a = nt - nc, b = nt + nc, R0 = a * a / (b * b), c = 1 - (into ? -ddn : tdir.dot(n));
            double Re = R0 + (1 - R0) * c * c * c * c * c, Tr = 1 - Re, P = .25 + .5 * Re, RP = Re / P, TP = Tr / (1 - P);
            return obj.Emission + f.mult(depth > 2 ? (random.NextDouble() < P ? // Russian roulette
                radiance(reflRay, depth) * RP : radiance(new Ray(x, tdir), depth) * TP) :
                radiance(reflRay, depth) * Re + radiance(new Ray(x, tdir), depth) * Tr);
        }

        public static void Main(string[] args) {
            int w = int.Parse(args[0]); // width
            int h = int.Parse(args[1]); // height
            int samps = int.Parse(args[2]); // # samples
            string outputFileName = args[3];
            random = new Random(int.Parse(args[4]));
            Ray cam = new Ray(new Vector(50, 52, 295.6), new Vector(0, -0.042612, -1).norm()); // cam pos, dir
            Vector cx = new Vector(w * .5135 / h), cy = (cx % cam.Direction).norm() * .5135;
            var c = Enumerable.Repeat(new Vector(), w * h).ToArray();
            for(int y = 0; y < h; y++) { // Loop over image rows
                //Console.Write("\rRendering ({0} spp) {1:0.00}%",samps*4, 100.*Y/(h-1));
                for(int x = 0; x < w; x++) { // Loop cols
                    for(int sy = 0, i = (h - y - 1) * w + x; sy < 2; sy++) { // 2x2 subpixel rows
                        for(int sx = 0; sx < 2; sx++) { // 2x2 subpixel cols
                            Vector r = new Vector();
                            for(int s = 0; s < samps; s++) {
                                double r1 = 2 * random.NextDouble(), dx = r1 < 1 ? Math.Sqrt(r1) - 1 : 1 - Math.Sqrt(2 - r1);
                                double r2 = 2 * random.NextDouble(), dy = r2 < 1 ? Math.Sqrt(r2) - 1 : 1 - Math.Sqrt(2 - r2);
                                Vector d = cx * (((sx + .5 + dx) / 2 + x) / w - .5) +
                                    cy * (((sy + .5 + dy) / 2 + y) / h - .5) + cam.Direction;
                                d = d.norm();
                                r = r + radiance(new Ray(cam.Origin + d * 140, d), 0) * (1.0 / samps);
                            } // Camera rays are pushed ^^^^^ forward to start in interior
                            c[i] = c[i] + new Vector(clamp(r.X), clamp(r.Y), clamp(r.Z)) * .25;
                        }
                    }
                }
            }
            var bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            for(var y = 0; y < h; y++) {
                for(var x = 0; x < w; x++) {
                    var i = (1 + y - 1) * w + x;
                    bitmap.SetPixel(x, y, Color.FromArgb(toInt(c[i].X), toInt(c[i].Y), toInt(c[i].Z)));
                }
            }
            bitmap.Save(outputFileName, ImageFormat.Png);
        }
    }
}