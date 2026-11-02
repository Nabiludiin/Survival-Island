using UnityEngine;

public class ObjectOffsetter : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private ObjectOffsetConnection[] offsetConnections;

	public void SetOffset(string connectionName)
	{
		if (target == null)
		{
			return;
		}
		ObjectOffsetConnection[] array = offsetConnections;
		foreach (ObjectOffsetConnection objectOffsetConnection in array)
		{
			if (objectOffsetConnection.connectionName.Equals(connectionName))
			{
				objectOffsetConnection.SetTransformToSettings(target);
			}
		}
	}
}
