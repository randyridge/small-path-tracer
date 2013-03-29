namespace SmallPathTracer {
    public struct Color {
        // --- Private Readonly Fields ---
        private readonly double blue;
        readonly private double green;
        private readonly double red;

        // --- Public Static Readonly Fields ---
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(1, 1, 1);

        // --- Public Constructors ---
        public Color(double red, double green, double blue) {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        // --- Public Properties ---
        public double Blue {
            get { return blue; }
        }

        public double Green {
            get { return green; }
        }

        public double Red {
            get { return red; }
        }

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