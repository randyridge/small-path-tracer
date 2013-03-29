namespace SmallPathTracer {
    public sealed class Color {
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
    }
}