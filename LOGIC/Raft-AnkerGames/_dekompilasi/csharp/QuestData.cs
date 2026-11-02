using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Raft / Quest / QuestItem")]
public class QuestData : ScriptableObject
{
	public QuestType quest;

	public QuestType[] questRequirements;

	public string descriptionText;

	public int progressCountRequired = 1;
}
