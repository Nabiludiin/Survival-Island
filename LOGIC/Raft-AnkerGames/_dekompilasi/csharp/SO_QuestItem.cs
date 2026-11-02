using UnityEngine;

[CreateAssetMenu(fileName = "QuestItem", menuName = "Raft/Quest/QuestItem")]
public class SO_QuestItem : ScriptableObject
{
	public ChunkPointType destinationType;

	public QuestItemType questItemType;

	public Sprite itemImage;

	[SerializeField]
	private string displayTerm;

	public string DisplayName
	{
		get
		{
			if (string.IsNullOrEmpty(displayTerm))
			{
				displayTerm = "QuestItem/" + questItemType;
			}
			return Helper.GetTerm(displayTerm);
		}
	}

	public string DisplayTerm => displayTerm;
}
