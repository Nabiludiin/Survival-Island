using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "Raft/Cutscene", order = 1)]
public class SO_Cutscene : ScriptableObject
{
	public CutsceneType type;

	public SO_VoiceData voiceData;

	public float startVoiceDelay;

	public float lockPlayerDuration;

	public float cutsceneDuration;
}
