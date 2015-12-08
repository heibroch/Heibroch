using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Heibroch.Test
{
    [TestClass]
    public abstract class TestClassBase
    {
        protected void TestCompleted([CallerMemberName]string memberName = "")
        {
            var autoTestReportAttribute = this.GetType().GetMethod(memberName).GetCustomAttributes(true).FirstOrDefault(x => (x is AutoTestReport));
            if (autoTestReportAttribute == null) return;

            var autoTestReport = (AutoTestReport)autoTestReportAttribute;
            autoTestReport.AppendToReport(memberName);
        }
    }
}
