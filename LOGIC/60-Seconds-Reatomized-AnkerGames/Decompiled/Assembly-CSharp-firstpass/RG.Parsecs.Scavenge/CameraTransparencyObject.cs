using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CameraTransparencyObject : MonoBehaviour, ICameraTransparent
{
	public bool IsTransparent;

	public float TransparentValue = 0.5f;

	private Coroutine _coroutine;

	public void SetTransparent()
	{
		IsTransparent = true;
		Renderer[] componentsInChildren = transform.GetComponentsInChildren<Renderer>();
		if (componentsInChildren.Length != 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Color color = componentsInChildren[i].material.GetColor("_Color");
				componentsInChildren[i].material.SetColor("_Color", new Color(color.r, color.g, color.b, TransparentValue));
			}
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			_coroutine = StartCoroutine(SetTransparencyOff());
		}
	}

	private IEnumerator SetTransparencyOff()
	{
		yield return new WaitForSeconds(0.2f);
		IsTransparent = false;
		Renderer[] renderer = transform.GetComponentsInChildren<Renderer>();
		if (renderer.Length == 0)
		{
			yield return null;
		}
		for (int i = 0; i < renderer.Length; i++)
		{
			Color color = renderer[i].material.GetColor("_Color");
			renderer[i].material.SetColor("_Color", new Color(color.r, color.g, color.b, 1f));
		}
	}
}
