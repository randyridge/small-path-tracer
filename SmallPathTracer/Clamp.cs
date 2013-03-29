namespace SmallPathTracer {
    public static class Clamp {
        // --- Public Static Methods ---
        public static double ToClosedUnitInterval(double value) {
            return ToRange(value, 0, 1);
        }

        public static double ToRange(double value, double minimum, double maximum) {
            return (value < minimum) ? minimum : ((value > maximum) ? maximum : value);
        }
    }
}