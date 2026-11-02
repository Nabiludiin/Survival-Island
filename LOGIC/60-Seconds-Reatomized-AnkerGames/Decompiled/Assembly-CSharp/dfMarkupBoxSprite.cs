using System;
using UnityEngine;

public class dfMarkupBoxSprite : dfMarkupBox
{
	private static int[] TRIANGLE_INDICES = new int[6] { 0, 1, 2, 0, 2, 3 };

	private dfRenderData renderData = new dfRenderData();

	public dfAtlas Atlas { get; set; }

	public string Source { get; set; }

	public dfMarkupBoxSprite(dfMarkupElement element, dfMarkupDisplayType display, dfMarkupStyle style)
		: base(element, display, style)
	{
	}

	internal void LoadImage(dfAtlas atlas, string source)
	{
		dfAtlas.ItemInfo itemInfo = atlas[source];
		if (itemInfo == null)
		{
			throw new InvalidOperationException("Sprite does not exist in atlas: " + source);
		}
		Atlas = atlas;
		Source = source;
		Size = itemInfo.sizeInPixels;
		Baseline = (int)Size.y;
	}

	protected override dfRenderData OnRebuildRenderData()
	{
		renderData.Clear();
		if (Atlas != null && Atlas[Source] != null)
		{
			dfSprite.RenderOptions options = new dfSprite.RenderOptions
			{
				atlas = Atlas,
				spriteInfo = Atlas[Source],
				pixelsToUnits = 1f,
				size = Size,
				color = Style.Color,
				baseIndex = 0,
				fillAmount = 1f,
				flip = dfSpriteFlip.None
			};
			dfSlicedSprite.renderSprite(renderData, options);
			renderData.Material = Atlas.Material;
			renderData.Transform = Matrix4x4.identity;
		}
		return renderData;
	}

	private static void addTriangleIndices(dfList<Vector3> verts, dfList<int> triangles)
	{
		int count = verts.Count;
		int[] tRIANGLE_INDICES = TRIANGLE_INDICES;
		for (int i = 0; i < tRIANGLE_INDICES.Length; i++)
		{
			triangles.Add(count + tRIANGLE_INDICES[i]);
		}
	}
}
