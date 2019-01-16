using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelBJJ.ScheduleService;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.UnitTests.ScheduleServiceTests
{
    [TestClass]
    public class TrainingServiceTests
    {
        TrainingsService trainingService = new TrainingsService();

        [TestMethod]
        public void GetTrainingDay_ShouldReturnFullTraingingDay()
        {
            var dayOfWeek = DayOfWeek.Monday;

            var training = trainingService.GetTrainingDay(dayOfWeek);

            Assert.IsNotNull(training);
            Assert.IsTrue(training.Day == dayOfWeek);
        }

        [TestMethod]
        public void GetTrainingDay_ShouldReturnNullForSunday()
        {
            var dayOfWeek = DayOfWeek.Sunday;

            var training = trainingService.GetTrainingDay(dayOfWeek);

            Assert.IsNull(training);
        }

        [TestMethod]
        public void GetTrainingWeek_ShouldReturnWeek()
        {
            var week = trainingService.GetTrainingWeek();

            Assert.IsNotNull(week);
        }
    }
}
