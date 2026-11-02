using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CollectorController : MonoBehaviour
{
	[SerializeField]
	private CharacterController _characterController;

	[SerializeField]
	private Transform _leftCollector;

	[SerializeField]
	private Transform _rightCollector;

	[SerializeField]
	private Transform _topCollector;

	[SerializeField]
	private Transform _bottomCollector;

	[SerializeField]
	private Transform _triggerObject;

	private void Update()
	{
		float axis = Input.GetAxis("Vertical");
		float axis2 = Input.GetAxis("Horizontal");
		if (axis > 0f)
		{
			_triggerObject.SetPositionAndRotation(_topCollector.position, _topCollector.rotation);
		}
		else if (axis < 0f)
		{
			_triggerObject.SetPositionAndRotation(_bottomCollector.position, _bottomCollector.rotation);
		}
		else if (axis2 < 0f)
		{
			_triggerObject.SetPositionAndRotation(_leftCollector.position, _leftCollector.rotation);
		}
		else if (axis2 > 0f)
		{
			_triggerObject.SetPositionAndRotation(_rightCollector.position, _rightCollector.rotation);
		}
	}

	public ScavangeItemController Collect()
	{
		return null;
	}
}
