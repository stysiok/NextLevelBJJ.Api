using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using NextLevelBJJ.WebContentServices.Models;
using System.Collections.Generic;
using System.Text;
using NextLevelBJJ.WebContentServices;

namespace NextLevelBJJ.UnitTests.WebContentServices.UnitTest
{
    [TestClass]
    public class ClassesServiceUnitTests
    {
        readonly Mock<ITrainingsService> trainingsService = new Mock<ITrainingsService>();
        TrainingDay trainingDayMonday;
        IClassesService classesService;


        [TestInitialize]
        public void Initialize()
        {
            trainingDayMonday = new TrainingDay
            {
                Day = DayOfWeek.Monday,
                Classes = new List<Class>
                {
                    new Class
                    {
                        Day = DayOfWeek.Monday,
                        Name = "BJJ Początkująca",
                        StartHour = new TimeSpan(17, 15, 0),
                        FinishHour = new TimeSpan(18, 30, 0)
                    },
                    new Class
                    {
                        Day = DayOfWeek.Monday,
                        Name = "BJJ DZIECI",
                        StartHour = new TimeSpan(17, 30, 0),
                        FinishHour = new TimeSpan(18, 30, 0)
                    },
                    new Class
                    {
                        Day = DayOfWeek.Monday,
                        Name = "BJJ Zaawansowana",
                        StartHour = new TimeSpan(18, 30, 0),
                        FinishHour = new TimeSpan(19, 45, 0)
                    },
                    new Class
                    {
                        Day = DayOfWeek.Monday,
                        Name = "MMA Zaawansowana",
                        StartHour = new TimeSpan(19, 45, 0),
                        FinishHour = new TimeSpan(21, 0, 0)
                    },
                }
            };

            trainingsService.Setup(m => m.GetTrainingWeek())
                .Returns(new List<TrainingDay> { trainingDayMonday });

            trainingsService.Setup(m => m.GetTrainingDay(DayOfWeek.Monday))
                .Returns(trainingDayMonday);

            trainingsService.Setup(m => m.GetTrainingDay(DayOfWeek.Sunday))
                .Returns(new TrainingDay { Day = DayOfWeek.Sunday, Classes = null });


            classesService = new ClassesService(trainingsService.Object);
        }

        [TestMethod]
        public void GetClass_ValidDateNotKidsClass_ReturnsValidNotKidsClass()
        {
            var date = new DateTime(2019, 02, 11, 18, 25, 36);
            bool isKids = false;

            var result = classesService.GetClass(date, isKids);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsKidsClass);
            Assert.IsFalse(result.Name.Contains("DZIECI"));
            Assert.IsTrue(result.Name.Equals("BJJ Zaawansowana"));
            Assert.IsTrue(result.Day == DayOfWeek.Monday);
            Assert.IsTrue(result.StartHour == new TimeSpan(18, 30, 0));
            Assert.IsTrue(result.FinishHour == new TimeSpan(19, 45, 0));
        }

        [TestMethod]
        public void GetClass_ValidDateKidsClass_ReturnsValidKidsClass()
        {
            var date = new DateTime(2019, 02, 11, 17, 35, 36);
            bool isKids = true;

            var result = classesService.GetClass(date, isKids);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsKidsClass);
            Assert.IsTrue(result.Name.Contains("DZIECI"));
            Assert.IsTrue(result.Name.Equals("BJJ DZIECI"));
            Assert.IsTrue(result.Day == DayOfWeek.Monday);
            Assert.IsTrue(result.StartHour == new TimeSpan(17, 30, 0));
            Assert.IsTrue(result.FinishHour == new TimeSpan(18, 30, 0));
        }

        [TestMethod]
        public void GetClass_ClassesEmptyForDate_ReturnsNull()
        {
            var date = new DateTime(2019, 02, 10, 17, 35, 36);
            bool isKids = true;

            var result = classesService.GetClass(date, isKids);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUpcomingClass_DateWithNoClasses_ReturnsNull()
        {
            var upcomingClass = classesService.GetUpcomingClass(new DateTime(2019, 03, 19), false);

            Assert.IsNull(upcomingClass);
        }

        [TestMethod]
        public void GetUpcomingClass_DateWithEarlyHour_ReturnsNull()
        {
            var upcomingClass = classesService.GetUpcomingClass(new DateTime(2019, 03, 18, 15, 00, 00), false);

            Assert.IsNull(upcomingClass);
        }

        [TestMethod]
        public void GetUpcomingClass_DateWith10MinToNextClass_ReturnsClass()
        {
            var upcomingClass = classesService.GetUpcomingClass(new DateTime(2019, 03, 18, 17, 05, 00), false);

            Assert.IsNotNull(upcomingClass);
        }

        [TestMethod]
        public void GetUpcomingClass_DateWith13MinCurrentClass_ReturnsClass()
        {
            var upcomingClass = classesService.GetUpcomingClass(new DateTime(2019, 03, 18, 17, 28, 00), false);

            Assert.IsNotNull(upcomingClass);
        }

        [TestMethod]
        public void GetUpcomingClass_ValidDateAndKidsClassFilter_ReturnsKidsClass()
        {
            var upcomingClass = classesService.GetUpcomingClass(new DateTime(2019, 03, 18, 17, 28, 00), true);

            Assert.IsNotNull(upcomingClass);
        }
    }
}
