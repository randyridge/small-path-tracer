namespace SmallPathTracer {
    public sealed class Scene {
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

        // --- Public Methods ---
        public Intersection Intersect(Ray ray) {
            var intersection = Intersection.Miss;
            foreach(var sphere in spheres) {
                var sphereIntersection = sphere.Intersect(ray);
                if(sphereIntersection.IsHit && sphereIntersection.IsCloserThan(intersection)) {
                    intersection = sphereIntersection;
                }
            }
            return intersection;
        }
    }
}