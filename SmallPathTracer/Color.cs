namespace SmallPathTracer {
    public sealed class Color {
        // --- Public Static Readonly Fields ---
        public static readonly Color Black = new Color(0, 0, 0);

        // --- Public Constructors ---
        public Color(double red, double green, double blue) {
            Red = red;
            Green = green;
            Blue = blue;
        }

        // --- Public Properties ---
        public double Blue { get; private set; }

        public double Green { get; private set; }

        public double Red { get; private set; }

        // --- Public Static Operators ---
        public static Color operator *(Color color, double scalar) {
            return new Color(color.Red * scalar, color.Green * scalar, color.Blue * scalar);
        }

        public static Color operator +(Color left, Color right) {
            return new Color(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue);
        }

        // --- Public Methods ---
        public Vector Multiply(Vector vector) {
            // This is totally wrong, but here temporarily for refactoring...
            return new Vector(Red * vector.X, Green * vector.Y, Blue * vector.Z);
        }
    }
}