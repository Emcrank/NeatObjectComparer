using System;
using System.Collections.Generic;
using System.Linq;

namespace NeatObjectComparer
{
    /// <summary>
    /// Class that will compare two instances of the same type.
    /// </summary>
    /// <typeparam name="T">The type of both instances to compare.</typeparam>
    public class ObjectComparer<T> : ObjectComparer<T, T>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ObjectComparer{T}" />.
        /// </summary>
        /// <param name="comparisons">A collection of <see cref="PropertyComparison{TFirst,TSecond}" /></param>
        public ObjectComparer(IEnumerable<PropertyComparison<T>> comparisons)
            : base(comparisons.Cast<PropertyComparison<T, T>>().ToList()) { }
    }

    /// <summary>
    /// Class that will compare two instances of a different type.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first instance to compare.</typeparam>
    /// <typeparam name="TSecond">The type of the second instance to compare.</typeparam>
    public class ObjectComparer<TFirst, TSecond>
    {
        private readonly IList<PropertyComparison<TFirst, TSecond>> comparisons;

        /// <summary>
        /// Initializes an instance of <see cref="ObjectComparer{TFirst,TSecond}" />
        /// with the specified comparisons.
        /// </summary>
        /// <param name="comparisons">The collection of comparisons to execute.</param>
        public ObjectComparer(IList<PropertyComparison<TFirst, TSecond>> comparisons)
        {
            this.comparisons = comparisons;
        }

        /// <summary>
        /// Compares the two instances.
        /// </summary>
        /// <param name="firstInstance">The first instance to compare.</param>
        /// <param name="secondInstance">The second instance to compare.</param>
        /// <returns>All instances of <see cref="PropertyComparison{TFirst,TSecond}" /> passed in the constructor.</returns>
        public IEnumerable<PropertyComparison<TFirst, TSecond>> Compare(TFirst firstInstance, TSecond secondInstance)
        {
            foreach(var propertyComparison in comparisons)
                propertyComparison.Compare(firstInstance, secondInstance);

            return comparisons;
        }

        /// <summary>
        /// Compares the two instances and returns the differences.
        /// </summary>
        /// <param name="firstInstance">The first instance to compare.</param>
        /// <param name="secondInstance">The second instance to compare.</param>
        /// <returns>An IEnumerable of <see cref="PropertyComparison{TFirst,TSecond}" /> containing the differences.</returns>
        public IEnumerable<PropertyComparison<TFirst, TSecond>> GetDifferences(TFirst firstInstance,
            TSecond secondInstance)
        {
            return Compare(firstInstance, secondInstance).Where(x => x.HasDifference);
        }

        /// <summary>
        /// Compares the two instances and returns new instance of <see cref="T" />
        /// with only the different properties set.
        /// </summary>
        /// <typeparam name="T">The type to construct. Must be of <see cref="TFirst" /> or <see cref="TSecond" /></typeparam>
        /// <param name="firstInstance">The first instance to compare.</param>
        /// <param name="secondInstance">The second instance to compare.</param>
        /// <returns>An instance of <see cref="T" /> with only the different properties set.</returns>
        public T NewIncludingOnlyDifferences<T>(TFirst firstInstance, TSecond secondInstance) where T : new()
        {
            if(typeof(T) != typeof(TFirst) && typeof(T) != typeof(TSecond))
                throw new InvalidOperationException("The generic parameter must be of type TFirst or TSecond.");

            var differences = GetDifferences(firstInstance, secondInstance);
            var t = new T();

            foreach(var difference in differences)
            {
                if(typeof(TFirst) == typeof(TSecond))
                {
                    difference.SecondPropertyInfo.SetValue(t, difference.SecondValue);
                    continue;
                }

                if(typeof(T) == typeof(TFirst))
                    difference.FirstPropertyInfo.SetValue(t, difference.FirstValue);

                if(typeof(T) == typeof(TSecond))
                    difference.SecondPropertyInfo.SetValue(t, difference.SecondValue);
            }

            return t;
        }
    }
}