using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using NextLevelBJJ.WebContentServices;
using System.Linq;

namespace NextLevelBJJ.UnitTests.WebContentServices.UnitTest
{
    [TestClass]
    public class TrainingsServiceUnitTests
    {
        Dictionary<DayOfWeek, string> _dayIdPair;
        Mock<IWebHtmlLoadHelper> webHtmlLoadHelperMock = new Mock<IWebHtmlLoadHelper>();
        string mockReturnsObject = "Content";
        string mockReturnsNull = "Null";

        public TrainingsServiceUnitTests()
        {
            var htmlDoc = new HtmlDocument();
            var html = @"<!DOCTYPE html>
                <html>
                    <body>
                        <div id='monday'>18:30 - 19:30 Next Level training poniedzialek</div>
                        <div id='tuesday'>18:45 - 19:45 Next &nbsp; Level training wtorek</div>
                        <div id='wensday'>18:00 - 19:00 &amp; Next Level training środa</div>
                        <div id='thursday'>11:30 - 12:30 Next Level training czwartek</div>
                        <div id='friday'>12:30 - 13:30 Next Level &Oacute; training piątek</div>
                        <div id='saturday'>15:30 - 16:30 Next Level training sobota</div>
                    </body>
                </html>
                ";
            htmlDoc.LoadHtml(html);

            _dayIdPair = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "monday" },
                { DayOfWeek.Tuesday, "tuesday" },
                { DayOfWeek.Wednesday, "wensday" },
                { DayOfWeek.Thursday, "thursday" },
                { DayOfWeek.Friday, "friday" },
                { DayOfWeek.Saturday, "saturday" },
            };

            webHtmlLoadHelperMock.Setup(m => m.LoadContentFromUrl(mockReturnsObject))
                .Returns(htmlDoc);

            webHtmlLoadHelperMock.Setup(m => m.LoadContentFromUrl(mockReturnsNull))
                .Returns<HtmlDocument>(null);
        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForMonday()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsObject);

            var result = trainingsService.GetTrainingDay(DayOfWeek.Monday);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Day == DayOfWeek.Monday);
            Assert.IsTrue(result.Classes.Any(x => x.Name.Contains("poniedzialek")));
        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForTuesday()
        {

        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForWensday()
        {

        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForThursday()
        {

        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForFriday()
        {

        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForSaturday()
        {

        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnTrainingForSunday()
        {

        }

        [TestMethod]
        public void GetTrainingWeek_ShouldReturnTrainingWeek()
        {

        }

    }
}
