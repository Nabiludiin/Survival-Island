using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CameraTransparencyWall : MonoBehaviour, ICameraTransparent
{
	public bool IsTransparent;

	public MeshRenderer WallRenderer;

	public Wall Wall;

	public float TransparentValue = 0.5f;

	private Coroutine _coroutine;

	private void Awake()
	{
		Wall.SetupMaterials();
		SetAlpha(1f);
		SetAlpha_TransparentMaterials(1f);
	}

	private void OnDestroy()
	{
		SetAlpha(1f);
		SetAlpha_TransparentMaterials(1f);
	}

	private void SetAlpha_TransparentMaterials(float alpha)
	{
		Material[] transparentMaterials = Wall.TransparentMaterials;
		for (int i = 0; i < transparentMaterials.Length; i++)
		{
			if (!(transparentMaterials[i] == null))
			{
				Color color = transparentMaterials[i].GetColor("_Color");
				transparentMaterials[i].SetColor("_Color", new Color(color.r, color.g, color.b, alpha));
			}
		}
	}

	private void SetAlpha(float alpha)
	{
		if (alpha >= 1f)
		{
			Material[] opaqueMaterials = Wall.OpaqueMaterials;
			for (int i = 0; i < opaqueMaterials.Length; i++)
			{
				if (!(opaqueMaterials[i] == null))
				{
					Color color = opaqueMaterials[i].GetColor("_Color");
					opaqueMaterials[i].SetColor("_Color", new Color(color.r, color.g, color.b, alpha));
				}
			}
			WallRenderer.sharedMaterials = opaqueMaterials;
			return;
		}
		Material[] transparentMaterials = Wall.TransparentMaterials;
		for (int j = 0; j < transparentMaterials.Length; j++)
		{
			if (!(transparentMaterials[j] == null))
			{
				Color color2 = transparentMaterials[j].GetColor("_Color");
				transparentMaterials[j].SetColor("_Color", new Color(color2.r, color2.g, color2.b, alpha));
			}
		}
		WallRenderer.sharedMaterials = transparentMaterials;
	}

	public void SetTransparent(bool events)
	{
		if (Wall.CanBeTransparent)
		{
			IsTransparent = true;
			if (events)
			{
				Wall.OnTransparent.Invoke(Wall);
			}
			SetAlpha(TransparentValue);
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			_coroutine = StartCoroutine(SetTransparencyOff());
		}
	}

	public void SetTransparent()
	{
		SetTransparent(events: true);
	}

	private IEnumerator SetTransparencyOff()
	{
		yield return new WaitForSeconds(0.2f);
		IsTransparent = false;
		SetAlpha(1f);
		SetAlpha_TransparentMaterials(1f);
	}
}
