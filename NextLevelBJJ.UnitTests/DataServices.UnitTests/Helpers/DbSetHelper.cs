using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;
using Moq;
using NextLevelBJJ.DataServices.Models.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelBJJ.UnitTests.DataServices.UnitTests.Helpers
{
    public static class DbSetHelper
    {
        public static Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        public static Mock<DbSet<T>> CreateDbSetMock<T>(IDictionary<string ,T> elements) where T : class
        {
            return CreateDbSetMock(elements.Select(v => v.Value));
        }

        public static IDictionary<string ,T> CreateDataSeed<T>() where T : class, IExistanceFields
        {
            var fixture = ConfiguredFixture();

            var validObject = fixture.Build<T>()
                                    .With(p => p.IsDeleted, false)
                                    .With(p => p.IsEnabled, true)
                                    .Create();
            var notEnabledObject = fixture.Build<T>()
                                    .With(p => p.IsDeleted, false)
                                    .With(p => p.IsEnabled, false)
                                    .Create();
            var deletedObject = fixture.Build<T>()
                                    .With(p => p.IsDeleted, true)
                                    .With(p => p.IsEnabled, true)
                                    .Create();
            var deletedNotEnabledObject = fixture.Build<T>()
                                    .With(p => p.IsDeleted, true)
                                    .With(p => p.IsEnabled, false)
                                    .Create();

            return new Dictionary<string, T> {
                { "valid", validObject },
                { "notEnabled", notEnabledObject },
                { "deleted", deletedObject },
                { "deletedNotEnabled", deletedNotEnabledObject }
            };
        }

        public static Fixture ConfiguredFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                            .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}
