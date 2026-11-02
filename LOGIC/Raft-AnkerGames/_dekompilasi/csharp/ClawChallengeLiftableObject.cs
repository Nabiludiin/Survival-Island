using UnityEngine;

public class ClawChallengeLiftableObject : MonoBehaviour
{
	public int uniqueIndex;

	[HideInInspector]
	public float startLocalYPosition;

	private void Awake()
	{
		startLocalYPosition = transform.localPosition.y;
	}
}
