using UnityEngine;

public interface IInterestPoint
{
	bool Occupy(bool value);

	bool Occupied();

	bool IsReachable();

	Vector3 GetPoint();
}
