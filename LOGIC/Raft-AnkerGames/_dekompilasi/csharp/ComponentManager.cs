using UnityEngine;

public class ComponentManager<T> : MonoBehaviour
{
	private static T component;

	private static T component2;

	public static T Value
	{
		get
		{
			return component;
		}
		set
		{
			component = value;
		}
	}

	public static T ValueConsole
	{
		get
		{
			return component2;
		}
		set
		{
			component2 = value;
		}
	}
}
