using TMPro;
using UnityEngine;

public class FrequencyTextMeshProUI : MonoBehaviour
{
	[SerializeField]
	public ChunkPointType frequencyType = ChunkPointType.Landmark_RadioTower;

	[SerializeField]
	private TextMeshProUGUI frequencyText;

	private void Start()
	{
		RecieverFrequency[] allFrequencies = RecieverFrequency.AllFrequencies;
		foreach (RecieverFrequency recieverFrequency in allFrequencies)
		{
			if (recieverFrequency.chunkPointType == frequencyType)
			{
				frequencyText.text = recieverFrequency.ToString();
			}
		}
	}
}
