using UnityEngine;

public class ChargeMeter : MonoBehaviour
{
	[SerializeField]
	private bool setCanvasCircle;

	[SerializeField]
	private float chargeSpeed = 100f;

	private CanvasHelper canvas;

	private Settings settings;

	[SerializeField]
	private float maxCharge = 100f;

	private float minCharge;

	private float currentCharge;

	public float ChargeNormal => Mathf.Clamp(currentCharge / maxCharge, minCharge, maxCharge);

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		settings = ComponentManager<Settings>.Value;
	}

	public void Charge(float chargeMultiplier = 1f)
	{
		currentCharge += chargeSpeed * Time.deltaTime * chargeMultiplier;
		currentCharge = Mathf.Clamp(currentCharge, minCharge, maxCharge);
		if (setCanvasCircle && canvas != null)
		{
			canvas.SetLoadCircle(ChargeNormal);
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad && ChargeNormal != 0f)
		{
			settings.controls.LookSensitivity = ControlsSettingsBox.DefaultLookSensitivity * 0.5f;
		}
	}

	public void Reset()
	{
		currentCharge = 0f;
		settings.controls.LookSensitivity = ControlsSettingsBox.DefaultLookSensitivity;
		if (setCanvasCircle && canvas != null)
		{
			canvas.SetLoadCircle(state: false);
		}
	}
}
