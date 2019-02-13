using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace NextLevelBJJ.UnitTests.DataServices.UnitTests.Helpers
{
    public class UnitTestsExtensions
    {
        public void MethodReturningClass_PassedArgument_ReturnsNull<T>(Func<Task<T>> func) where T : class 
        {
            var result = func().Result;
            Assert.IsNull(result);
        }

        public void MethodReturningStruct_PassedArgument_ThrowsException<T>(Func<Task<T>> func, string messageInsideException = "") where T : struct 
        {
            var result = Assert.ThrowsException<Exception>(() => func().Result);

            Assert.IsNotNull(result);
            if (!string.IsNullOrEmpty(messageInsideException))
            {
                Assert.IsTrue(result.Message.Contains(messageInsideException));
            }
        }
    }
}
