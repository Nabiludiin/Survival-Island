using UnityEngine;

public class FluidComponent : MonoBehaviour
{
	[SerializeField]
	private Transform fluidTransform;

	[SerializeField]
	private float yPosStartLocal;

	[SerializeField]
	private float stepInterval;

	private float targetY;

	protected virtual void Start()
	{
		HideFluid(instantly: true);
	}

	private void Update()
	{
		if (!fluidTransform.gameObject.activeInHierarchy)
		{
			return;
		}
		Vector3 localPosition = fluidTransform.localPosition;
		if (localPosition.y != targetY)
		{
			localPosition.y = Mathf.Lerp(localPosition.y, targetY, Time.deltaTime * 2f);
			fluidTransform.localPosition = localPosition;
			if (targetY == yPosStartLocal && Mathf.Abs(localPosition.y - targetY) < 0.01f)
			{
				fluidTransform.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetStep(int stepNumber)
	{
		if (!fluidTransform.gameObject.activeInHierarchy)
		{
			ShowFluid();
		}
		targetY = yPosStartLocal + stepInterval * (float)stepNumber;
	}

	public void HideFluid(bool instantly = false)
	{
		if (instantly)
		{
			fluidTransform.gameObject.SetActive(value: false);
		}
		else
		{
			targetY = yPosStartLocal;
		}
	}

	protected void ShowFluid()
	{
		fluidTransform.gameObject.SetActive(value: true);
	}
}
