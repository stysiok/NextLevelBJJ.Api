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
    public class PassTypesServiceUnitTests : UnitTestsExtensions
    {
        Mock<NextLevelContext> nextLevelContextMock = new Mock<NextLevelContext>();
        IPassTypesService passTypesService;
        IDictionary<string, PassType> passTypeList;

        [TestInitialize]
        public void Initialize()
        {
            passTypeList = DbSetHelper.CreateDataSeed<PassType>();
            var mockStudentsDbSet = DbSetHelper.CreateDbSetMock(passTypeList);
            
            nextLevelContextMock.Setup(p => p.PassTypes).Returns(mockStudentsDbSet.Object);
            
            passTypesService = new PassTypesService(nextLevelContextMock.Object);
        }

        [TestMethod]
        public void GetPassTypeById_ValidId_ReturnsPassType()
        {
            var validPassType = passTypeList["valid"];
            var result = passTypesService.GetPassTypeById(validPassType.Id).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result, validPassType);
        }

        [TestMethod]
        public void GetStudentByPassCode_NotEnabledStudent_ReturnsNull()
        {
            Method_PassedArgument_ReturnsNull(passTypesService.GetPassTypeById(passTypeList["notEnabled"].Id));
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedStudent_ReturnsNull()
        {
            Method_PassedArgument_ReturnsNull(passTypesService.GetPassTypeById(passTypeList["deleted"].Id));
        }
        

        [TestMethod]
        public void GetStudentByPassCode_DeletedNotEnabledStudent_ReturnsNull()
        {
            Method_PassedArgument_ReturnsNull(passTypesService.GetPassTypeById(passTypeList["deletedNotEnabled"].Id));
        }

        [TestMethod]
        public void GetStudentByPassCode_NullStudentsDbSet_ThrowsException()
        {
            var studentsService = new PassTypesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => studentsService.GetPassTypeById(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania rodzaju karnetu. Dodatkowa informacja: "));
        }
    }
}
