using UnityEngine;

public struct SObjectDataLog(float time, Vector3 pos, Quaternion rot, string id)
{
	public string Id = id;

	public SBaseDataLog BaseData = new SBaseDataLog(time, pos, rot);

	public string GetString(char sep)
	{
		return BaseData.GetString(sep) + sep + Id.ToString();
	}
}
