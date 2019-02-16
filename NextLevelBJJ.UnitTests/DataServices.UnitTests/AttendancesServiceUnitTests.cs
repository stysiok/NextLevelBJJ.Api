using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.UnitTests.DataServices.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.UnitTests.DataServices.UnitTests
{
    [TestClass]
    public class AttendancesServiceUnitTests : UnitTestsExtensions
    {
        Mock<NextLevelContext> nextLevelContextMock = new Mock<NextLevelContext>();
        IAttendancesService attendancesSerivce;
        IDictionary<string, Attendance> attendanceList; 

        [TestInitialize]
        public void Initialize()
        {
            var fixture = DbSetHelper.ConfiguredFixture();
            fixture.RepeatCount = 10;
            var list = fixture.Build<Attendance>()
                    .With(p => p.IsDeleted, false)
                    .With(p => p.IsEnabled, true)
                    .With(p => p.StudentId, 1)
                    .CreateMany();

            var oldAttendance = fixture.Build<Attendance>()
                                       .With(p => p.IsDeleted, false)
                                       .With(p => p.IsEnabled, true)
                                       .With(p => p.StudentId, 2)
                                       .With(p => p.CreatedDate, new DateTime(2020, 12, 21))
                                       .Create();
            var newAttendance = fixture.Build<Attendance>()
                                       .With(p => p.IsDeleted, false)
                                       .With(p => p.IsEnabled, true)
                                       .With(p => p.StudentId, 2)
                                       .With(p => p.CreatedDate, new DateTime(2020, 12, 23))
                                       .Create();

            attendanceList = DbSetHelper.CreateDataSeed<Attendance>();

            attendanceList.Add("oldAttendance", oldAttendance);
            attendanceList.Add("newAttendance", newAttendance);

            int counter = 0;
            foreach (var attendance in list)
            {
                attendanceList.Add("validAttendance" + counter++, attendance);
            }

            var mockDbSet = DbSetHelper.CreateDbSetMock(attendanceList);

            nextLevelContextMock.Setup(p => p.Attendances).Returns(mockDbSet.Object);

            attendancesSerivce = new AttendancesService(nextLevelContextMock.Object);
        }

        [TestMethod]
        public void GetAttendancesAmountTrackedOnPass_ValidId_ReturnsAmountOfEntries()
        {
            var valid = attendanceList["valid"];
            var result = attendancesSerivce.GetAttendancesAmountTrackedOnPass(valid.PassId).Result;

            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void GetAttendancesAmountTrackedOnPass_NotEnabledStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetAttendancesAmountTrackedOnPass(attendanceList["notEnabled"].PassId).Result;
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetAttendancesAmountTrackedOnPass_DeletedStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetAttendancesAmountTrackedOnPass(attendanceList["deleted"].PassId).Result;
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetAttendancesAmountTrackedOnPass_DeletedNotEnabledStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetAttendancesAmountTrackedOnPass(attendanceList["deletedNotEnabled"].PassId).Result;
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void GetAttendancesAmountTrackedOnPass_NullPassTypesDbSet_ThrowsException()
        {
            var service = new AttendancesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => service.GetAttendancesAmountTrackedOnPass(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ilości odbytych treningów na danym karnecie. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void GetStudentAttendences_ValidId_ReturnsAttendances()
        {
            var valid = attendanceList["validAttendance1"];
            var result = attendancesSerivce.GetStudentAttendences(valid.StudentId, 0, 8).Result;

            Assert.IsTrue(result.Count == 8);
            CollectionAssert.AllItemsAreUnique(result);
            CollectionAssert.AllItemsAreNotNull(result);
            foreach (var item in result)
            {
                Assert.AreEqual(valid.StudentId, item.StudentId);
            }
        }

        [TestMethod]
        public void GetStudentAttendences_NotEnabledStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetStudentAttendences(attendanceList["notEnabled"].StudentId, 1, 1).Result;
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetStudentAttendences_DeletedStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetStudentAttendences(attendanceList["deleted"].StudentId, 1, 1).Result;
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetStudentAttendences_DeletedNotEnabledStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetStudentAttendences(attendanceList["deletedNotEnabled"].StudentId, 1, 1).Result;
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetStudentAttendences_NullPassTypesDbSet_ThrowsException()
        {
            var service = new AttendancesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => service.GetStudentAttendences(1, 1, 1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania uczestnictwa klubowicza w zajęciach. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void GetRecentAttendance_ValidId_ReturnsAttendances()
        {
            var valid = attendanceList["newAttendance"];
            var result = attendancesSerivce.GetRecentAttendance(valid.StudentId).Result;

            Assert.AreEqual(valid, result);
        }

        [TestMethod]
        public void GetRecentAttendance_NotEnabledStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetRecentAttendance(attendanceList["notEnabled"].StudentId).Result;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRecentAttendance_DeletedStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetRecentAttendance(attendanceList["deleted"].StudentId).Result;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRecentAttendance_DeletedNotEnabledStudent_ReturnsNull()
        {
            var result = attendancesSerivce.GetRecentAttendance(attendanceList["deletedNotEnabled"].StudentId).Result;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRecentAttendance_NullPassTypesDbSet_ThrowsException()
        {
            var service = new AttendancesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => service.GetRecentAttendance(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ostaniego treningu klubowicza. Dodatkowa informacja: "));
        }
    }
}
