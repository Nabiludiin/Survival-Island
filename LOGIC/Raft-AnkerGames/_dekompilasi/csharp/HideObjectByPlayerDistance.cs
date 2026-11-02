using UnityEngine;

public class HideObjectByPlayerDistance : MonoBehaviour
{
	[SerializeField]
	private TimerEventer checkPlayerDistanceTimer;

	[SerializeField]
	private GameObject objectToHide;

	[SerializeField]
	private float hideDistance = 10f;

	private void Update()
	{
		if (checkPlayerDistanceTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			objectToHide.SetActiveSafe(Helper.LocalPlayerIsWithinDistance(transform.position, hideDistance));
		}
	}
}
