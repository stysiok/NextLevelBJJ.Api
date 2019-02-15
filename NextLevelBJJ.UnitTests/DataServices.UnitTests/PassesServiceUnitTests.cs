using AutoFixture;
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
    public class PassesServiceUnitTests : UnitTestsExtensions
    {
        Mock<NextLevelContext> nextLevelContextMock = new Mock<NextLevelContext>();
        IPassesService passesService;
        IDictionary<string, Pass> passTypeList;
        Pass latestPass, notLatestPass;

        [TestInitialize]
        public void Initialize()
        {
            var fixture = DbSetHelper.ConfiguredFixture();
            latestPass = fixture.Build<Pass>()
                                    .With(p => p.IsEnabled, true)
                                    .With(p => p.IsDeleted, false)
                                    .With(p => p.CreatedDate, new DateTime(2020, 12, 12))
                                    .With(p => p.StudentId, 1)
                                    .Create();
            notLatestPass = fixture.Build<Pass>()
                                    .With(p => p.IsEnabled, true)
                                    .With(p => p.IsDeleted, false)
                                    .With(p => p.CreatedDate, new DateTime(2019, 12, 12))
                                    .With(p => p.StudentId, 1)
                                    .Create();

            passTypeList = DbSetHelper.CreateDataSeed<Pass>();
            passTypeList.Add("latestPass", latestPass);
            passTypeList.Add("notLatestPass", notLatestPass);
            

            var mockStudentsDbSet = DbSetHelper.CreateDbSetMock(passTypeList);

            nextLevelContextMock.Setup(p => p.Passes).Returns(mockStudentsDbSet.Object);

            passesService = new PassesService(nextLevelContextMock.Object);
        }

        [TestMethod]
        public void GetPass_ValidId_ReturnsPass()
        {
            var validPass = passTypeList["valid"];
            var result = passesService.GetPass(validPass.Id).Result;

            Assert.AreEqual(validPass, result);
        }

        [TestMethod]
        public void GetPass_NotEnabledStudent_ReturnsNull()
        {
            MethodReturningClass_PassedArgument_ReturnsNull(() => passesService.GetPass(passTypeList["notEnabled"].Id));
        }

        [TestMethod]
        public void GetPass_DeletedStudent_ReturnsNull()
        {
            MethodReturningClass_PassedArgument_ReturnsNull(() => passesService.GetPass(passTypeList["deleted"].Id));
        }

        [TestMethod]
        public void GetPass_DeletedNotEnabledStudent_ReturnsNull()
        {
            MethodReturningClass_PassedArgument_ReturnsNull(() => passesService.GetPass(passTypeList["deletedNotEnabled"].Id));
        }

        [TestMethod]
        public void GetPass_NullPassTypesDbSet_ThrowsException()
        {
            var passesService = new PassesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passesService.GetPass(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania karnetu. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void GetRecentStudentPass_ValidId_ReturnsPass()
        {
            var validPass = passTypeList["latestPass"];
            var result = passesService.GetRecentStudentPass(validPass.StudentId).Result;

            Assert.AreEqual(validPass, result);
        }

        [TestMethod]
        public void GetRecentStudentPass_NotEnabledStudent_ThrowsException()
        {
            var result = Assert.ThrowsException<Exception>(() => passesService.GetRecentStudentPass(passTypeList["notEnabled"].StudentId).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ostatniego karnetu klubowicza. Dodatkowa informacja: "));
        }

        [TestMethod]
        public void GetRecentStudentPass_DeletedStudent_ThrowsException()
        {
            var result = Assert.ThrowsException<Exception>(() => passesService.GetRecentStudentPass(passTypeList["deleted"].StudentId).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ostatniego karnetu klubowicza. Dodatkowa informacja: "));
        }

        [TestMethod]
        public void GetRecentStudentPass_DeletedNotEnabledStudent_ThrowsException()
        {
            var result = Assert.ThrowsException<Exception>(() => passesService.GetRecentStudentPass(passTypeList["deletedNotEnabled"].StudentId).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ostatniego karnetu klubowicza. Dodatkowa informacja: "));
        }

        [TestMethod]
        public void GetRecentStudentPass_NullPassTypesDbSet_ThrowsException()
        {
            var passesService = new PassesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passesService.GetRecentStudentPass(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ostatniego karnetu klubowicza. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void GetStudentPasses_ValidId_ReturnsPassList()
        {
            var studentId = passTypeList["latestPass"].StudentId;
            var result = passesService.GetStudentPasses(studentId).Result;

            Assert.IsTrue(result.Count == 2);
            foreach(var pass in result)
            {
                Assert.AreEqual(studentId, pass.StudentId);
            }
        }

        [TestMethod]
        public void GetStudentPasses_NotEnabledStudent_ReturnsEmptyList()
        {
            var result = passesService.GetStudentPasses(passTypeList["notEnabled"].StudentId).Result;

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetStudentPasses_DeletedStudent_ReturnsEmptyList()
        {
            var result = passesService.GetStudentPasses(passTypeList["deleted"].StudentId).Result;

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetStudentPasses_DeletedNotEnabledStudent_ReturnssEmptyList()
        {
            var result = passesService.GetStudentPasses(passTypeList["deletedNotEnabled"].StudentId).Result;

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetStudentPasses_NullPassTypesDbSet_ThrowsException()
        {
            var passesService = new PassesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passesService.GetStudentPasses(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania karnetów klubowicza. Dodatkowa informacja: "));
        }
    }
}
