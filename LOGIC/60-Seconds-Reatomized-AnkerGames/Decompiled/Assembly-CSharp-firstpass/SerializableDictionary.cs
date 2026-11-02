using System.Collections.Generic;
using UnityEngine;

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private TKey[] m_keys;

	[SerializeField]
	private TValue[] m_values;

	public SerializableDictionary()
	{
	}

	public SerializableDictionary(IDictionary<TKey, TValue> dict)
		: base(dict.Count)
	{
		foreach (KeyValuePair<TKey, TValue> item in dict)
		{
			base[item.Key] = item.Value;
		}
	}

	public void CopyFrom(IDictionary<TKey, TValue> dict)
	{
		Clear();
		foreach (KeyValuePair<TKey, TValue> item in dict)
		{
			base[item.Key] = item.Value;
		}
	}

	public void OnAfterDeserialize()
	{
		if (m_keys != null && m_values != null && m_keys.Length == m_values.Length)
		{
			Clear();
			int num = m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				base[m_keys[i]] = m_values[i];
			}
			m_keys = null;
			m_values = null;
		}
	}

	public void OnBeforeSerialize()
	{
		int num = Count;
		m_keys = new TKey[num];
		m_values = new TValue[num];
		int num2 = 0;
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<TKey, TValue> current = enumerator.Current;
			m_keys[num2] = current.Key;
			m_values[num2] = current.Value;
			num2++;
		}
	}
}
