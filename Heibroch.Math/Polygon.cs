using System.Collections.Generic;

namespace Heibroch.Math
{
    public class Polygon
    {
        public List<Vector2D> Points { get; set; }

        public double CalculateArea() => CalculateArea(Points.ToArray());

        public static double CalculateArea(Vector2D[] points)
        {
            var doubleArea = 0d;
            for (var i = 0; i < points.Length; i++)
            {
                var current = points[i];
                var next = points[i + 1 >= points.Length ? 0 : i + 1];

                doubleArea += (current.X * next.Y) - (current.Y * next.X);
            }

            return doubleArea / 2;
        }
    }
}
