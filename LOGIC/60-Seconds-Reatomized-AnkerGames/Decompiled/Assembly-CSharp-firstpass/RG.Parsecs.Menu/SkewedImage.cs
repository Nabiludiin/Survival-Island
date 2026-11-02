using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class SkewedImage : Image
{
	[SerializeField]
	private float skewX;

	[SerializeField]
	private float skewY;

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		base.OnPopulateMesh(vh);
		Rect pixelAdjustedRect = GetPixelAdjustedRect();
		Vector4 vector = new Vector4(pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
		Color32 color = this.color;
		vh.Clear();
		vh.AddVert(new Vector3(vector.x - skewX, vector.y - skewY), color, new Vector2(0f, 0f));
		vh.AddVert(new Vector3(vector.x + skewX, vector.w - skewY), color, new Vector2(0f, 1f));
		vh.AddVert(new Vector3(vector.z + skewX, vector.w + skewY), color, new Vector2(1f, 1f));
		vh.AddVert(new Vector3(vector.z - skewX, vector.y + skewY), color, new Vector2(1f, 0f));
		vh.AddTriangle(0, 1, 2);
		vh.AddTriangle(2, 3, 0);
	}
}
