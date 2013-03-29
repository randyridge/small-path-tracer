using System;
using System.Linq;

namespace SmallPathTracer {
    public sealed class Renderer {
        // --- Private Static Readonly Fields ---
        private static readonly Sphere[] spheres = new[] { //Scene: radius, position, emission, color, material 
            new Sphere(1e5, new Point(1e5 + 1, 40.8, 81.6), Vector.Zero, new Color(.75, .25, .25), ReflectionType.Diffuse), //Left
            new Sphere(1e5, new Point(-1e5 + 99, 40.8, 81.6), Vector.Zero, new Color(.25, .25, .75), ReflectionType.Diffuse), //Rght
            new Sphere(1e5, new Point(50, 40.8, 1e5), Vector.Zero, new Color(.75, .75, .75), ReflectionType.Diffuse), //Back
            new Sphere(1e5, new Point(50, 40.8, -1e5 + 170), Vector.Zero, Color.Black, ReflectionType.Diffuse), //Frnt
            new Sphere(1e5, new Point(50, 1e5, 81.6), Vector.Zero, new Color(.75, .75, .75), ReflectionType.Diffuse), //Botm
            new Sphere(1e5, new Point(50, -1e5 + 81.6, 81.6), Vector.Zero, new Color(.75, .75, .75), ReflectionType.Diffuse), //Top
            new Sphere(16.5, new Point(27, 16.5, 47), Vector.Zero, new Color(.999, .999, .999), ReflectionType.Specular), //Mirr
            new Sphere(16.5, new Point(73, 16.5, 78), Vector.Zero, new Color(.999, .999, .999), ReflectionType.Refractive), //Glas
            new Sphere(600, new Point(50, 681.6 - .27, 81.6), new Vector(12, 12, 12), Color.Black, ReflectionType.Diffuse) //Lite
        };

        // --- Private Readonly Fields ---
        private readonly Random random;

        // --- Public Constructors ---
        public Renderer(int randomSeed) {
            random = new Random(randomSeed);
        }

        // --- Private Static Methods ---
        private static Intersection Intersect(Ray ray) {
            var intersection = Intersection.Miss;
            foreach(var sphere in spheres) {
                var sphereIntersection = sphere.Intersect(ray);
                if(sphereIntersection.IsHit && sphereIntersection.IsCloserThan(intersection)) {
                    intersection = sphereIntersection;
                }
            }
            return intersection;
        }

