using UnityEngine;

public class UI_GraphDrawer : MonoBehaviour
{
	public UI_DebugGraphs GraphDrawerSystem;

	public UI_DebugGraphs.GraphData ActiveGraph;

	private void OnPostRender()
	{
		GraphDrawerSystem.ClearMaterial.SetPass(0);
		float num = ActiveGraph.TextureScroll * GraphDrawerSystem.Size.x;
		GL.Begin(7);
		GL.Color(new Color(0f, 0f, 0f, 0f));
		GL.Vertex3(num, 0f, 0f);
		GL.Vertex3(num, GraphDrawerSystem.FrameSize.y, 0f);
		GL.Vertex3(num + (float)GraphDrawerSystem.PixelsPerFrame + 2f, GraphDrawerSystem.FrameSize.y, 0f);
		GL.Vertex3(num + (float)GraphDrawerSystem.PixelsPerFrame + 2f, 0f, 0f);
		GL.End();
		GraphDrawerSystem.LineDrawZOff.SetPass(0);
		GL.Begin(1);
		float num2 = Mathf.InverseLerp(ActiveGraph.MinValue, ActiveGraph.MaxValue, ActiveGraph.LastValue) * GraphDrawerSystem.Size.y;
		float num3 = Mathf.InverseLerp(ActiveGraph.MinValue, ActiveGraph.MaxValue, ActiveGraph.Value) * GraphDrawerSystem.Size.y;
		GL.Color(ActiveGraph.Color);
		GL.Vertex3(num, num2 + 1f, 0f);
		GL.Vertex3(num + (float)GraphDrawerSystem.PixelsPerFrame, num3 + 1f, 0f);
		GL.End();
		GL.Begin(1);
		GL.Color(new Color(1f, 1f, 1f, 1f));
		GL.Vertex3(GraphDrawerSystem.Size.x + 2f, GraphDrawerSystem.Size.y + 2f, 0f);
		GL.Vertex3(0f, GraphDrawerSystem.Size.y + 2f, 0f);
		GL.Vertex3(GraphDrawerSystem.Size.x + 2f, 0f, 0f);
		GL.Vertex3(0f, 0f, 0f);
		GL.End();
	}
}
