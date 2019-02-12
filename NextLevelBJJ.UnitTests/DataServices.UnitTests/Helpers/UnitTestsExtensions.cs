using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace NextLevelBJJ.UnitTests.DataServices.UnitTests.Helpers
{
    public class UnitTestsExtensions
    {
        public void Method_PassedArgument_ReturnsNull<T>(Task<T> task) where T : class
        {
            var result = task.Result;
            Assert.IsNull(result);
        }
    }
}
