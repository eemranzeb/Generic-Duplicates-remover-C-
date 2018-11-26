using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoveDuplicateItems
{
    public static class GenericHelper
    {
        private interface IHasValue<T>
        {
            bool HasValue(T value);
        }

        private struct StructHasValue<T> : IHasValue<T>, IHasValue<T?> where T : struct
        {
            public bool HasValue(T value)
            {
                return true;
            }

            public bool HasValue(T? value)
            {
                return value.HasValue;
            }
        }

        private struct ClassHasValue<T> : IHasValue<T> where T : class
        {
            public bool HasValue(T value)
            {
                return null != value;
            }
        }

        private static class ObjectValue<T>
        {
            public static readonly IHasValue<T> Value = CreateObjectValue();

            private static IHasValue<T> CreateObjectValue()
            {
                Type typeT = typeof(T);
                if (typeT.IsValueType)
                {
                    Type nullType = Nullable.GetUnderlyingType(typeT);
                    return (IHasValue<T>)Activator.CreateInstance(typeof(StructHasValue<>).MakeGenericType(nullType ?? typeT));
                }

                return (IHasValue<T>)Activator.CreateInstance(typeof(ClassHasValue<>).MakeGenericType(typeT));
            }
        }

        public static bool HasValue<T>(T value)
        {
            return ObjectValue<T>.Value.HasValue(value);
        }
    }
}
