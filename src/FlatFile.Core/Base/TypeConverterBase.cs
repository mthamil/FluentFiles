﻿using System;
using System.Reflection;

namespace FlatFile.Core.Base
{
    /// <summary>
    /// A generic base class for converting between strings and a given type.
    /// </summary>
    /// <typeparam name="TValue">The type to convert to and from a string.</typeparam>
    public abstract class TypeConverterBase<TValue> : ITypeConverter
    {
        public virtual bool CanConvertFrom(Type type)
        {
            return type == typeof(string) || type == typeof(TValue);
        }

        public virtual bool CanConvertTo(Type type)
        {
            return type == typeof(string) || type == typeof(TValue);
        }

        public object ConvertFromString(string source, PropertyInfo targetProperty)
        {
            return ConvertFrom(source, targetProperty);
        }

        protected abstract TValue ConvertFrom(string source, PropertyInfo targetProperty);

        public string ConvertToString(object source, PropertyInfo sourceProperty)
        {
            return ConvertTo((TValue)source, sourceProperty);
        }

        protected abstract string ConvertTo(TValue source, PropertyInfo sourceProperty);
    }
}