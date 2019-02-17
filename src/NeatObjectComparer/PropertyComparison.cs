using System;
using System.Reflection;

namespace NeatObjectComparer
{
    /// <summary>
    /// Class that holds logic to compare two properties on two of the same type.
    /// </summary>
    /// <typeparam name="T">The type of the instance the property resides on.</typeparam>
    public class PropertyComparison<T> : PropertyComparison<T, T>
    {
        /// <summary>
        /// Initializes an instance of <see cref="PropertyComparison{T}" />
        /// with the specified parameter.
        /// This constructor will use the default <code>obj.Equals</code>
        /// method to evaluate equality.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public PropertyComparison(string propertyName) : base(propertyName) { }

        /// <summary>
        /// Initializes an instance of <see cref="PropertyComparison{T}" />
        /// with the specified parameters.
        /// </summary>
        /// <param name="propertyName">The name of the property. If its named differently on both types, use other constructor.</param>
        /// <param name="isPropertyEqual">The delegate that is invoked to evaluate whether the property is equal.</param>
        public PropertyComparison(string propertyName, IsPropertyEqual isPropertyEqual) : base(
            propertyName,
            isPropertyEqual) { }

        /// <summary>
        /// Initializes an instance of <see cref="PropertyComparison{T}" />
        /// with the specified parameters.
        /// </summary>
        /// <param name="firstPropertyName">The name of the property on the first type.</param>
        /// <param name="secondPropertyName">The name of the property on the second type.</param>
        /// <param name="isPropertyEqual">The delegate that is invoked to evaluate whether the property is equal.</param>
        public PropertyComparison(string firstPropertyName, string secondPropertyName,
            IsPropertyEqual isPropertyEqual) : base(firstPropertyName, secondPropertyName, isPropertyEqual) { }
    }

    /// <summary>
    /// Class that holds logic to compare two properties on two types.
    /// </summary>
    /// <typeparam name="TFirst">Type of the first instance on which the property resides.</typeparam>
    /// <typeparam name="TSecond">Type of the second instance on which the property resides.</typeparam>
    public class PropertyComparison<TFirst, TSecond>
    {
        /// <summary>
        /// The delegate that is invoked to evaluate whether the property is equal.
        /// Should return true if they are equal; otherwise false.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        /// <returns>Should return true if the properties are equal.</returns>
        public delegate bool IsPropertyEqual(TFirst firstInstance, TSecond secondInstance);

        private static readonly Func<string, string, IsPropertyEqual> defaultIsPropertyEqual = (p1, p2) =>
        {
            return (x, y) =>
            {
                var firstValue = GetPropertyInfo<TFirst>(p1).GetValue(x);
                var secondValue = GetPropertyInfo<TSecond>(p2).GetValue(y);
                return firstValue.Equals(secondValue);
            };
        };

        private readonly IsPropertyEqual isPropertyEqual;

        /// <summary>
        /// Initializes an instance of <see cref="PropertyComparison{TFirst,TSecond}" />
        /// with the specified parameter.
        /// This constructor will use the default <code>obj.Equals</code>
        /// method to evaluate equality.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public PropertyComparison(string propertyName) : this(
            propertyName,
            defaultIsPropertyEqual(propertyName, propertyName)) { }

        /// <summary>
        /// Initializes an instance of <see cref="PropertyComparison{TFirst,TSecond}" />
        /// with the specified parameters.
        /// </summary>
        /// <param name="propertyName">The name of the property. If its named differently on both types, use other constructor.</param>
        /// <param name="isPropertyEqual">The delegate that is invoked to evaluate whether the property is equal.</param>
        public PropertyComparison(string propertyName, IsPropertyEqual isPropertyEqual)
            : this(propertyName, propertyName, isPropertyEqual) { }

        /// <summary>
        /// Initializes an instance of <see cref="PropertyComparison{TFirst,TSecond}" />
        /// with the specified parameters.
        /// </summary>
        /// <param name="firstPropertyName">The name of the property on the first type.</param>
        /// <param name="secondPropertyName">The name of the property on the second type.</param>
        /// <param name="isPropertyEqual">The delegate that is invoked to evaluate whether the property is equal.</param>
        public PropertyComparison(string firstPropertyName, string secondPropertyName,
            IsPropertyEqual isPropertyEqual)
        {
            this.isPropertyEqual = isPropertyEqual;
            FirstPropertyInfo = GetPropertyInfo<TFirst>(firstPropertyName);
            SecondPropertyInfo = GetPropertyInfo<TSecond>(secondPropertyName);
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo" /> for the first instance's property.
        /// </summary>
        public PropertyInfo FirstPropertyInfo { get; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo" /> for the second instance's property.
        /// </summary>
        public PropertyInfo SecondPropertyInfo { get; }

        /// <summary>
        /// Gets property information for a specified property name.
        /// </summary>
        /// <typeparam name="T">The type on which the property resides.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>An instance of <see cref="PropertyInfo" /> for the specified property.</returns>
        private static PropertyInfo GetPropertyInfo<T>(string propertyName)
        {
            var type = typeof(T);

            var propertyInfo = type.GetProperty(propertyName);
            if(propertyInfo == null)
                throw new MissingMemberException(type.Name, propertyName);

            return propertyInfo;
        }

        /// <summary>
        /// Executes the compare delegate to determine whether the properties are equal.
        /// </summary>
        /// <param name="firstInstance">The first instance.</param>
        /// <param name="secondInstance">The second instance.</param>
        public PropertyComparisonResult Compare(TFirst firstInstance, TSecond secondInstance)
        {
            return new PropertyComparisonResult
            {
                IsEqual = isPropertyEqual(firstInstance, secondInstance),

                FirstValue = Convert.ChangeType(
                    FirstPropertyInfo.GetValue(firstInstance),
                    FirstPropertyInfo.PropertyType),

                SecondValue = Convert.ChangeType(
                    SecondPropertyInfo.GetValue(secondInstance),
                    SecondPropertyInfo.PropertyType),

                FirstPropertyInfo = FirstPropertyInfo,
                SecondPropertyInfo = SecondPropertyInfo,
                FirstType = typeof(TFirst),
                SecondType = typeof(TSecond)
            };
        }
    }
}