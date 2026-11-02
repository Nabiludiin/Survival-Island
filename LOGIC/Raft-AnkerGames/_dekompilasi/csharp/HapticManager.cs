using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : SimpleMonoBehaviourSingleton<HapticManager>
{
	private class AttenuatedHaptic
	{
		public HapticEvent hapticEvent;

		public HapticAudio haptic;

		public Vector3 position;
	}

	private class Haptic
	{
		public HapticEvent hapticEvent;

		public HapticAudio haptic;
	}

	private class ProceduralHaptic
	{
		public HapticEvent hapticEvent;

		public float globalStrength;

		public int lifetime;
	}

	[SerializeField]
	private SO_HapticStringEvents hapticStringEvents;

	private List<Haptic> haptics = new List<Haptic>();

	private List<AttenuatedHaptic> attenuatedHaptics = new List<AttenuatedHaptic>();

	private List<ProceduralHaptic> proceduralHaptics = new List<ProceduralHaptic>();

	public HapticPool SharkPool = new HapticPool(5);

	public HapticPool DynamitePool = new HapticPool(3);

	private void Start()
	{
		hapticStringEvents.Initialize(gameObject);
	}

	public void Update()
	{
		RuntimeManager.CoreSystem.get3DNumListeners(out var numlisteners);
		if (numlisteners <= 0)
		{
			return;
		}
		RuntimeManager.CoreSystem.get3DListenerAttributes(0, out var pos, out var _, out var _, out var _);
		Vector3 vector = new Vector3(pos.x, pos.y, pos.z);
		List<HapticEvent> list = new List<HapticEvent>();
		List<HapticEvent> list2 = new List<HapticEvent>();
		List<HapticEvent> list3 = new List<HapticEvent>();
		float num = 0f;
		float intensity = 0f;
		foreach (ProceduralHaptic proceduralHaptic in proceduralHaptics)
		{
			if (proceduralHaptic.lifetime > 0)
			{
				num += proceduralHaptic.globalStrength;
				proceduralHaptic.lifetime--;
			}
			else
			{
				list3.Add(proceduralHaptic.hapticEvent);
			}
		}
		foreach (Haptic haptic in haptics)
		{
			if (haptic.haptic.Update(out intensity))
			{
				num += intensity;
				continue;
			}
			haptic.haptic.Stop();
			list.Add(haptic.hapticEvent);
		}
		float num2 = 0f;
		float num3 = 0f;
		foreach (AttenuatedHaptic attenuatedHaptic in attenuatedHaptics)
		{
			Vector3 vector2 = attenuatedHaptic.position - vector;
			float num4 = Helper.MapValue(0f, attenuatedHaptic.haptic.rangeSqr, vector2.sqrMagnitude, 1f, 0f);
			if (attenuatedHaptic.haptic.Update(out intensity))
			{
				num2 += intensity * num4;
				num3++;
				continue;
			}
			attenuatedHaptic.haptic.Stop();
			if (attenuatedHaptic.hapticEvent == HapticEvent.Dynamite_0 || attenuatedHaptic.hapticEvent == HapticEvent.Dynamite_1 || attenuatedHaptic.hapticEvent == HapticEvent.Dynamite_2)
			{
				DynamitePool.ReleaseIndex((int)(attenuatedHaptic.hapticEvent - 29));
			}
			list2.Add(attenuatedHaptic.hapticEvent);
		}
		foreach (HapticEvent ev in list)
		{
			haptics.RemoveAll((Haptic item) => item.hapticEvent == ev);
		}
		foreach (HapticEvent ev2 in list2)
		{
			attenuatedHaptics.RemoveAll((AttenuatedHaptic item) => item.hapticEvent == ev2);
		}
		foreach (HapticEvent ev3 in list3)
		{
			proceduralHaptics.RemoveAll((ProceduralHaptic item) => item.hapticEvent == ev3);
		}
		float num5 = num;
		if (num3 > 0f)
		{
			num5 += num2 / num3;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Gamepad.current?.SetMotorSpeeds(num5 * 0.5f * ComponentManager<Settings>.Value.controls.VibrationStrength, num5 * 0.5f * ComponentManager<Settings>.Value.controls.VibrationStrength);
		}
	}

	public void ResetHaptics()
	{
		SharkPool.Reset();
		DynamitePool.Reset();
		hapticStringEvents.StopAll();
	}

	public void PlayOneShot(HapticEvent hapticEvent)
	{
		if (hapticStringEvents.GetHapticAudio(hapticEvent, out var hapticAudio) && !hapticAudio.IsPlaying())
		{
			hapticAudio.Start();
			haptics.Add(new Haptic
			{
				hapticEvent = hapticEvent,
				haptic = hapticAudio
			});
		}
	}

	public void Start(HapticEvent hapticEvent)
	{
		if (hapticStringEvents.GetHapticAudio(hapticEvent, out var hapticAudio) && !hapticAudio.IsPlaying())
		{
			hapticAudio.Start();
			haptics.Add(new Haptic
			{
				hapticEvent = hapticEvent,
				haptic = hapticAudio
			});
		}
	}

	public void StartAttenuated(HapticEvent hapticEvent, Vector3 position)
	{
		if (hapticStringEvents.GetHapticAudio(hapticEvent, out var hapticAudio) && !hapticAudio.IsPlaying())
		{
			hapticAudio.Start();
			attenuatedHaptics.Add(new AttenuatedHaptic
			{
				hapticEvent = hapticEvent,
				haptic = hapticAudio,
				position = position
			});
		}
	}

	public void Stop(HapticEvent hapticEvent)
	{
		if (hapticStringEvents.GetHapticAudio(hapticEvent, out var hapticAudio))
		{
			haptics.RemoveAll((Haptic item) => item.hapticEvent == hapticEvent);
			hapticAudio.Stop();
		}
	}

	public void StopAtennuated(HapticEvent hapticEvent)
	{
		if (hapticStringEvents.GetHapticAudio(hapticEvent, out var hapticAudio))
		{
			attenuatedHaptics.RemoveAll((AttenuatedHaptic item) => item.hapticEvent == hapticEvent);
			hapticAudio.Stop();
		}
	}

	public void ProgressProcedural(HapticEvent hapticEvent, float strength)
	{
		ProceduralHaptic proceduralHaptic = proceduralHaptics.Find((ProceduralHaptic item) => item.hapticEvent == hapticEvent);
		if (proceduralHaptic == null)
		{
			proceduralHaptics.Add(new ProceduralHaptic
			{
				hapticEvent = hapticEvent,
				globalStrength = strength,
				lifetime = 4
			});
		}
		else
		{
			proceduralHaptic.lifetime++;
		}
	}

	public void SetVolume(HapticEvent hapticEvent, float volume)
	{
		if (hapticStringEvents.GetHapticAudio(hapticEvent, out var hapticAudio))
		{
			hapticAudio.volume = volume;
		}
	}
}
