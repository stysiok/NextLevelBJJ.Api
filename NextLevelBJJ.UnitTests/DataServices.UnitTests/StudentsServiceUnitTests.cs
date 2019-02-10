using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.UnitTests.DataServices.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelBJJ.UnitTests.DataServices.UnitTests
{
    [TestClass]
    public class StudentsServiceUnitTests
    {
        Mock<NextLevelContext> nextLevelContextMock = new Mock<NextLevelContext>();
        IStudentsService studentsService;
        Student validStudent, notEnabledStudent, deletedStudent, deletedNotEnabledStudent;
        Fixture fixture; 

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                            .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            validStudent = fixture.Build<Student>()
                                    .With(p => p.IsDeleted, false)
                                    .With(p => p.IsEnabled, true)
                                    .Create();
            notEnabledStudent = fixture.Build<Student>()
                                    .With(p => p.IsDeleted, false)
                                    .With(p => p.IsEnabled, false)
                                    .Create();
            deletedStudent = fixture.Build<Student>()
                                    .With(p => p.IsDeleted, true)
                                    .With(p => p.IsEnabled, true)
                                    .Create();
            deletedNotEnabledStudent = fixture.Build<Student>()
                                    .With(p => p.IsDeleted, true)
                                    .With(p => p.IsEnabled, false)
                                    .Create();

            var students = new List<Student>
            {
                validStudent,
                notEnabledStudent,
                deletedNotEnabledStudent,
                deletedStudent
            };
            var mockStudentsDbSet = DbSetMocker.CreateDbSetMock(students);


            nextLevelContextMock.Setup(p => p.Students).Returns(mockStudentsDbSet.Object);


            studentsService = new StudentsService(nextLevelContextMock.Object);
        }

        [TestMethod]
        public void GetStudentByPassCode_ValidPassCode_ReturnsStudent()
        {
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
            var result = studentsService.GetStudentByPassCode(notEnabledStudent.PassCode).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedStudent_ReturnsNull()
        {
            var result = studentsService.GetStudentByPassCode(deletedStudent.PassCode).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedNotEnabledStudent_ReturnsNull()
        {
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
