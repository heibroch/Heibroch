using System;

namespace Heibroch.Test
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoTestReport : Attribute
    {
        public Type TestedType { get; set; }

        public void AppendToReport(string methodName)
        {
            //Do stuff
        }



        public AutoTestReport(Type testedType)
        {
            TestedType = testedType;
        }
    }
}
