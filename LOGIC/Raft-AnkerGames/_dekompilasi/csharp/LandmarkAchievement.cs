using UnityEngine;

public class LandmarkAchievement : MonoBehaviour
{
	public float checkDistance;

	public AchievementType achievementType;

	[SerializeField]
	private bool drawGizmosWhenNotSelected;

	[SerializeField]
	private Transform checkFrom;

	private Network_Player localPlayer;

	private void Start()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (checkFrom == null)
		{
			checkFrom = transform;
		}
	}

	private void Update()
	{
		UpdateAchivementCheck();
	}

	private void OnDrawGizmos()
	{
		DrawSphereGizmo();
	}

	private void DrawSphereGizmo()
	{
		Gizmos.color = Color.black;
		if (checkFrom != null)
		{
			Gizmos.DrawWireSphere(checkFrom.position, checkDistance);
		}
	}

	private void UpdateAchivementCheck()
	{
		if (CheckIfInDistance())
		{
			UnlockLandmarkAchievement();
		}
	}

	private bool CheckIfInDistance()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer == null)
		{
			return false;
		}
		return Vector3.Distance(checkFrom.position, localPlayer.transform.position) < checkDistance;
	}

	private void UnlockLandmarkAchievement()
	{
		AchievementHandler.UnlockAchievement(achievementType);
	}
}
