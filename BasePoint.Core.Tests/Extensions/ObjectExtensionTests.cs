using BasePoint.Core.Extensions;
using BasePoint.Core.Tests.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BasePoint.Core.Tests.Extensions
{
    public class ObjectExtensionTests
    {

        [Fact]
        public void CoalesceSet_ListOfIntsValuesGiven_ShouldSetTheFirstNonNullableValue()
        {
            var value = ObjectExtension.Coalesce([null, 2, 3]);

            value.Should().Be(2);
        }

        [Fact]
        public void CoalesceSet_ListOfEntitiesValuesGiven_ShouldSetTheFirstNonNullableValue()
        {
            var entity2 = new SampleEntity() { SampleName = "SampleName2" };
            var entity3 = new SampleEntity() { SampleName = "SampleName3" };

            var entity1 = ObjectExtension.Coalesce([null, entity2, entity3]);

            entity1.Should().Be(entity2);
        }
    }
}