using Knife.Tools;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public class DecalReceiverRaycaster : MonoBehaviour
{
	public Camera RaycastCamera;

	public bool UseGPURaycast;

	private GPURaycastDecalsTargetInfo hitInfo;

	private void Update()
	{
		bool key = Input.GetKey(KeyCode.LeftShift);
		if ((!Input.GetMouseButtonDown(0) || key) && !(Input.GetMouseButton(0) & key))
		{
			return;
		}
		RaycastHit raycastHit;
		if (UseGPURaycast)
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.mousePosition.x / (float)RaycastCamera.pixelWidth;
			zero.y = Input.mousePosition.y / (float)RaycastCamera.pixelHeight;
			if (GPURaycast.RaycastToRegisteredTargets(RaycastCamera, zero, out hitInfo))
			{
				DecalReceiverHelper.SendGPURaycastInfo(hitInfo);
			}
		}
		else if (Physics.Raycast(RaycastCamera.ScreenPointToRay(Input.mousePosition), out raycastHit))
		{
			DecalReceiverHelper.SendPhysicsRaycastInfo(raycastHit);
		}
	}
}
