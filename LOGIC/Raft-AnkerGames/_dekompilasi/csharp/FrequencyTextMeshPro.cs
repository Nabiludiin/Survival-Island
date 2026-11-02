using TMPro;
using UnityEngine;

public class FrequencyTextMeshPro : MonoBehaviour
{
	[SerializeField]
	private ChunkPointType frequencyType = ChunkPointType.Landmark_RadioTower;

	[SerializeField]
	private TextMeshPro frequencyText;

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
