using System;
using System.Linq;

namespace SmallPathTracer {
    public sealed class Renderer {
        // --- Private Readonly Fields ---
        private readonly Random random;
        private readonly Scene scene;

        // --- Public Constructors ---
        public Renderer(int randomSeed) {
            random = new Random(randomSeed);
            scene = new Scene();
        }

        // --- Private Methods ---
        private Vector CalculateDiffuseRadiance(int depth, Vector nl, Sphere obj, Color f, Point x) {
            var r1 = 2 * Math.PI * random.NextDouble();
            var r2 = random.NextDouble();
            var r2SquareRoot = Math.Sqrt(r2);
            var w = nl;
            var u = ((Math.Abs(w.X) > .1 ? Vector.UnitY : Vector.UnitX) % w).Normalize();
            var v = w % u;
            var d = (u * Math.Cos(r1) * r2SquareRoot + v * Math.Sin(r1) * r2SquareRoot + w * Math.Sqrt(1 - r2)).Normalize();
            return obj.Emission + f.Multiply(Radiance(new Ray(x, d), depth));
        }

        private Vector CalculateRefractionRadiance(Ray ray, int depth, Point x, Vector n, Vector nl, Sphere obj, Color f) {
            var reflRay = new Ray(x, ray.Direction - n * 2 * n.Dot(ray.Direction)); // Ideal dielectric REFRACTION
            var into = n.Dot(nl) > 0; // Ray from outside going in?
            const int Nc = 1;
            const double Nt = 1.5;
            var nnt = @into ? Nc / Nt : Nt / Nc;
            var ddn = ray.Direction.Dot(nl);

            double cos2T;
            if((cos2T = 1 - nnt * nnt * (1 - ddn * ddn)) < 0) { // Total internal reflection
                return obj.Emission + f.Multiply(Radiance(reflRay, depth));
            }

            var tdir = (ray.Direction * nnt - n * ((@into ? 1 : -1) * (ddn * nnt + Math.Sqrt(cos2T)))).Normalize();
            const double A = Nt - Nc;
            const double B = Nt + Nc;
            const double R0 = A * A / (B * B);
            var c = 1 - (@into ? -ddn : tdir.Dot(n));
            var re = R0 + (1 - R0) * c * c * c * c * c;
            var tr = 1 - re;
            var pp = .25 + .5 * re;
            var rp = re / pp;
            var tp = tr / (1 - pp);

            return obj.Emission + f.Multiply(depth > 2 ? (random.NextDouble() < pp ? // Russian roulette
                Radiance(reflRay, depth) * rp : Radiance(new Ray(x, tdir), depth) * tp) :
                Radiance(reflRay, depth) * re + Radiance(new Ray(x, tdir), depth) * tr);
        }

        private Vector CalculateSpecularRadiance(Ray ray, int depth, Sphere obj, Color f, Point x, Vector n) {
            return obj.Emission + f.Multiply(Radiance(new Ray(x, ray.Direction - n * 2 * n.Dot(ray.Direction)), depth));
        }

        private Vector Radiance(Ray ray, int depth) {
            var intersection = scene.Intersect(ray);
            if(intersection.IsMiss) {
                return Vector.Zero; // if miss, return black
            }

            var sphere = intersection.Sphere; // the hit object
            if(depth > 100) { // *** Added to prevent stack overflow
                return sphere.Emission;
            }

            var x = ray.Origin + ray.Direction * intersection.Distance;
            var n = (x - sphere.Position).Normalize();
            var nl = n.Dot(ray.Direction) < 0 ? n : n * -1;
            var color = sphere.Color;
            var p = color.Red > color.Green && color.Red > color.Blue ? color.Red : color.Green > color.Blue ? color.Green : color.Blue; // max ReflectionType

            if(++depth > 5) {
                if(random.NextDouble() < p) {
                    color = color * (1 / p);
                }
                else {
                    return sphere.Emission; //R.R.
                }
            }

            if(sphere.ReflectionType == ReflectionType.Diffuse) { // Ideal DIFFUSE reflection
                return CalculateDiffuseRadiance(depth, nl, sphere, color, x);
            }

            if(sphere.ReflectionType == ReflectionType.Specular) { // Ideal SPECULAR reflection
                return CalculateSpecularRadiance(ray, depth, sphere, color, x, n);
            }

            return CalculateRefractionRadiance(ray, depth, x, n, nl, sphere, color);
        }

        // --- Public Methods ---
        public Color[] Render(int width, int height, int samples) {
            var camera = new Camera(new Point(50, 52, 295.6), new Vector(0, -0.042612, -1).Normalize());
            var cx = new Vector(width * .5135 / height, 0, 0);
            var cy = (cx % camera.Direction).Normalize() * .5135;
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
                                var d = cx * (((sx + .5 + dx) / 2 + x) / width - .5) + cy * (((sy + .5 + dy) / 2 + y) / height - .5) + camera.Direction;
                                d = d.Normalize();
                                r = r + Radiance(new Ray(camera.Position + d * 140, d), 0) * (1.0 / samples);
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