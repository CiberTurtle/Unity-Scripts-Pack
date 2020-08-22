using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
public class DisplayAsAttribute : Attribute
{
	public string name;

	public DisplayAsAttribute(string name)
	{
		this.name = name;
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
public class SliderAttribute : Attribute
{
	public string name;
	public float min;
	public float max;

	public SliderAttribute(string name, float min, float max)
	{
		this.name = name;
		this.min = min;
		this.max = max;
	}
}
