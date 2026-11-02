using UnityEngine;

namespace RG.Parsecs.Scavenge;

[ExecuteInEditMode]
public class AlphaChange : MonoBehaviour
{
	public SpriteRenderer[] spriteRenderers;

	[Range(0f, 1f)]
	public float alpha;

	private float _prevAlpha = -1f;

	private void Update()
	{
		if (_prevAlpha != alpha)
		{
			_prevAlpha = alpha;
			for (int i = 0; i < spriteRenderers.Length; i++)
			{
				Color color = spriteRenderers[i].color;
				color.a = alpha;
				spriteRenderers[i].color = color;
			}
		}
	}
}
