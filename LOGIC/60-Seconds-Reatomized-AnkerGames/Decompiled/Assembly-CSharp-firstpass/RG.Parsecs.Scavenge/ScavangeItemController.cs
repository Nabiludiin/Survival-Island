using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ScavangeItemController : MonoBehaviour
{
	public int Weight;

	[SerializeField]
	private SpriteRenderer _outline;

	private Color _original;

	private void Awake()
	{
		_original = _outline.material.GetColor("_Color3");
	}

	public void Activate(bool activate)
	{
		_outline.material.SetColor("_Color3", (!activate) ? _original : Color.red);
	}
}
