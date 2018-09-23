﻿namespace FluentFiles.Delimited.Implementation
{
    using System.Linq;
    using System.Text;
    using FluentFiles.Core.Base;

    public class DelimitedLineBuilder :
        LineBuilderBase<IDelimitedLayoutDescriptor, IDelimitedFieldSettingsContainer>, 
        IDelimitedLineBuilder
    {
        public DelimitedLineBuilder(IDelimitedLayoutDescriptor descriptor)
            : base(descriptor)
        {
        }

        public override string BuildLine<T>(T entry)
        {
            var builder = new StringBuilder();
            var line = Descriptor.Fields.Aggregate(builder,
                (current, field) => current.Append(current.Length > 0 ? Descriptor.Delimiter : string.Empty)
                                           .Append(GetStringValueFromField(field, field.GetValueOf(entry))));
            return line.ToString();
        }

        protected override string PostprocessFieldValue(IDelimitedFieldSettingsContainer field, string value)
        {
            var quotes = Descriptor.Quotes;
            if (!string.IsNullOrEmpty(quotes))
            {
                value = string.Format("{0}{1}{0}", quotes, value);
            }
            return value;
        }
    }
}