using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RG.Utils;

public static class InterfaceHelper
{
	private static Dictionary<Type, List<Type>> _interfaceToComponentMapping;

	private static Type[] _allTypes;

	static InterfaceHelper()
	{
		InitInterfaceToComponentMapping();
	}

	private static void InitInterfaceToComponentMapping()
	{
		_interfaceToComponentMapping = new Dictionary<Type, List<Type>>();
		_allTypes = GetAllTypes();
		Type[] allTypes = _allTypes;
		foreach (Type type in allTypes)
		{
			if (!type.IsInterface)
			{
				continue;
			}
			string text = type.ToString().ToLower();
			if (text.Contains("unity") || text.Contains("system.") || text.Contains("mono.") || text.Contains("mono.") || text.Contains("icsharpcode.") || text.Contains("nsubstitute") || text.Contains("nunit.") || text.Contains("microsoft.") || text.Contains("boo.") || text.Contains("serializ") || text.Contains("json") || text.Contains("log.") || text.Contains("logging") || text.Contains("test") || text.Contains("editor") || text.Contains("debug"))
			{
				continue;
			}
			IList<Type> typesInheritedFromInterface = GetTypesInheritedFromInterface(type);
			if (typesInheritedFromInterface.Count <= 0)
			{
				continue;
			}
			List<Type> list = new List<Type>();
			foreach (Type item in typesInheritedFromInterface)
			{
				if (!item.IsInterface && (typeof(Component) == item || item.IsSubclassOf(typeof(Component))) && !list.Contains(item))
				{
					list.Add(item);
				}
			}
			_interfaceToComponentMapping.Add(type, list);
		}
	}

	private static Type[] GetAllTypes()
	{
		List<Type> list = new List<Type>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly in assemblies)
		{
			list.AddRange(assembly.GetTypes());
		}
		return list.ToArray();
	}

	private static IEnumerable<Type> GetTypesInheritedFromInterface<T>() where T : class
	{
		return GetTypesInheritedFromInterface(typeof(T));
	}

	private static IList<Type> GetTypesInheritedFromInterface(Type type)
	{
		if (_allTypes == null)
		{
			_allTypes = GetAllTypes();
		}
		List<Type> list = new List<Type>();
		Type[] allTypes = _allTypes;
		foreach (Type type2 in allTypes)
		{
			if (type.IsAssignableFrom(type2) && type2.IsSubclassOf(typeof(Component)))
			{
				list.Add(type2);
			}
		}
		return list;
	}

	public static IList<T> FindObjects<T>(bool firstOnly = false) where T : class
	{
		List<T> list = new List<T>();
		List<Type> list2 = _interfaceToComponentMapping[typeof(T)];
		if (list2 == null || list2.Count <= 0)
		{
			Debug.LogError("No descendants found for type " + typeof(T));
			return null;
		}
		foreach (Type item2 in list2)
		{
			UnityEngine.Object[] array = ((!firstOnly) ? UnityEngine.Object.FindObjectsOfType(item2) : new UnityEngine.Object[1] { UnityEngine.Object.FindObjectOfType(item2) });
			if (array == null || array.Length == 0)
			{
				continue;
			}
			List<T> list3 = new List<T>();
			UnityEngine.Object[] array2 = array;
			foreach (UnityEngine.Object obj in array2)
			{
				if (!(obj is T item))
				{
					Debug.LogError("Unable to cast '" + obj.GetType()?.ToString() + "' to '" + typeof(T)?.ToString() + "'");
				}
				else
				{
					list3.Add(item);
				}
			}
			list.AddRange(list3);
		}
		return list;
	}

	public static T FindObject<T>() where T : class
	{
		return FindObjects<T>()[0];
	}

	public static IList<T> GetInterfaceComponents<T>(this Component component, bool firstOnly = false) where T : class
	{
		List<Type> list = _interfaceToComponentMapping[typeof(T)];
		if (list == null || list.Count <= 0)
		{
			Debug.LogError("No descendants found for type " + typeof(T));
			return null;
		}
		List<T> list2 = new List<T>();
		foreach (Type item2 in list)
		{
			Component[] array = ((!firstOnly) ? component.GetComponents(item2) : new Component[1] { component.GetComponent(item2) });
			if (array == null || array.Length == 0)
			{
				continue;
			}
			List<T> list3 = new List<T>();
			Component[] array2 = array;
			foreach (Component component2 in array2)
			{
				if (!(component2 is T item))
				{
					Debug.LogError("Unable to cast '" + component2.GetType()?.ToString() + "' to '" + typeof(T)?.ToString() + "'");
				}
				else
				{
					list3.Add(item);
				}
			}
			list2.AddRange(list3);
		}
		return list2;
	}

	public static T GetInterfaceComponent<T>(this Component component) where T : class
	{
		return component.GetInterfaceComponents<T>(firstOnly: true)[0];
	}
}
