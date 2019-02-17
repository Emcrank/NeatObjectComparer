using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace NeatObjectComparer
{
    /// <summary>
    /// Class that holds the result of two properties being compared.
    /// </summary>
    [Serializable]
    [DataContract(Name = "PropertyComparisonResult", Namespace = Namespace.DataContract)]
    public class PropertyComparisonResult
    {
        /// <summary>
        /// Gets the value for the second instances property.
        /// </summary>
        [DataMember(Name = "FirstValue", Order = 0)]
        public dynamic FirstValue { get; internal set; }

        /// <summary>
        /// Gets the value for the first instances property.
        /// </summary>
        [DataMember(Name = "SecondValue", Order = 1)]
        public dynamic SecondValue { get; internal set; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo" /> for the first instance's property.
        /// </summary>
        [DataMember(Name = "FirstPropertyInfo", Order = 2)]
        public PropertyInfo FirstPropertyInfo { get; internal set; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo" /> for the second instance's property.
        /// </summary>
        [DataMember(Name = "SecondPropertyInfo", Order = 3)]
        public PropertyInfo SecondPropertyInfo { get; internal set; }

        /// <summary>
        /// Gets the type of the first instance.
        /// </summary>
        [DataMember(Name = "FirstType", Order = 4)]
        public Type FirstType { get; internal set; }

        /// <summary>
        /// Gets the type of the second instance.
        /// </summary>
        [DataMember(Name = "SecondType", Order = 5)]
        public Type SecondType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the property values are equal.
        /// </summary>
        [DataMember(Name = "IsEqual", Order = 6)]
        public bool IsEqual { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the property values have a difference.
        /// </summary>
        [IgnoreDataMember]
        public bool HasDifference => !IsEqual;

        /// <summary>
        /// Gets the first instance's property name.
        /// </summary>
        [IgnoreDataMember]
        public string FirstPropertyName => FirstPropertyInfo.Name;

        /// <summary>
        /// Gets the second instance's property name.
        /// </summary>
        [IgnoreDataMember]
        public string SecondPropertyName => SecondPropertyInfo.Name;
    }
}