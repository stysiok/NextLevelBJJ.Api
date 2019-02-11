using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.UnitTests.DataServices.UnitTests.Helpers;
using System;
using System.Collections.Generic;

namespace NextLevelBJJ.UnitTests.DataServices.UnitTests
{
    [TestClass]
    public class StudentsServiceUnitTests
    {
        Mock<NextLevelContext> nextLevelContextMock = new Mock<NextLevelContext>();
        IStudentsService studentsService;
        IDictionary<string, Student> studentTestObjects;

        [TestInitialize]
        public void Initialize()
        {
            studentTestObjects = DbSetHelper.CreateDataSeed<Student>();
            var mockStudentsDbSet = DbSetHelper.CreateDbSetMock(studentTestObjects);

            nextLevelContextMock.Setup(p => p.Students).Returns(mockStudentsDbSet.Object);


            studentsService = new StudentsService(nextLevelContextMock.Object);
        }

        [TestMethod]
        public void GetStudentByPassCode_ValidPassCode_ReturnsStudent()
        {
            var validStudent = studentTestObjects["valid"];
            var result = studentsService.GetStudentByPassCode(validStudent.PassCode).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result, validStudent);
        }

        [TestMethod]
        public void GetStudentByPassCode_EmptyPassCode_ReturnsNull()
        {
            var result = studentsService.GetStudentByPassCode("").Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_NotEnabledStudent_ReturnsNull()
        {
            var notEnabledStudent = studentTestObjects["notEnabled"];
            var result = studentsService.GetStudentByPassCode(notEnabledStudent.PassCode).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedStudent_ReturnsNull()
        {
            var deletedStudent = studentTestObjects["deleted"];
            var result = studentsService.GetStudentByPassCode(deletedStudent.PassCode).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedNotEnabledStudent_ReturnsNull()
        {
            var deletedNotEnabledStudent = studentTestObjects["deletedNotEnabled"];
            var result = studentsService.GetStudentByPassCode(deletedNotEnabledStudent.PassCode).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_NullStudentsDbSet_ThrowsException()
        {
            var studentsService = new StudentsService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => studentsService.GetStudentByPassCode("").Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania profilu klubowicza. Dodatkowa informacja: "));
        }
    }
}
