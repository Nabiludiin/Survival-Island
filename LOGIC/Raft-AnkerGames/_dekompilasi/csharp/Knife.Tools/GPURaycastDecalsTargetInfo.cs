using UnityEngine;

namespace Knife.Tools;

public struct GPURaycastDecalsTargetInfo
{
	public Vector3 position;

	public Vector3 normal;

	public Renderer hittedRenderer;

	public int VertexIndex;

	public DecalsTarget hittedTarget;

	public bool IsHitted;
}
