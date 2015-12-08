using System;
using System.Collections.Generic;
using Maths = System.Math;

namespace Heibroch.Math
{
    public static class Axis
    {
        private static List<double> CreateValuesAtIntervals(double minimum,
                                                            double maximum,
                                                            double valueSpacing = 1000,
                                                            bool includeZero = true)
        {
            //For each thousand, then create a tick
            var tickValues = new List<double>();

            for (var i = minimum; i < maximum; i += valueSpacing)
            {
                if (!includeZero && Maths.Abs(i) < double.Epsilon) continue;
                tickValues.Add(Maths.Round(i / valueSpacing, 0) * valueSpacing);
            }
            
            return tickValues;
        }
    }
}
