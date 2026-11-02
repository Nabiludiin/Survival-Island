using UnityEngine;

public class AI_State_CirculateSpawn_Water : AI_State_CirculateSpawn
{
	[SerializeField]
	protected float minYPoint = -1f;

	[SerializeField]
	protected float maxYPoint = -1f;

	protected override bool PlayerIsWithinRange()
	{
		if (users != null)
		{
			foreach (Network_Player value in users.Values)
			{
				if (Helper.IsValidWaterTarget(value) && Vector3.Distance(stateMachine.transform.position, value.transform.position) <= playerDistance)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected override Vector3 GetValidTargetPoint()
	{
		Vector3 vector = SpawnPointPosition + Random.insideUnitSphere * GetCirculationRadius();
		vector.y = Mathf.Clamp(vector.y, minYPoint, maxYPoint);
		int num = 0;
		while (num < 100)
		{
			num++;
			Vector3 vector2 = vector - stateMachine.transform.position;
			if (Helper.HasClearVision(stateMachine.transform.position, vector, stateMachine.movement.obstructionLayer, vector2.magnitude * 0.9f))
			{
				break;
			}
			vector = SpawnPointPosition + Random.insideUnitSphere * GetCirculationRadius();
			vector.y = Mathf.Clamp(vector.y, minYPoint, maxYPoint);
		}
		return vector;
	}
}
