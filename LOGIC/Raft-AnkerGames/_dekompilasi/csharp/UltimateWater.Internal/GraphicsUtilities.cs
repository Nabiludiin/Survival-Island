using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater.Internal;

public static class GraphicsUtilities
{
	private static CommandBuffer _CommandBuffer;

	public static void Blit(Texture source, RenderTexture target, Material material, int shaderPass, MaterialPropertyBlock properties)
	{
		if (_CommandBuffer == null)
		{
			_CommandBuffer = new CommandBuffer
			{
				name = "UltimateWater Custom Blit"
			};
		}
		GL.PushMatrix();
		GL.modelview = Matrix4x4.identity;
		GL.LoadProjectionMatrix(Matrix4x4.identity);
		material.mainTexture = source;
		_CommandBuffer.SetRenderTarget(target);
		_CommandBuffer.DrawMesh(Quads.TriangleMesh, Matrix4x4.identity, material, 0, shaderPass, properties);
		Graphics.ExecuteCommandBuffer(_CommandBuffer);
		RenderTexture.active = target;
		GL.PopMatrix();
		_CommandBuffer.Clear();
	}
}
