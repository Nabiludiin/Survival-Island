using System.Collections.Generic;
using UnityEngine;

public class AI_State_AirLand_AwayFromPlayer : AI_State_AirLand
{
	[SerializeField]
	private float minDistanceFromPlayer = 3f;

	private Dictionary<Network_UserId, Network_Player> users;

	protected override void Start()
	{
		base.Start();
		if (ComponentManager<Raft_Network>.Value != null)
		{
			users = ComponentManager<Raft_Network>.Value.remoteUsers;
		}
	}

	public override bool SetRandomLandPosition()
	{
		base.SetRandomLandPosition();
		int num = 0;
		while (num < 200)
		{
			num++;
			bool flag = false;
			foreach (Network_Player value in ComponentManager<Raft_Network>.Value.remoteUsers.Values)
			{
				if (!(value == null) && Vector3.Distance(landPosition, value.transform.position) <= minDistanceFromPlayer)
				{
					flag = true;
					Debug.DrawLine(landPosition, landPosition + Vector3.up * 3f, Color.red, 2f);
					base.SetRandomLandPosition();
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
		}
		return false;
	}
}
