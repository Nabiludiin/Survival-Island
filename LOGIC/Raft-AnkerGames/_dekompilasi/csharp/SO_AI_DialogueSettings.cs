using UnityEngine;

[CreateAssetMenu(fileName = "AI_DialogueSetting", menuName = "Raft/Dialogue/AI/AI_DialogueSetting")]
public class SO_AI_DialogueSettings : ScriptableObject
{
	[Range(0f, 100f)]
	public int enterAreaSpeechPrevelancyPercentage = 25;
}
