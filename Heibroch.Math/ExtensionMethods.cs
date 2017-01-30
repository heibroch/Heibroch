namespace Heibroch.Math
{
    public static class ExtensionMethods
    {
        public static bool IsEqualTo(this float value1, float value2)
        {
            return System.Math.Abs(value1 - value2) < float.Epsilon;
        }

        public static bool IsEqualTo(this double value1, double value2)
        {
            return System.Math.Abs(value1 - value2) < double.Epsilon;
        }

        public static bool FindIntersection(Line2D line1, Line2D line2, out Vector2D intersection)
        {
            /* http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
             * Now there are five cases:
             * If r × s = 0 and (q − p) × r = 0, then the two lines are collinear. If in addition, either 0 ≤ (q − p) · r ≤ r · r or 0 ≤ (p − q) · s ≤ s · s, then the two lines are overlapping.
             * If r × s = 0 and (q − p) × r = 0, but neither 0 ≤ (q − p) · r ≤ r · r nor 0 ≤ (p − q) · s ≤ s · s, then the two lines are collinear but disjoint.
             * If r × s = 0 and (q − p) × r ≠ 0, then the two lines are parallel and non-intersecting.
             * If r × s ≠ 0 and 0 ≤ t ≤ 1 and 0 ≤ u ≤ 1, the two line segments meet at the point p + t r = q + u s.
             * Otherwise, the two line segments are not parallel but do not intersect.
             */

            // line1.Start = p
            // line2.Start = q

            var r = line1.Point2 - line1.Point1;
            var s = line2.Point2 - line2.Point1;
            var startPointVector = (line2.Point1 - line1.Point1);

            var denom = r.Cross(s); // denom = r × s

            var firstScalar = startPointVector.Cross(s) / denom; // firstScalar = t
            var secondScalar = startPointVector.Cross(r) / denom; // secondScalar = u
            intersection = line1.Point1 + (firstScalar * r);
            return denom != 0d && firstScalar >= 0d && firstScalar <= 1d && secondScalar >= 0d && secondScalar <= 1d;
        }
    }
}
