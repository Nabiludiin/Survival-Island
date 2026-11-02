using UnityEngine;
using UnityEngine.UI;

public class SetAlphaHitMinimumThreshold : MonoBehaviour
{
	[SerializeField]
	private Image _image;

	private void Start()
	{
		_image.alphaHitTestMinimumThreshold = 1f;
	}
}
