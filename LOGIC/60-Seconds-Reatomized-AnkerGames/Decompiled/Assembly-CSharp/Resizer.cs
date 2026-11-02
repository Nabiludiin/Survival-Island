using UnityEngine;

public class Resizer : MonoBehaviour
{
	[SerializeField]
	private bool _disableWidth;

	[SerializeField]
	private bool _disableHeight;

	[SerializeField]
	private bool _disableDepth;

	private void Start()
	{
		ResolutionHandler resolutionHandler = Object.FindObjectOfType<ResolutionHandler>();
		if (resolutionHandler != null)
		{
			if (!_disableWidth && !_disableHeight && !_disableDepth)
			{
				transform.localScale *= resolutionHandler.ResizeRatio;
			}
			else
			{
				float x = (_disableWidth ? transform.localScale.x : (transform.localScale.x * resolutionHandler.ResizeRatio));
				float y = (_disableHeight ? transform.localScale.y : (transform.localScale.y * resolutionHandler.ResizeRatio));
				float z = (_disableDepth ? transform.localScale.z : (transform.localScale.z * resolutionHandler.ResizeRatio));
				transform.localScale = new Vector3(x, y, z);
			}
		}
		Object.Destroy(this);
	}

	private void Update()
	{
	}
}
