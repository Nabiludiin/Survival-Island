using UnityEngine;

public struct SBaseDataLog(float time, Vector3 pos, Quaternion rot)
{
	public float Time = time;

	public Vector3 Position = pos;

	public Quaternion Rotation = rot;

	public string GetString(char sep)
	{
		return Time.ToString() + sep + Position.ToString() + sep + Rotation.ToString();
	}
}
