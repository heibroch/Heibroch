namespace Heibroch.Math
{
    public struct Vector2D
    {
        public double X;
        public double Y;

        public double Cross(Vector2D other)
        {
            return X * other.Y - Y * other.X;
        }
        
        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D {X = v1.X + v2.X, Y = v1.Y + v2.Y};
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D {X = v1.X - v2.X, Y = v1.Y - v2.Y};
        }
        
        public static Vector2D operator *(Vector2D v, double scalar)
        {
            return new Vector2D {X = v.X * scalar, Y = v.Y * scalar};
        }

        public static Vector2D operator *(double scalar, Vector2D v)
        {
            return v * scalar;
        }
    }
}
