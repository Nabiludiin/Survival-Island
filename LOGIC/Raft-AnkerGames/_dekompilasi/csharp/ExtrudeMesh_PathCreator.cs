using PathCreation;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
public class ExtrudeMesh_PathCreator : ExtrudeMesh
{
	private PathCreator pathCreator;

	private void OnValidate()
	{
		if (pathCreator == null)
		{
			pathCreator = GetComponent<PathCreator>();
		}
	}

	public override OrientedPoint GetOrientedPoint(float t)
	{
		return new OrientedPoint
		{
			pos = pathCreator.path.GetPointAtTime(t, EndOfPathInstruction.Stop),
			rot = pathCreator.path.GetRotation(t, EndOfPathInstruction.Stop)
		};
	}

	protected override void SetPathLength()
	{
		pathLength = pathCreator.path.length;
	}
}
