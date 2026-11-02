using UnityEngine;

public class AI_State_CirculateSpawn_Air : AI_State_CirculateSpawn
{
	[SerializeField]
	private float minAltitude = 10f;

	protected override Vector3 GetValidTargetPoint()
	{
		Vector3 vector = SpawnPointPosition + Random.insideUnitSphere * GetCirculationRadius();
		int num = 0;
		while (num < 200)
		{
			num++;
			float num2 = Mathf.Abs(stateMachine.transform.position.y - vector.y);
			float num3 = Vector3.Distance(stateMachine.transform.position, vector);
			Vector3 vector2 = vector - stateMachine.transform.position;
			if (num2 <= heightDifference && num3 >= circulationRadius / 2f && vector.y >= minAltitude && Helper.HasClearVision(stateMachine.transform.position + Vector3.up * 0.5f, vector, stateMachine.movement.obstructionLayer, vector2.magnitude * 0.9f))
			{
				break;
			}
			vector = SpawnPointPosition + Random.insideUnitSphere * GetCirculationRadius();
		}
		return vector;
	}
}
