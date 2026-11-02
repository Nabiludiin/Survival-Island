using UnityEngine;

namespace RG.Parsecs.Survival;

[ExecuteInEditMode]
public class CameraAspect : MonoBehaviour
{
	[SerializeField]
	private BoundsGroup[] _boundsGroups;

	[SerializeField]
	private Sprite _backgroundSprite;

	[SerializeField]
	private Camera _camera;

	private void Awake()
	{
		if (_camera != null)
		{
			_camera.orthographicSize = GetBoundsX() * (float)Screen.height / (float)Screen.width * 0.5f;
		}
		else
		{
			Camera.main.orthographicSize = GetBoundsX() * (float)Screen.height / (float)Screen.width * 0.5f;
		}
	}

	private float GetBoundsX()
	{
		if (_boundsGroups != null)
		{
			for (int i = 0; i < _boundsGroups.Length; i++)
			{
				if (_boundsGroups[i].Platform == Application.platform && _boundsGroups[i] != null)
				{
					return _boundsGroups[i].RefSprite.bounds.size.x;
				}
			}
		}
		return _backgroundSprite.bounds.size.x;
	}
}
