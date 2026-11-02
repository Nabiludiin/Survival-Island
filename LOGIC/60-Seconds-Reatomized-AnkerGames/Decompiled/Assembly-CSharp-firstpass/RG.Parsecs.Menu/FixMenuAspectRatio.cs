using UnityEngine;

namespace RG.Parsecs.Menu;

[ExecuteInEditMode]
public class FixMenuAspectRatio : MonoBehaviour
{
	private Vector2 size = Vector2.zero;

	private void Start()
	{
	}

	private void Update()
	{
		if (size.x != (float)Screen.width || size.y != (float)Screen.height)
		{
			size = new Vector2(Screen.width, Screen.height);
			Camera.main.fieldOfView = size.y / size.x * 46.933f;
		}
	}
}
