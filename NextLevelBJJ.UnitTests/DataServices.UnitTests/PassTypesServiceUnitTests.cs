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
    public class PassTypesServiceUnitTests
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
            var notEnabledPassType = passTypeList["notEnabled"];
            var result = passTypesService.GetPassTypeById(notEnabledPassType.Id).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedStudent_ReturnsNull()
        {
            var deletedPassType = passTypeList["deleted"];
            var result = passTypesService.GetPassTypeById(deletedPassType.Id).Result;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetStudentByPassCode_DeletedNotEnabledStudent_ReturnsNull()
        {
            var deletedNotEnabledPassType = passTypeList["deletedNotEnabled"];
            var result = passTypesService.GetPassTypeById(deletedNotEnabledPassType.Id).Result;

            Assert.IsNull(result);
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
