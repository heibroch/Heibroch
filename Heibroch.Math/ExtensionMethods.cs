using System;

namespace Heibroch.Common
{
    public static class ExtensionMethods
    {
        public static bool IsEqualTo(this float value1, float value2)
        {
            return Math.Abs(value1 - value2) < float.Epsilon;
        }

        public static bool IsEqualTo(this double value1, double value2)
        {
            return Math.Abs(value1 - value2) < double.Epsilon;
        }
    }
}
