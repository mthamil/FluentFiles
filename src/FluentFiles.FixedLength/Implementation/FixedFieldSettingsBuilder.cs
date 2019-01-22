using System;

namespace FluentFiles.FixedLength.Implementation
{
    using System.Reflection;
    using FluentFiles.Core;
    using FluentFiles.Core.Conversion;
    using FluentFiles.Core.Extensions;

    public class FixedFieldSettingsBuilder : IFixedFieldSettingsBuilder
    {
        private readonly PropertyInfo _property;
        private bool _isNullable;
        private string _nullValue;
        private bool _truncateIfExceedFieldLength;
        private Func<string, string> _stringNormalizer;
        private int _length;
        private char _paddingChar;
        private bool _padLeft;
        private Func<char, int, bool> _skipWhile;
        private Func<char, int, bool> _takeUntil;
        private IFieldValueConverter _converter;

        public FixedFieldSettingsBuilder(PropertyInfo property)
        {
            _property = property;
        }

        public IFixedFieldSettingsBuilder TruncateFieldContentIfExceedLength()
        {
            _truncateIfExceedFieldLength = true;
            return this;
        }

        public IFixedFieldSettingsBuilder WithStringNormalizer(Func<string, string> stringNormalizer)
        {
            _stringNormalizer = stringNormalizer;
            return this;
        }

        public IFixedFieldSettingsBuilder WithLength(int length)
        {
            _length = length;
            return this;
        }

        public IFixedFieldSettingsBuilder WithLeftPadding(char paddingChar)
        {
            _paddingChar = paddingChar;
            _padLeft = true;
            return this;
        }

        public IFixedFieldSettingsBuilder WithRightPadding(char paddingChar)
        {
            _paddingChar = paddingChar;
            _padLeft = false;
            return this;
        }

        public IFixedFieldSettingsBuilder SkipWhile(Func<char, int, bool> predicate)
        {
            _skipWhile = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return this;
        }

        public IFixedFieldSettingsBuilder TakeUntil(Func<char, int, bool> predicate)
        {
            _takeUntil = predicate ?? throw new ArgumentNullException(nameof(predicate));
            return this;
        }

        public IFixedFieldSettingsBuilder AllowNull(string nullValue)
        {
            _isNullable = true;
            _nullValue = nullValue;
            return this;
        }

        public IFixedFieldSettingsBuilder WithTypeConverter<TConverter>() where TConverter : ITypeConverter, new()
        {
            return WithTypeConverter(ReflectionHelper.CreateInstance<TConverter>(true));
        }

        public IFixedFieldSettingsBuilder WithTypeConverter(ITypeConverter converter)
        {
            var typeConverter = converter ?? throw new ArgumentNullException(nameof(converter));
            return WithConverter(new FieldValueConverterAdapter(typeConverter));
        }

        public IFixedFieldSettingsBuilder WithConverter<TConverter>() where TConverter : IFieldValueConverter, new()
        {
            return WithConverter(ReflectionHelper.CreateInstance<TConverter>(true));
        }

        public IFixedFieldSettingsBuilder WithConverter(IFieldValueConverter converter)
        {
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
            return this;
        }

        public IFixedFieldSettingsBuilder WithConversionFromString<TProperty>(ConvertFromString<TProperty> conversion)
        {
            if (_converter == null)
                _converter = new DelegatingConverter<TProperty>();

            if (_converter is DelegatingConverter<TProperty> delegatingConverter)
                delegatingConverter.ConversionFromString = conversion;
            else
                throw new InvalidOperationException("A converter has already been explicitly set.");

            return this;
        }

        public IFixedFieldSettingsBuilder WithConversionToString<TProperty>(ConvertToString<TProperty> conversion)
        {
            if (_converter == null)
                _converter = new DelegatingConverter<TProperty>();

            if (_converter is DelegatingConverter<TProperty> delegatingConverter)
                delegatingConverter.ConversionToString = conversion;
            else
                throw new InvalidOperationException("A converter has already been explicitly set.");

            return this;
        }

        public IFixedFieldSettingsContainer Build()
        {
            return new FixedFieldSettings(_property)
            {
                IsNullable = _isNullable,
                NullValue = _nullValue,
                PaddingChar = _paddingChar,
                SkipWhile = _skipWhile,
                TakeUntil = _takeUntil,
                PadLeft = _padLeft,
                Length = _length,
                StringNormalizer = _stringNormalizer,
                TruncateIfExceedFieldLength = _truncateIfExceedFieldLength,
                Converter = _converter
            };
        }
    }
}