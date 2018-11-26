using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoveDuplicateItems
{
    public sealed class EqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> _keySelector;
        private readonly IEqualityComparer<TKey> _keyComparer;

        public EqualityComparer(Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        {
            if (null == keySelector) throw new ArgumentNullException("keySelector");

            _keySelector = keySelector;
            _keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
        }

        public EqualityComparer(Func<T, TKey> keySelector)
            : this(keySelector, null)
        {
        }

        public bool Equals(T x, T y)
        {
            if (!GenericHelper.HasValue(x)) return !GenericHelper.HasValue(y);
            if (!GenericHelper.HasValue(y)) return false;

            TKey xKey = _keySelector(x);
            TKey yKey = _keySelector(y);

            if (!GenericHelper.HasValue(xKey)) return !GenericHelper.HasValue(yKey);
            if (!GenericHelper.HasValue(yKey)) return false;

            return _keyComparer.Equals(xKey, yKey);
        }

        public int GetHashCode(T obj)
        {
            if (!GenericHelper.HasValue(obj)) return 0;

            TKey key = _keySelector(obj);
            if (!GenericHelper.HasValue(key)) return 0;

            return _keyComparer.GetHashCode(key);
        }
    }

    public static class EqualityComparer
    {
        public static IEqualityComparer<T> Create<T, TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        {
            if (null == keySelector) throw new ArgumentNullException("keySelector");
            return new EqualityComparer<T, TKey>(keySelector, keyComparer);
        }

        public static IEqualityComparer<T> Create<T, TKey>(Func<T, TKey> keySelector)
        {
            return Create(keySelector, null);
        }
    }
}
