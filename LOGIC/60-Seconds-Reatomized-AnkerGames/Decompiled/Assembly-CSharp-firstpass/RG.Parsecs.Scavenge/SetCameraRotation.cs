using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class SetCameraRotation : MonoBehaviour
{
	[SerializeField]
	private CameraSettingsControl _cameraSettingsControl;

	[SerializeField]
	private InputField _x;

	[SerializeField]
	private InputField _y;

	[SerializeField]
	private InputField _z;

	private Vector3 _currentVector;

	private void Awake()
	{
		_currentVector = _cameraSettingsControl.transform.rotation.eulerAngles;
		_x.text = _currentVector.x.ToString();
		_y.text = _currentVector.y.ToString();
		_z.text = _currentVector.z.ToString();
	}

	private void Update()
	{
		if (float.TryParse(_x.text, out var result) && float.TryParse(_y.text, out var result2) && float.TryParse(_z.text, out var result3))
		{
			_currentVector = new Vector3(result, result2, result3);
			_cameraSettingsControl.SetCameraRotation(_currentVector);
		}
	}
}
