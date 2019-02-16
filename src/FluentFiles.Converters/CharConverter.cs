﻿namespace FluentFiles.Converters
{
    using FluentFiles.Core.Conversion;
    using System;

    /// <summary>
    /// Converts between characters and strings.
    /// </summary>
    public sealed class CharConverter : ConverterBase<char>
    {
        /// <summary>
        /// Converts a string to a character.
        /// </summary>
        /// <param name="context">Provides information about a field deserialization operation.</param>
        /// <returns>A string as a single character.</returns>
        protected override char ConvertFrom(in FieldDeserializationContext context)
        {
            var trimmed = context.Source;
            if (trimmed.Length > 1)
                trimmed = trimmed.Trim();

            if (trimmed.Length > 0)
                return trimmed[0];

            return char.MinValue;
        }

        /// <summary>
        /// Converts a character to a string.
        /// </summary>
        /// <param name="context">Provides information about a field serialization operation.</param>
        /// <returns>A character as a full string.</returns>
        protected override string ConvertTo(in FieldSerializationContext<char> context)
        {
            if (context.Source == char.MinValue)
                return string.Empty;

            return new string(context.Source, 1);
        }
    }
}
