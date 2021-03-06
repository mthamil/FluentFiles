﻿namespace FluentFiles.FixedLength
{
    using FluentFiles.Core;
    using FluentFiles.Core.Conversion;
    using FluentFiles.Core.Extensions;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides extensions for <see cref="IFixedFieldSettingsBuilder"/>.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Specifies an index at which a value actually begins within a field.
        /// </summary>
        public static IFixedFieldSettingsBuilder StartsAt(this IFixedFieldSettingsBuilder builder, ushort startIndex)
        {
            return builder.SkipWhile((_, i) => i < startIndex);
        }

        /// <summary>
        /// Specifies an index at which a value actually ends within a field.
        /// </summary>
        public static IFixedFieldSettingsBuilder EndsAt(this IFixedFieldSettingsBuilder builder, ushort endIndex)
        {
            return builder.TakeUntil((_, i) => i > endIndex);
        }

        /// <summary>
        /// Specifies that a field's value should be converted using the provided <see cref="TypeConverter"/>.
        /// </summary>
        /// <param name="builder">The settings builder.</param>
        /// <param name="typeConverter">The type converter to use.</param>
        public static IFixedFieldSettingsBuilder WithTypeConverter(this IFixedFieldSettingsBuilder builder, TypeConverter typeConverter)
        {
            return builder.WithConverter(new TypeConverterAdapter(typeConverter));
        }

#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Specifies that a field's value should be converted using a new instance of the type <typeparamref name="TConverter"/>.
        /// </summary>
        /// <typeparam name="TConverter">The type of <see cref="ITypeConverter"/> to use for conversion.</typeparam>
        public static IFixedFieldSettingsBuilder WithTypeConverter<TConverter>(this IFixedFieldSettingsBuilder builder) where TConverter : ITypeConverter, new()
        {
            return builder.WithTypeConverter(ReflectionHelper.CreateInstance<TConverter>(true));
        }

        /// <summary>
        ///  Specifies that a field's value should be converted using the provided <see cref="ITypeConverter"/> implementation.
        /// </summary>
        /// <param name="builder">The settings builder.</param>
        /// <param name="converter">The converter to use.</param>
        public static IFixedFieldSettingsBuilder WithTypeConverter(this IFixedFieldSettingsBuilder builder, ITypeConverter converter)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            return builder.WithConverter(new ITypeConverterAdapter(converter));
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
