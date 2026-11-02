using System;
using UnityEngine;
using UnityEngine.AzureSky;

public class ParticleSystemLOD : MonoBehaviour
{
	[Serializable]
	public struct LODState
	{
		public float distance;

		public GameObject[] enabledNear;

		public GameObject[] enabledFar;
	}

	public LODState[] nightLODStates;

	public LODState[] dayLODStates;

	public GameObject Billboard;

	private float lastDistance;

	private float nightTimeStart = 19.5f;

	private float nightTimeEnd = 4.5f;

	private AzureSkyController skyController;

	private Transform mainCameraTransform;

	private void Awake()
	{
	}

	private void OnDisable()
	{
		for (int i = 0; i < nightLODStates.Length; i++)
		{
			for (int j = 0; j < nightLODStates[i].enabledNear.Length; j++)
			{
				nightLODStates[i].enabledNear[j].SetActive(value: true);
			}
			for (int k = 0; k < nightLODStates[i].enabledFar.Length; k++)
			{
				nightLODStates[i].enabledFar[k].SetActive(value: true);
			}
		}
		for (int l = 0; l < dayLODStates.Length; l++)
		{
			for (int m = 0; m < dayLODStates[l].enabledNear.Length; m++)
			{
				dayLODStates[l].enabledNear[m].SetActive(value: true);
			}
			for (int n = 0; n < dayLODStates[l].enabledFar.Length; n++)
			{
				dayLODStates[l].enabledFar[n].SetActive(value: true);
			}
		}
		if (Billboard != null)
		{
			Billboard.SetActive(value: false);
		}
	}

	private void Start()
	{
		skyController = ComponentManager<AzureSkyController>.Value;
		mainCameraTransform = Camera.main.transform;
		BedManager value = ComponentManager<BedManager>.Value;
		if (value != null)
		{
			nightTimeStart = value.canSleepAfterTime - 1f;
			nightTimeEnd = value.wakeUpTime + 1f;
		}
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			mainCameraTransform = null;
			skyController = null;
		}
	}

	private void Update()
	{
		if (!skyController)
		{
			skyController = ComponentManager<AzureSkyController>.Value;
		}
		if (mainCameraTransform == null)
		{
			Network_Player value = ComponentManager<Network_Player>.Value;
			if (value != null)
			{
				mainCameraTransform = value.Camera.transform;
			}
			return;
		}
		float magnitude = (transform.position - mainCameraTransform.position).magnitude;
		if (!(Mathf.Abs(magnitude - lastDistance) > 0.5f))
		{
			return;
		}
		lastDistance = magnitude;
		bool num = skyController.timeOfDay.hour > nightTimeStart || skyController.timeOfDay.hour < nightTimeEnd;
		LODState[] array = null;
		array = ((!num) ? dayLODStates : nightLODStates);
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = magnitude < array[i].distance;
			bool flag2 = magnitude >= array[i].distance;
			for (int j = 0; j < array[i].enabledNear.Length; j++)
			{
				if (array[i].enabledNear[j].activeSelf != flag)
				{
					array[i].enabledNear[j].SetActive(flag);
				}
			}
			for (int k = 0; k < array[i].enabledFar.Length; k++)
			{
				if (array[i].enabledFar[k].activeSelf != flag2)
				{
					array[i].enabledFar[k].SetActive(flag2);
				}
			}
		}
	}
}
