namespace SmallPathTracer {
    public static class DoubleExtensions {
        // --- Public Static Methods ---
        public static double ToClosedUnitInterval(this double value) {
            return Clamp.ToClosedUnitInterval(value);
        }
    }
}