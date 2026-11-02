using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Scavenge;

public class SetCameraFov : MonoBehaviour
{
	[SerializeField]
	private CameraSettingsControl _cameraSettingsControl;

	[SerializeField]
	private InputField _input;

	private float _x;

	private void Awake()
	{
		_x = _cameraSettingsControl.Camera.fieldOfView;
		_input.text = _x.ToString();
	}

	private void Update()
	{
		if (float.TryParse(_input.text, out _x))
		{
			_cameraSettingsControl.SetCameraFov(_x);
		}
	}
}
