using Xunit;

namespace FluentFiles.Tests.FixedLength
{
    using FluentFiles.Core.Base;
    using FluentFiles.FixedLength;
    using FluentFiles.FixedLength.Implementation;
    using FluentFiles.Tests.Base.Entities;
    using FluentAssertions;
    using System;
    using System.Globalization;
    using System.Reflection;

    public class FixedLengthLineParserTests
    {
        private readonly FixedLengthLineParser parser;
        private readonly IFixedLayout<TestObject> layout; 

        public FixedLengthLineParserTests()
        {
            layout = new FixedLayout<TestObject>()
                    .WithMember(o => o.Id, set => set.WithLength(5).WithLeftPadding('0'))
                    .WithMember(o => o.Description, set => set.WithLength(25).WithRightPadding(' '))
                    .WithMember(o => o.NullableInt, set => set.WithLength(5).AllowNull("=Null").WithLeftPadding('0'));

            parser = new FixedLengthLineParser(layout);
        }

        [Theory]
        [InlineData("00001Description 1            00003", 1, "Description 1", 3)]
        [InlineData("00005Description 5            =Null", 5, "Description 5", null)]
        public void ParserShouldReadAnyValidString(string inputString, int id, string description, int? nullableInt)
        {
            var entry = new TestObject();

            var parsedEntity = parser.ParseLine(inputString, entry);

            parsedEntity.Id.Should().Be(id);
            parsedEntity.Description.Should().Be(description);
            parsedEntity.NullableInt.Should().Be(nullableInt);
        }

        [Theory]
        [InlineData("00001Description 1            00003", 1, "Description 1", 3)]
        [InlineData("00005Description 5            ", 5, "Description 5", null)]
        [InlineData("00005Description 5            3", 5, "Description 5", 3)]
        public void ParserShouldSetValueNullValueIfStringIsToShort(string inputString, int id, string description, int? nullableInt)
        {
            var entry = new TestObject();

            var parsedEntity = parser.ParseLine(inputString, entry);

            parsedEntity.Id.Should().Be(id);
            parsedEntity.Description.Should().Be(description);
            parsedEntity.NullableInt.Should().Be(nullableInt);
        }

        [Fact]
        public void ParserShouldUseTypeConverter()
        {
            layout.WithMember(o => o.Id, set => set.WithLength(4).WithTypeConverter<IdHexConverter>())
                  .WithMember(o => o.Description, set => set.WithLength(25).AllowNull(string.Empty))
                  .WithMember(o => o.NullableInt, set => set.WithLength(5).AllowNull(string.Empty));

            var entry = new TestObject();
            var parsedEntity = parser.ParseLine("BEEF", entry);

            parsedEntity.Id.Should().Be(48879);
        }

        [Fact]
        public void ParserShouldUseConversionFunction()
        {
            layout.WithMember(o => o.Id, set => set.WithLength(4).WithConversionFromString(s => Int32.Parse(s, NumberStyles.AllowHexSpecifier)))
                  .WithMember(o => o.Description, set => set.WithLength(25).AllowNull(string.Empty))
                  .WithMember(o => o.NullableInt, set => set.WithLength(5).AllowNull(string.Empty));

            var entry = new TestObject();
            var parsedEntity = parser.ParseLine("BEEF", entry);

            parsedEntity.Id.Should().Be(48879);
        }

        class IdHexConverter : TypeConverterBase<int>
        {
            protected override int ConvertFrom(string source, PropertyInfo targetProperty)
            {
                return Int32.Parse(source, NumberStyles.AllowHexSpecifier);
            }

            protected override string ConvertTo(int source, PropertyInfo sourceProperty)
            {
                return source.ToString("X");
            }
        }
    }
}