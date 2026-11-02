using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_HapticStringEvents", menuName = "SO_HapticStringEvents")]
public class SO_HapticStringEvents : ScriptableObject
{
	[Serializable]
	public struct Entry
	{
		public int hapticEvent;

		public HapticAudio hapticAudio;
	}

	private Dictionary<int, HapticAudio> dictionary;

	[SerializeField]
	private Entry[] hapticEvents;

	public void Initialize(GameObject gameObject)
	{
		Entry[] array = hapticEvents;
		for (int i = 0; i < array.Length; i++)
		{
			Entry entry = array[i];
			try
			{
				entry.hapticAudio.CreateAudioSources();
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}
		if (dictionary == null)
		{
			dictionary = hapticEvents.ToDictionary((Entry x) => x.hapticEvent, (Entry x) => x.hapticAudio);
		}
	}

	public void StopAll()
	{
		Entry[] array = hapticEvents;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].hapticAudio.Stop();
		}
	}

	public bool GetHapticAudio(HapticEvent name, out HapticAudio hapticAudio)
	{
		if (dictionary == null)
		{
			hapticAudio = null;
			return false;
		}
		return dictionary.TryGetValue((int)name, out hapticAudio);
	}
}
