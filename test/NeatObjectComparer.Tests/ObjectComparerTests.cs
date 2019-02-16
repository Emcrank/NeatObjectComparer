using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeatObjectComparer.Tests
{
    [TestClass]
    public class ObjectComparerTests
    {
        [TestMethod]
        public void OneType_GetDifferences_ReturnsNoDifferences()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 5000 };
            var secondToCompare = new FirstToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare>>
            {
                new PropertyComparison<FirstToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare>(
                    nameof(firstToCompare.BProperty))
            };

            var differences = new ObjectComparer<FirstToCompare>(comparisons)
                .GetDifferences(firstToCompare, secondToCompare);

            Assert.IsFalse(differences.Any());
        }

        [TestMethod]
        public void OneType_GetDifferences_ReturnsDifferences()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 1 };
            var secondToCompare = new FirstToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare>>
            {
                new PropertyComparison<FirstToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare>(
                    nameof(firstToCompare.BProperty))
            };

            var differences = new ObjectComparer<FirstToCompare>(comparisons)
                .GetDifferences(firstToCompare, secondToCompare);

            var propertyComparisons = differences.ToList();
            Assert.AreEqual(1, propertyComparisons.Count);
            Assert.AreEqual(propertyComparisons.First().FirstPropertyName, nameof(FirstToCompare.BProperty));
        }

        [TestMethod]
        public void OneType_NewIncludingOnlyDifferences_ReturnsNewOfSpecifiedType()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 1 };
            var secondToCompare = new FirstToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare>>
            {
                new PropertyComparison<FirstToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare>(
                    nameof(firstToCompare.BProperty))
            };

            var newInstance = new ObjectComparer<FirstToCompare>(comparisons)
                .NewIncludingOnlyDifferences<FirstToCompare>(firstToCompare, secondToCompare);

            Assert.IsInstanceOfType(newInstance, typeof(FirstToCompare));
            Assert.IsNull(newInstance.AProperty);
            Assert.AreEqual(newInstance.BProperty, 5000);
        }

        [TestMethod]
        public void TwoTypes_GetDifferences_ReturnsNoDifferences()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 5000 };
            var secondToCompare = new SecondToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare, SecondToCompare>>
            {
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.BProperty))
            };

            var differences = new ObjectComparer<FirstToCompare, SecondToCompare>(comparisons)
                .GetDifferences(firstToCompare, secondToCompare);

            Assert.IsFalse(differences.Any());
        }

        [TestMethod]
        public void TwoTypes_GetDifferences_ReturnsDifferences()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 1 };
            var secondToCompare = new SecondToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare, SecondToCompare>>
            {
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.BProperty))
            };

            var differences = new ObjectComparer<FirstToCompare, SecondToCompare>(comparisons)
                .GetDifferences(firstToCompare, secondToCompare);

            Assert.AreEqual(1, differences.Count());
            Assert.AreEqual(differences.First().FirstPropertyName, nameof(FirstToCompare.BProperty));
        }

        [TestMethod]
        public void TwoTypes_NewIncludingOnlyDifferences_ReturnsNewOfSpecifiedType()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 1 };
            var secondToCompare = new SecondToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare, SecondToCompare>>
            {
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.BProperty))
            };

            var newInstance = new ObjectComparer<FirstToCompare, SecondToCompare>(comparisons)
                .NewIncludingOnlyDifferences<FirstToCompare>(firstToCompare, secondToCompare);

            Assert.IsInstanceOfType(newInstance, typeof(FirstToCompare));
            Assert.IsNull(newInstance.AProperty);
            Assert.AreEqual(newInstance.BProperty, 1);
        }

        [TestMethod]
        public void TwoTypes_GetDifferences_ReturnsDifferencesWithCustomDelegate()
        {
            var firstToCompare = new FirstToCompare { AProperty = "Test", BProperty = 1 };
            var secondToCompare = new SecondToCompare { AProperty = "Test", BProperty = 5000 };

            var comparisons = new List<PropertyComparison<FirstToCompare, SecondToCompare>>
            {
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.AProperty)),
                new PropertyComparison<FirstToCompare, SecondToCompare>(
                    nameof(firstToCompare.BProperty), (f,s) => f.BProperty == 1 && s.BProperty == 5000)
            };
            
            var differences = new ObjectComparer<FirstToCompare, SecondToCompare>(comparisons)
                .GetDifferences(firstToCompare, secondToCompare);
            
            Assert.IsFalse(differences.Any());
        }

        private class FirstToCompare
        {
            public string AProperty { get; set; }

            public int BProperty { get; set; }
        }

        private class SecondToCompare
        {
            public string AProperty { get; set; }

            public int BProperty { get; set; }
        }
    }
}