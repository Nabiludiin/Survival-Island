using UnityEngine;

namespace Knife.Tools;

public struct GPURaycastInfo
{
	public Vector3 position;

	public Vector3 normal;

	public Renderer hittedRenderer;

	public bool IsHitted;
}
