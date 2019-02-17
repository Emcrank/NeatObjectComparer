# NeatObjectComparer - A library for comparing objects.
**Compares two objects using the default `Equals` method or by defining your own comparison methods.**

## Usage
Comparing two instances of the same type.
```csharp
//Take this class as an example.
public class AClassToBeCompared
{
	public string AProperty { get; set; }
	public int BProperty { get; set; }
	public static IList<PropertyComparison<AClassToBeCompared>> Comparisons => new List<PropertyComparison<AClassToBeCompared>>
	{
		//Passing only the property name will use the Equals method to compare the two instances.
		new PropertyComparison<AClassToBeCompared>(nameof(AProperty)),
		//Using a custom equality evaluation method. Should return true when they are equal. 
		//f and s are the two instances being compared.
		new PropertyComparison<AClassToBeCompared>(nameof(BProperty), (f,s) => f.BProperty == s.BProperty)
	};
}
```
```csharp
var firstInstance = new AClassToBeCompared
{
	AProperty = "Foo",
	BProperty = 50000
};
var secondInstance = new AClassToBeCompared
{
	AProperty = "Foo",
	BProperty = 30000
};

//Instantiate the comparer.
var comparer = new ObjectComparer<AClassToBeCompared>(AClassToBeCompared.Comparisons);
//Get the differences.
var comparisons = comparer.Compare(firstInstance, secondInstance);

foreach(var comparison in comparisons) 
{
	Console.WriteLine("{0} = {1}", comparison.FirstPropertyName, comparison.HasDifference);
}

//Output:
//AProperty = false
//BProperty = true
```
