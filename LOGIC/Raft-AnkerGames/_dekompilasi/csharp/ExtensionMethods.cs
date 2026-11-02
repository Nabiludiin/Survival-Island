using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
	public static void IncrementCycle(this ref int number, int increment, int min, int max)
	{
		number += increment;
		if (number > max)
		{
			number = min;
		}
		if (number < min)
		{
			number = max;
		}
	}

	public static Vector3 XZOnly(this Vector3 vector)
	{
		return new Vector3(vector.x, 0f, vector.z);
	}

	public static void Clamp(this Vector3 vector, float min, float max)
	{
		vector.x = Mathf.Clamp(vector.x, min, max);
		vector.y = Mathf.Clamp(vector.y, min, max);
		vector.z = Mathf.Clamp(vector.z, min, max);
	}

	public static float DistanceXZ(this Vector3 vector, Vector3 other)
	{
		vector.y = (other.y = 0f);
		return Vector3.Distance(vector, other);
	}

	public static bool IsLessThan(this Vector3 vector, Vector3 other)
	{
		if (vector.x < other.x)
		{
			return vector.y < other.y;
		}
		return false;
	}

	public static bool IsLessThanOrEqual(this Vector3 vector, Vector3 other)
	{
		if (vector.x <= other.x)
		{
			return vector.y <= other.y;
		}
		return false;
	}

	public static bool IsGreaterThan(this Vector3 vector, Vector3 other)
	{
		if (vector.x > other.x)
		{
			return vector.y > other.y;
		}
		return false;
	}

	public static bool IsGreaterThanOrEqual(this Vector3 vector, Vector3 other)
	{
		if (vector.x >= other.x)
		{
			return vector.y >= other.y;
		}
		return false;
	}

	public static Vector3 RoundToDividable(this Vector3 vector, float dividableAmount)
	{
		Vector3 result = vector;
		result.x = (float)Mathf.RoundToInt(result.x / dividableAmount) * dividableAmount;
		result.y = (float)Mathf.RoundToInt(result.y / dividableAmount) * dividableAmount;
		result.z = (float)Mathf.RoundToInt(result.z / dividableAmount) * dividableAmount;
		return result;
	}

	public static bool IsLessThan(this Vector2 vector, Vector2 other)
	{
		if (vector.x < other.x)
		{
			return vector.y < other.y;
		}
		return false;
	}

	public static bool IsLessThanOrEqual(this Vector2 vector, Vector2 other)
	{
		if (vector.x <= other.x)
		{
			return vector.y <= other.y;
		}
		return false;
	}

	public static bool IsGreaterThan(this Vector2 vector, Vector2 other)
	{
		if (vector.x > other.x)
		{
			return vector.y > other.y;
		}
		return false;
	}

	public static bool IsGreaterThanOrEqual(this Vector2 vector, Vector2 other)
	{
		if (vector.x >= other.x)
		{
			return vector.y >= other.y;
		}
		return false;
	}

	public static float BiggestOfValues(params float[] values)
	{
		float num = values[0];
		for (int i = 1; i < values.Length; i++)
		{
			if (values[i] > num)
			{
				num = values[i];
			}
		}
		return num;
	}

	public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
	{
		float t = Mathf.InverseLerp(fromMin, fromMax, value);
		return Mathf.Lerp(toMin, toMax, t);
	}

	public static bool IsNullOrEmpty(this string text)
	{
		return string.IsNullOrEmpty(text);
	}

	public static bool IsValid(this string text)
	{
		return !text.IsNullOrEmpty();
	}

	public static bool Contains(this string source, string toCheck, StringComparison comp)
	{
		if (source == null)
		{
			return false;
		}
		return source.IndexOf(toCheck, comp) >= 0;
	}

	public static bool ContainsInvalidWindowsCharacter(this string source)
	{
		if (source == null)
		{
			return false;
		}
		if (source.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
		{
			return true;
		}
		if (!source.Contains("/") && !source.Contains("\\") && !source.Contains(":") && !source.Contains("*") && !source.Contains("?") && !source.Contains("\"") && !source.Contains("<") && !source.Contains(">"))
		{
			return source.Contains("|");
		}
		return true;
	}

	public static void SubscribeMethod(this Action action, Action method)
	{
		action = (Action)Delegate.Combine(action, method);
	}

	public static void CallSafe(this Action action)
	{
		action?.Invoke();
	}

	public static void CallSafe<T1>(this Action<T1> action, T1 arg1)
	{
		action?.Invoke(arg1);
	}

	public static void CallSafe<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
	{
		action?.Invoke(arg1, arg2);
	}

	public static void CallSafe<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
	{
		action?.Invoke(arg1, arg2, arg3);
	}

	public static void CallSafe<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		action?.Invoke(arg1, arg2, arg3, arg4);
	}

	public static string ToShortString(this KeyCode keycode)
	{
		return keycode switch
		{
			KeyCode.None => "Not set", 
			KeyCode.Mouse0 => "LMB", 
			KeyCode.Mouse1 => "RMB", 
			KeyCode.Mouse2 => "MMB", 
			KeyCode.Escape => "Esc", 
			KeyCode.CapsLock => "Caps", 
			KeyCode.Comma => ",", 
			KeyCode.Period => ".", 
			KeyCode.DownArrow => "DArrow", 
			KeyCode.LeftArrow => "LArrow", 
			KeyCode.RightArrow => "RArrow", 
			KeyCode.UpArrow => "UArrow", 
			KeyCode.LeftShift => "LShift", 
			KeyCode.RightShift => "RShift", 
			KeyCode.LeftControl => "LCtrl", 
			KeyCode.RightControl => "RCtrl", 
			KeyCode.LeftAlt => "LAlt", 
			KeyCode.RightAlt => "RAlt", 
			KeyCode.Alpha1 => "1", 
			KeyCode.Alpha2 => "2", 
			KeyCode.Alpha3 => "3", 
			KeyCode.Alpha4 => "4", 
			KeyCode.Alpha5 => "5", 
			KeyCode.Alpha6 => "6", 
			KeyCode.Alpha7 => "7", 
			KeyCode.Alpha8 => "8", 
			KeyCode.Alpha9 => "9", 
			KeyCode.Alpha0 => "0", 
			KeyCode.Keypad0 => "Num0", 
			KeyCode.Keypad1 => "Num1", 
			KeyCode.Keypad2 => "Num2", 
			KeyCode.Keypad3 => "Num3", 
			KeyCode.Keypad4 => "Num4", 
			KeyCode.Keypad5 => "Num5", 
			KeyCode.Keypad6 => "Num6", 
			KeyCode.Keypad7 => "Num7", 
			KeyCode.Keypad8 => "Num8", 
			KeyCode.Keypad9 => "Num9", 
			KeyCode.Backslash => "Backslash", 
			_ => keycode.ToString(), 
		};
	}

	public static bool IsIndexWithinRange(this Array array, int index)
	{
		if (array != null && index >= 0)
		{
			return index < array.Length;
		}
		return false;
	}

	public static bool ContainsItems(this Array array)
	{
		if (array != null)
		{
			return array.Length > 0;
		}
		return false;
	}

	public static bool EqualsOtherArray<T>(this T[] array, T[] other)
	{
		for (int i = 0; i < array.Length; i++)
		{
			ref readonly T reference = ref array[i];
			object obj = other[i];
			if (!reference.Equals(obj))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsIndexWithinRange<T>(this List<T> array, int index)
	{
		if (array != null && index >= 0)
		{
			return index < array.Count;
		}
		return false;
	}

	public static bool ContainsItems<T>(this List<T> array)
	{
		if (array != null)
		{
			return array.Count > 0;
		}
		return false;
	}

	public static void AddRangeUniqueOnly<T>(this List<T> array, IEnumerable<T> range)
	{
		IEnumerator enumerator = range.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!array.Contains((T)enumerator.Current))
			{
				array.Add((T)enumerator.Current);
			}
		}
	}

	public static bool AddUniqueOnly<T>(this List<T> array, T itemToAdd)
	{
		if (itemToAdd != null && !array.Contains(itemToAdd))
		{
			array.Add(itemToAdd);
			return true;
		}
		return false;
	}

	public static bool ContainsItems<T, B>(this Dictionary<T, B> dic)
	{
		if (dic != null)
		{
			return dic.Count > 0;
		}
		return false;
	}

	public static bool IsNotNull(this Behaviour behaviour)
	{
		return behaviour != null;
	}

	public static bool SetActiveSafe(this GameObject gameObject, bool value)
	{
		if (gameObject == null)
		{
			return false;
		}
		if (gameObject.activeSelf != value)
		{
			gameObject.SetActive(value);
			return true;
		}
		return false;
	}

	public static void SetLayerSafe(this GameObject gameObject, int layer)
	{
		if (!gameObject.layer.Equals(layer))
		{
			gameObject.layer = layer;
		}
	}

	public static void SetLayerRecursivly(this GameObject parentGameObject, int layer)
	{
		for (int i = 0; i < parentGameObject.transform.childCount; i++)
		{
			Transform child = parentGameObject.transform.GetChild(i);
			child.gameObject.SetLayerSafe(layer);
			child.gameObject.SetLayerRecursivly(layer);
		}
	}

	public static bool SetParentSafe(this Transform transform, Transform newParent)
	{
		if (transform.parent != newParent)
		{
			transform.SetParent(newParent);
			return true;
		}
		return false;
	}

	public static bool ParentedToRaft(this Transform transform)
	{
		GameManager singleton = SingletonGeneric<GameManager>.Singleton;
		if (singleton != null)
		{
			return transform.parent == singleton.lockedPivot;
		}
		return false;
	}

	public static Transform FindChildRecursively(this Transform tranform, string childName)
	{
		Transform transform = tranform.Find(childName);
		if (transform != null)
		{
			return transform;
		}
		foreach (Transform item in tranform)
		{
			transform = item.FindChildRecursively(childName);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public static void GetComponentsInChildrenRecursively<T>(this Transform tranform, ref List<T> result)
	{
		if (result == null)
		{
			result = new List<T>();
		}
		foreach (Transform item in tranform)
		{
			T component = item.GetComponent<T>();
			if (component != null)
			{
				result.AddUniqueOnly(component);
			}
			item.GetComponentsInChildrenRecursively(ref result);
		}
	}

	public static void FindChildrenByTag(this Transform parent, ref List<Transform> children, string tag)
	{
		foreach (Transform item in parent.transform)
		{
			if (item.tag == tag)
			{
				if (children == null)
				{
					children = new List<Transform>();
				}
				children.Add(item);
			}
			item.FindChildrenByTag(ref children, tag);
		}
	}

	public static void FindChildrenByLayer(this Transform parent, ref List<Transform> children, int layer)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (child.gameObject.layer.Equals(layer))
			{
				children.Add(child);
			}
			child.FindChildrenByLayer(ref children, layer);
		}
	}

	public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 position)
	{
		return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).MultiplyPoint3x4(position);
	}

	public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 position)
	{
		return Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse.MultiplyPoint3x4(position);
	}

	public static void SaveAnimatorParameters(this Animator anim, ref AnimatorStorage storage)
	{
		if (anim.parameterCount == 0)
		{
			return;
		}
		if (storage == null)
		{
			storage = new AnimatorStorage((uint)anim.parameterCount);
		}
		for (int i = 0; i < anim.parameterCount; i++)
		{
			AnimatorControllerParameter param = anim.parameters[i];
			if (storage.storedParamaters != null)
			{
				storage.storedParamaters[i].SaveParameter(param, anim);
			}
		}
	}

	public static void LoadAnimatorParameters(this Animator anim, AnimatorStorage storage)
	{
		if (storage != null && storage.storedParamaters != null)
		{
			for (int i = 0; i < storage.storedParamaters.Length; i++)
			{
				storage.storedParamaters[i].LoadParameter(anim);
			}
		}
	}

	public static void SetEnabledSafe(this Behaviour behaviour, bool value)
	{
		if (behaviour.enabled != value)
		{
			behaviour.enabled = value;
		}
	}

	public static void PlaySafe(this ParticleSystem particleSystem)
	{
		if (!(particleSystem == null) && !particleSystem.isPlaying)
		{
			particleSystem.Play();
		}
	}

	public static void StopSafe(this ParticleSystem particleSystem)
	{
		if (!(particleSystem == null) && particleSystem.isPlaying)
		{
			particleSystem.Stop();
		}
	}

	public static void PauseSafe(this ParticleSystem particleSystem)
	{
		if (!(particleSystem == null) && !particleSystem.isPaused)
		{
			particleSystem.Pause();
		}
	}

	public static void PlaySafe(this StudioEventEmitter emitter)
	{
		if (emitter == null)
		{
			Debug.LogError("Emitter is null (ExtensionMethods.cs->PlaySafe())", emitter);
		}
		else if (!emitter.IsPlaying())
		{
			emitter.Play();
		}
	}

	public static void StopSafe(this StudioEventEmitter emitter)
	{
		if (emitter.IsPlaying())
		{
			emitter.Stop();
		}
	}

	public static IEnumerator StopSafeDelayed(this StudioEventEmitter emitter, float delay)
	{
		if (emitter.IsPlaying())
		{
			yield return new WaitForSeconds(delay);
			emitter.Stop();
		}
	}

	public static void StopSafe(this NavMeshAgent agent)
	{
		if (agent.isOnNavMesh && agent.enabled && !agent.isStopped)
		{
			agent.isStopped = true;
		}
	}

	public static void ResumeSafe(this NavMeshAgent agent)
	{
		if (agent.isOnNavMesh && agent.enabled && agent.isStopped)
		{
			agent.isStopped = false;
		}
	}

	public static void TurnOffSafe(this NavMeshAgent agent)
	{
		if (agent.enabled)
		{
			if (agent.isOnNavMesh && !agent.isStopped)
			{
				agent.isStopped = true;
			}
			agent.enabled = false;
		}
	}

	public static void TurnOnSafe(this NavMeshAgent agent)
	{
		if (!agent.enabled)
		{
			agent.enabled = true;
			if (agent.isOnNavMesh && agent.isStopped)
			{
				agent.isStopped = false;
			}
		}
	}

	public static Color RandomColor(this Color color)
	{
		return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1f);
	}
}
