using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using NextLevelBJJ.DataService;

namespace NextLevelBJJ.UnitTests
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void testConn()
        {
            var check = new Check();

            check.check();
        }
    }
}