        // --- Private Methods ---
        private Vector Radiance(Ray ray, int depth) {
            var intersection = Intersect(ray);
            if(intersection.IsMiss) {
                return Vector.Zero; // if miss, return black
            }
            var obj = intersection.Sphere; // the hit object
            Point x = ray.Origin + ray.Direction * intersection.Distance;
            Vector n = (x - obj.Position).Normalize();
            Vector nl = n.Dot(ray.Direction) < 0 ? n : n * -1;
            var f = obj.Color;
            var p = f.Red > f.Green && f.Red > f.Blue ? f.Red : f.Green > f.Blue ? f.Green : f.Blue; // max ReflectionType

            if(depth > 100) {
                return obj.Emission; // *** Added to prevent stack overflow
            }

            if(++depth > 5) {
                if(random.NextDouble() < p) {
                    f = f * (1 / p);
                }
                else {
                    return obj.Emission; //R.R.
                }
            }

            if(obj.ReflectionType == ReflectionType.Diffuse) { // Ideal DIFFUSE reflection
                var r1 = 2 * Math.PI * random.NextDouble();
                var r2 = random.NextDouble();
                var r2SquareRoot = Math.Sqrt(r2);
                var w = nl;
                var u = ((Math.Abs(w.X) > .1 ? Vector.UnitY : Vector.UnitX) % w).Normalize();
                var v = w % u;
                var d = (u * Math.Cos(r1) * r2SquareRoot + v * Math.Sin(r1) * r2SquareRoot + w * Math.Sqrt(1 - r2)).Normalize();
                return obj.Emission + f.Multiply(Radiance(new Ray(x, d), depth));
            }

            if(obj.ReflectionType == ReflectionType.Specular) { // Ideal SPECULAR reflection
                return obj.Emission + f.Multiply(Radiance(new Ray(x, ray.Direction - n * 2 * n.Dot(ray.Direction)), depth));
            }

            var reflRay = new Ray(x, ray.Direction - n * 2 * n.Dot(ray.Direction)); // Ideal dielectric REFRACTION
            var into = n.Dot(nl) > 0; // Ray from outside going in?
            const int Nc = 1;
            const double Nt = 1.5;
            var nnt = into ? Nc / Nt : Nt / Nc;
            var ddn = ray.Direction.Dot(nl);

            double cos2T;
            if((cos2T = 1 - nnt * nnt * (1 - ddn * ddn)) < 0) { // Total internal reflection
                return obj.Emission + f.Multiply(Radiance(reflRay, depth));
            }

            var tdir = (ray.Direction * nnt - n * ((into ? 1 : -1) * (ddn * nnt + Math.Sqrt(cos2T)))).Normalize();
            const double A = Nt - Nc;
            const double B = Nt + Nc;
            const double R0 = A * A / (B * B);
            var c = 1 - (into ? -ddn : tdir.Dot(n));
            var re = R0 + (1 - R0) * c * c * c * c * c;
            var tr = 1 - re;
            var pp = .25 + .5 * re;
            var rp = re / pp;
            var tp = tr / (1 - pp);

            return obj.Emission + f.Multiply(depth > 2 ? (random.NextDouble() < pp ? // Russian roulette
                Radiance(reflRay, depth) * rp : Radiance(new Ray(x, tdir), depth) * tp) :
                Radiance(reflRay, depth) * re + Radiance(new Ray(x, tdir), depth) * tr);
        }

        // --- Public Methods ---
        public Color[] Render(int width, int height, int samples) {
            var cam = new Ray(new Point(50, 52, 295.6), new Vector(0, -0.042612, -1).Normalize()); // cam pos, dir
            var cx = new Vector(width * .5135 / height, 0, 0);
            var cy = (cx % cam.Direction).Normalize() * .5135;
            var colors = Enumerable.Repeat(Color.Black, width * height).ToArray();
            for(var y = 0; y < height; y++) { // Loop over image rows
                //Console.Write("\rRendering ({0} spp) {1:0.00}%",samps*4, 100.*Y/(h-1));
                for(var x = 0; x < width; x++) { // Loop cols
                    for(int sy = 0, i = (height - y - 1) * width + x; sy < 2; sy++) { // 2x2 subpixel rows
                        for(var sx = 0; sx < 2; sx++) { // 2x2 subpixel cols
                            var r = Vector.Zero;
                            for(var s = 0; s < samples; s++) {
                                var r1 = 2 * random.NextDouble();
                                var dx = r1 < 1 ? Math.Sqrt(r1) - 1 : 1 - Math.Sqrt(2 - r1);
                                var r2 = 2 * random.NextDouble();
                                var dy = r2 < 1 ? Math.Sqrt(r2) - 1 : 1 - Math.Sqrt(2 - r2);
                                var d = cx * (((sx + .5 + dx) / 2 + x) / width - .5) + cy * (((sy + .5 + dy) / 2 + y) / height - .5) + cam.Direction;
                                d = d.Normalize();
                                r = r + Radiance(new Ray(cam.Origin + d * 140, d), 0) * (1.0 / samples);
                            } // Camera rays are pushed ^^^^^ forward to start in interior
                            colors[i] = colors[i] + new Color(r.X.ToClosedUnitInterval(), r.Y.ToClosedUnitInterval(), r.Z.ToClosedUnitInterval()) * .25;
                        }
                    }
                }
            }
            return colors;
        }
    }
}