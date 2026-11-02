using UnityEngine;

[CreateAssetMenu(fileName = "ScreenEffect", menuName = "Raft/Camera/ScreenEffects/ScreenEffect", order = 1)]
public class SO_ScreenEffect : ScriptableObject
{
	public float timeOnScreen = 2f;

	public float fadeInSpeed = 6f;

	public float fadeOutSpeed = 0.3f;
}
