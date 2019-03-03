using AutoFixture;
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
    public class PassTypesServiceUnitTests : UnitTestsExtensions
    {
        Mock<NextLevelContext> nextLevelContextMock = new Mock<NextLevelContext>();
        IPassTypesService passTypesService;
        IDictionary<string, PassType> passTypeList;

        [TestInitialize]
        public void Initialize()
        {
            var fixture = DbSetHelper.ConfiguredFixture();
            var kidsPass = fixture.Build<PassType>()
                                  .With(p => p.Name, "Dzieci")
                                  .With(p => p.IsEnabled, true)
                                  .With(p => p.IsDeleted, false)
                                  .Create();

            passTypeList = DbSetHelper.CreateDataSeed<PassType>();
            passTypeList.Add("validKidsPass", kidsPass);

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
        public void GetPassTypeById_NotEnabledStudent_ReturnsNull()
        {
            MethodReturningClass_PassedArgument_ReturnsNull(() => passTypesService.GetPassTypeById(passTypeList["notEnabled"].Id));
        }

        [TestMethod]
        public void GetPassTypeById_DeletedStudent_ReturnsNull()
        {
            MethodReturningClass_PassedArgument_ReturnsNull(() => passTypesService.GetPassTypeById(passTypeList["deleted"].Id));
        }

        [TestMethod]
        public void GetPassTypeById_DeletedNotEnabledStudent_ReturnsNull()
        {
            MethodReturningClass_PassedArgument_ReturnsNull(() => passTypesService.GetPassTypeById(passTypeList["deletedNotEnabled"].Id));
        }

        [TestMethod]
        public void GetPassTypeById_NullStudentsDbSet_ThrowsException()
        {
            var passTypesService = new PassTypesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passTypesService.GetPassTypeById(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania rodzaju karnetu. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void GetPassTypeEntriesById_ValidId_ReturnsPassTypeEntries()
        {
            var validPassType = passTypeList["valid"];
            var result = passTypesService.GetPassTypeEntriesById(validPassType.Id).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(result, validPassType.Entries);
        }

        [TestMethod]
        public void GetPassTypeEntriesById_NotEnabledStudent_ThrowsException()
        {
            MethodReturningStruct_PassedArgument_ThrowsException(() => passTypesService.GetPassTypeEntriesById(passTypeList["notEnabled"].Id), "Błąd podczas pobierania ilości wejść opartych na rodzaju karnetu. Dodatkowa informacja: ");
        }

        [TestMethod]
        public void GetPassTypeEntriesById_DeletedStudent_ThrowsException()
        {
            MethodReturningStruct_PassedArgument_ThrowsException(() => passTypesService.GetPassTypeEntriesById(passTypeList["deleted"].Id), "Błąd podczas pobierania ilości wejść opartych na rodzaju karnetu. Dodatkowa informacja: ");
        }

        [TestMethod]
        public void GetPassTypeEntriesById_DeletedNotEnabledStudent_ThrowsException()
        {
            MethodReturningStruct_PassedArgument_ThrowsException(() => passTypesService.GetPassTypeEntriesById(passTypeList["deletedNotEnabled"].Id), "Błąd podczas pobierania ilości wejść opartych na rodzaju karnetu. Dodatkowa informacja: ");
        }

        [TestMethod]
        public void GetPassTypeEntriesById_NullPassTypesDbSet_ThrowsException()
        {
            var passTypesService = new PassTypesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passTypesService.GetPassTypeEntriesById(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania ilości wejść opartych na rodzaju karnetu. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void IsKidsPass_ValidId_ReturnsPassTypeEntries()
        {
            var validPassType = passTypeList["validKidsPass"];
            var result = passTypesService.IsKidsPass(validPassType.Id).Result;
            
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsKidsPass_NotEnabledStudent_ThrowsException()
        {
            MethodReturningStruct_PassedArgument_ThrowsException(() => passTypesService.IsKidsPass(passTypeList["notEnabled"].Id), "Błąd podczas pobierania informacji czy karnet jest karnetem dziecięcym. Dodatkowa informacja: ");
        }

        [TestMethod]
        public void IsKidsPass_DeletedStudent_ThrowsException()
        {
            MethodReturningStruct_PassedArgument_ThrowsException(() => passTypesService.IsKidsPass(passTypeList["deleted"].Id), "Błąd podczas pobierania informacji czy karnet jest karnetem dziecięcym. Dodatkowa informacja: ");
        }

        [TestMethod]
        public void IsKidsPass_DeletedNotEnabledStudent_ThrowsException()
        {
            MethodReturningStruct_PassedArgument_ThrowsException(() => passTypesService.IsKidsPass(passTypeList["deletedNotEnabled"].Id), "Błąd podczas pobierania informacji czy karnet jest karnetem dziecięcym. Dodatkowa informacja: ");
        }

        [TestMethod]
        public void IsKidsPass_NullPassTypesDbSet_ThrowsException()
        {
            var passTypesService = new PassTypesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passTypesService.IsKidsPass(1).Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania informacji czy karnet jest karnetem dziecięcym. Dodatkowa informacja: "));
        }


        [TestMethod]
        public void GetPassTypes_ValidId_ReturnsPassTypeEntries()
        {
            var validPassType = passTypeList["valid"];
            var result = passTypesService.GetPassTypes().Result;
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(pt => pt.IsEntityAccesible));
            CollectionAssert.AllItemsAreUnique(result);
            CollectionAssert.AllItemsAreNotNull(result);
            
        }

        [TestMethod]
        public void GetPassTypes_NotEnabledStudent_ThrowsException()
        {
            var notEnabledPassType = passTypeList["notEnabled"];
            var result = passTypesService.GetPassTypes().Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any(pt => pt.Id == notEnabledPassType.Id));
            Assert.IsFalse(result.Any(pt => pt.IsEntityAccesible == false));
        }

        [TestMethod]
        public void GetPassTypes_DeletedStudent_ThrowsException()
        {
            var deletedPassType = passTypeList["deleted"];
            var result = passTypesService.GetPassTypes().Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any(pt => pt.Id == deletedPassType.Id));
            Assert.IsFalse(result.Any(pt => pt.IsEntityAccesible == false));
        }

        [TestMethod]
        public void GetPassTypes_DeletedNotEnabledStudent_ThrowsException()
        {
            var deletedNotEnabledPassType = passTypeList["deletedNotEnabled"];
            var result = passTypesService.GetPassTypes().Result;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any(pt => pt.Id == deletedNotEnabledPassType.Id));
            Assert.IsFalse(result.Any(pt => pt.IsEntityAccesible == false));
        }

        [TestMethod]
        public void GetPassTypes_NullPassTypesDbSet_ThrowsException()
        {
            var passTypesService = new PassTypesService(new NextLevelContext());

            var result = Assert.ThrowsException<Exception>(() => passTypesService.GetPassTypes().Result);

            Assert.IsTrue(result.Message.Contains("Błąd podczas pobierania informacji o karnetach. Dodatkowa informacja: "));
        }
    }
}
