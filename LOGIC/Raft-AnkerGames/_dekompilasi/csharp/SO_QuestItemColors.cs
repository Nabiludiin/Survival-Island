using UnityEngine;

[CreateAssetMenu(fileName = "QuestItemColors", menuName = "Raft/Quest/QuestItemColors")]
public class SO_QuestItemColors : ScriptableObject
{
	public DestinationColorConnection[] destinationColorConnections;

	public Color defaultColor;

	public Color GetQuestItemColorFromDestination(ChunkPointType destinationType)
	{
		if (destinationType == ChunkPointType.None)
		{
			return defaultColor;
		}
		for (int i = 0; i < destinationColorConnections.Length; i++)
		{
			if (destinationColorConnections[i].destinationType == destinationType)
			{
				return destinationColorConnections[i].color;
			}
		}
		return Color.magenta;
	}
}
