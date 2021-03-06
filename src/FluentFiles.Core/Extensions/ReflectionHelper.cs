namespace FluentFiles.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Class ReflectionHelper.
    /// </summary>
    public static class ReflectionHelper
    {
        private static readonly object CacheLock = new object();
        private static readonly Dictionary<ConstructorInfo, Delegate> Cache = new Dictionary<ConstructorInfo, Delegate>();

        /// <summary>
        /// Creates an instance of type <typeparamref name="T"/> using the default constructor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cached">if set to <c>true</c> [cached].</param>
        /// <returns>An instance of type <typeparamref name="T"/>.</returns>
        public static T CreateInstance<T>(bool cached = false) => (T)CreateInstance(typeof(T), cached);

        /// <summary>
        /// Creates an instance of type <paramref name="targetType"/> using the default constructor.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="cached">if set to <c>true</c> [cached].</param>
        /// <returns>An instance of type <paramref name="targetType"/>.</returns>
        public static object CreateInstance(Type targetType, bool cached = false)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            return CreateInstance(targetType.GetConstructor(Type.EmptyTypes), cached);
        }

        /// <summary>
        /// Creates an instance of type <paramref name="targetType"/> using the specified constructor parameters.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="cached">if set to <c>true</c> [cached].</param>
        /// <param name="parameters">The constructor arguments.</param>
        /// <returns>An instance of type <paramref name="targetType"/>.</returns>
        public static object CreateInstance(Type targetType, bool cached = false, params object[] parameters)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            if (parameters == null || parameters.Length == 0) 
                return CreateInstance(targetType, cached);

            var constructor = targetType.GetConstructor(parameters.Select(a => a.GetType()).ToArray());

            return CreateInstance(constructor, cached, parameters);
        }

        private static object CreateInstance(ConstructorInfo constructor, bool cached, object[]? parameters = null)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));

            var hasArguments = parameters?.Length > 0;

            Delegate ctor;
            lock (CacheLock)
            {
                if (!Cache.TryGetValue(constructor, out ctor) || !cached)
                {
                    if (hasArguments)
                    {
                        var ctorArgs = constructor.GetParameters().Select((param, index) => Expression.Parameter(param.ParameterType, String.Format("Param{0}", index))).ToArray();
                        // ReSharper disable once CoVariantArrayConversion
                        ctor = Expression.Lambda(Expression.New(constructor, ctorArgs), ctorArgs).Compile();
                    }
                    else
                    {
                        ctor = Expression.Lambda(Expression.New(constructor)).Compile();
                    }
                }

                if (cached) CacheCtor(constructor, ctor);
            }
            return hasArguments ? ctor.DynamicInvoke(parameters) : ctor.DynamicInvoke();
        }

        private static void CacheCtor(ConstructorInfo key, Delegate ctor) 
        { 
            if (!Cache.ContainsKey(key))
                Cache.Add(key, ctor); 
        }

        /// <summary>
        /// Creates a delegate that instantiates a type using its parameterless constructor.
        /// </summary>
        /// <param name="targetType">The type to instantiate.</param>
        public static Func<object> CreateConstructor(Type targetType)
        {
            var constructor = targetType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                throw new ArgumentException("A parameterless constructor is required.", nameof(targetType));

            var newExpression = Expression.Lambda<Func<object>>(Expression.New(constructor));
            return newExpression.Compile();
        }

        /// <summary>
        /// Creates a delegate for accessing the value of a given member.
        /// </summary>
        /// <param name="member">The member to access.</param>
        public static Func<object, object?> CreateMemberGetter(MemberInfo member)
        {
            var parameter = Expression.Parameter(typeof(object));
            var memberExpression = Expression.PropertyOrField(Expression.Convert(parameter, member.DeclaringType), member.Name);

            var getter = Expression.Lambda<Func<object, object?>>(
                memberExpression.Type.IsValueType
                    ? Expression.Convert(memberExpression, typeof(object))
                    : (Expression)memberExpression, parameter);
            return getter.Compile();
        }

        /// <summary>
        /// Creates a delegate for assigning the value of a given member.
        /// </summary>
        /// <param name="member">The member to set.</param>
        public static Action<object, object?> CreateMemberSetter(MemberInfo member)
        {
            var targetParam = Expression.Parameter(typeof(object));
            var valueParam = Expression.Parameter(typeof(object), "value");
            var memberExpression = Expression.PropertyOrField(Expression.Convert(targetParam, member.DeclaringType), member.Name);

            var setter = Expression.Lambda<Action<object, object?>>(
                Expression.Assign(memberExpression, Expression.Convert(valueParam, memberExpression.Type)), targetParam, valueParam);
            return setter.Compile();
        }
    }
}