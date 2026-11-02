using System;
using UnityEngine;

[Serializable]
public class HairStyleConnection
{
	public HairStyle hairStyle;

	public SkinnedMeshRenderer blendShapeRenderer;

	public int blendShapeIndex;

	[Range(0f, 100f)]
	public float blendShapeValue;

	public GameObject[] models;

	public void SetState(bool state)
	{
		for (int i = 0; i < models.Length; i++)
		{
			GameObject gameObject = models[i];
			if (gameObject != null)
			{
				gameObject.SetActiveSafe(state);
			}
		}
		if (blendShapeRenderer != null)
		{
			if (state)
			{
				blendShapeRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeValue);
			}
			else
			{
				blendShapeRenderer.SetBlendShapeWeight(blendShapeIndex, 0f);
			}
		}
	}
}
