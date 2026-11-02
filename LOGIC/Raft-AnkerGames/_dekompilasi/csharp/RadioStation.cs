using FMODUnity;
using UnityEngine;

public class RadioStation : MonoBehaviour
{
	public StudioEventEmitter eventRef_radioTransmitter;

	[SerializeField]
	public float radioClipLength = 683f;

	[SerializeField]
	private float maxDifference = 0.1f;

	private float currentCalculatedRadioTime;

	private Network_Water water;

	public int GetRadioStationTime => (int)(CalculateRadioTimeWithWater(water.WaterTime) * 1000f);

	private void Awake()
	{
		ComponentManager<RadioStation>.Value = this;
	}

	private void Start()
	{
		if (water == null)
		{
			water = ComponentManager<Network_Water>.Value;
		}
		eventRef_radioTransmitter.Play();
	}

	private void FixedUpdate()
	{
		float num = CalculateRadioTimeWithWater(water.WaterTime);
		if (num > currentCalculatedRadioTime && Mathf.Abs(num - currentCalculatedRadioTime) > maxDifference)
		{
			SyncRadioTimeToWaterTime();
		}
		currentCalculatedRadioTime = num;
	}

	private void OnDestroy()
	{
		eventRef_radioTransmitter.Stop();
	}

	private float GetRadioTimeProgress()
	{
		int position = 0;
		eventRef_radioTransmitter.EventInstance.getTimelinePosition(out position);
		return position / 1000;
	}

	private float CalculateRadioTimeWithWater(float waterTime)
	{
		return waterTime % radioClipLength;
	}

	private void SyncRadioTimeToWaterTime()
	{
		int timelinePosition = (int)(CalculateRadioTimeWithWater(water.WaterTime) * 1000f);
		eventRef_radioTransmitter.EventInstance.setTimelinePosition(timelinePosition);
	}
}
