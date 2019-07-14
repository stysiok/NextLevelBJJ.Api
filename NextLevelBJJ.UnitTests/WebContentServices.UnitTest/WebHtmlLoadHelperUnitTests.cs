using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using NextLevelBJJ.WebContentServices.Helpers;

namespace NextLevelBJJ.UnitTests.WebContentServices.UnitTest
{
    [TestClass]
    public class WebHtmlLoadHelperUnitTests
    {
        private readonly IWebHtmlLoadHelper webHtmlLoadHelper;

        public WebHtmlLoadHelperUnitTests()
        {
            webHtmlLoadHelper = new WebHtmlLoadHelper();
        }

        [TestMethod]
        public void LoadContentFromUrl_ValidUrl_ReturnsHtmlDocument()
        {
            string url = @"https://www.akademianextlevel.com/grafik";

            var result = webHtmlLoadHelper.LoadContentFromUrl(url);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoadContentFromUrl_NotValidUrl_ThrowsException()
        {
            string url = @"httasdasdps://www.nexadsasdasdtlevelbjj.pl/grafdasdik";

            var result = Assert.ThrowsException<Exception>(() => webHtmlLoadHelper.LoadContentFromUrl(url));

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania grafiku zajęć ze strony. Dodatkowa informacja: "));
        }

        [TestMethod]
        public void LoadContentFromUrl_EmptyUrl_ThrowsException()
        {
            string url = "";

            var result = Assert.ThrowsException<Exception>(() => webHtmlLoadHelper.LoadContentFromUrl(url));

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania grafiku zajęć ze strony. Dodatkowa informacja: "));
        }

    }
}
