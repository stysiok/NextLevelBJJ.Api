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
                        <div id='monday'>11:30 - 12:30 Next Level training poniedzialek</div>
                        <div id='monday'>14:30 - 15:30 Next Level training poniedzialek</div>
                        <div id='tuesday'>18:45 - 19:45 Next Level training wtorek</div>
                        <div id='wednesday'>18:00 - 19:00 &amp; Next &nbsp; Level &Oacute; training środa</div>
                        <div id='thursday'>11:30 - 12:30 Next Level training czwartek DZIECI</div>
                        <div id='friday'>12:30 - 13:30 Next Level training piątek</div>
                        <div id='saturday'>15:30 - 16:30 Next Level training sobota</div>
                    </body>
                </html>
                ";
            htmlDoc.LoadHtml(html);

            _dayIdPair = new Dictionary<DayOfWeek, string>()
            {
                { DayOfWeek.Monday, "monday" },
                { DayOfWeek.Tuesday, "tuesday" },
                { DayOfWeek.Wednesday, "wednesday" },
                { DayOfWeek.Thursday, "thursday" },
                { DayOfWeek.Friday, "friday" },
                { DayOfWeek.Saturday, "saturday" }
            };

            webHtmlLoadHelperMock.Setup(m => m.LoadContentFromUrl(mockReturnsObject))
                .Returns(htmlDoc);

            webHtmlLoadHelperMock.Setup(m => m.LoadContentFromUrl(mockReturnsNull))
                .Returns<HtmlDocument>(null);
        }

        [TestMethod]
        public void GetTrainingDay_Monday_ReturnsTrainingForMonday()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsObject);
            var day = DayOfWeek.Monday;

            var result = trainingsService.GetTrainingDay(day);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Day == day);
            Assert.IsTrue(result.Classes.Any());
            Assert.IsTrue(result.Classes.All(x => x.Name.Contains("poniedzialek") && x.Day == day));
            Assert.AreEqual(1, result.Classes.Count());
        }

        [TestMethod]
        public void GetTrainingDay_Tuesday_MappingIsCorrect()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsObject);
            var day = DayOfWeek.Tuesday;

            var result = trainingsService.GetTrainingDay(day);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Day == day);
            Assert.IsTrue(result.Classes.Any());
            Assert.IsTrue(result.Classes.All(x => x.Name.Contains("wtorek") && x.Day == day));
            foreach (var training in result.Classes)
            {
                Assert.IsTrue(training.Name == "Next Level training wtorek");
                Assert.IsFalse(training.IsKidsClass);
                Assert.IsTrue(training.StartHour.Equals(new TimeSpan(18, 45, 00)));
                Assert.IsTrue(training.FinishHour.Equals(new TimeSpan(19, 45, 00)));
            }
        }

        [TestMethod]
        public void GetTrainingDay_Wedensday_ClassesDoNotContainWierdText()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsObject);
            var day = DayOfWeek.Wednesday;

            var result = trainingsService.GetTrainingDay(day);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Day == day);
            Assert.IsTrue(result.Classes.Any());
            Assert.IsTrue(result.Classes.All(x => x.Name.Contains("środa") 
                                                    && x.Day == day
                                                    && !x.Name.Contains("&nbsp;")
                                                    && !x.Name.Contains("&amp;")
                                                    && !x.Name.Contains("&Oacute;")));
        }

        [TestMethod]
        public void GetTrainingDay_Sunday_ReturnsNullClassesForKeyAbsentInDictionary()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsObject);
            var day = DayOfWeek.Sunday;

            var result = trainingsService.GetTrainingDay(day);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Day == day);
            Assert.IsNull(result.Classes);
        }

        [TestMethod]
        public void GetTrainingDay_NullHtmlDoc_ThrowsException()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsNull);

            var exc = Assert.ThrowsException<Exception>(() => trainingsService.GetTrainingDay(DayOfWeek.Friday));
            Assert.IsTrue(exc.Message.Contains("Błąd podczas przetwarzania grafiku ze strony internetowej. Dodatkowa informacja: "));
        }

        [TestMethod]
        public void GetTrainingWeek_ShouldReturnTrainingWeek()
        {
            var trainingsService = new TrainingsService(_dayIdPair, webHtmlLoadHelperMock.Object, mockReturnsObject);

            var result = trainingsService.GetTrainingWeek();

            Assert.IsTrue(result.Count() == Enum.GetValues(typeof(DayOfWeek)).Length);
            CollectionAssert.AllItemsAreNotNull(result);
        }

    }
}
