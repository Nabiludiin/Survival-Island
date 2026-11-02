using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Knife.DeferredDecals;

public class CustomRendererManager
{
	private static CustomRendererManager m_Instance;

	internal Dictionary<Renderer, int[]> m_CustomRenderers = new Dictionary<Renderer, int[]>();

	private CommandBuffer renderBuffer;

	private Material renderingMaterial;

	private int globalTexID = -1;

	public static CustomRendererManager instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new CustomRendererManager();
			}
			return m_Instance;
		}
	}

	public int GlobalTexID
	{
		get
		{
			if (globalTexID == -1)
			{
				globalTexID = Shader.PropertyToID("_GlobalTex");
			}
			return globalTexID;
		}
	}

	public void SetRenderBuffer(CommandBuffer renderBuffer, Material renderingMaterial)
	{
		this.renderBuffer = renderBuffer;
		this.renderingMaterial = renderingMaterial;
		UpdateBuffer();
	}

	public void AddCustomRendererID(Renderer targetRenderer, int[] submeshes)
	{
		if (!m_CustomRenderers.ContainsKey(targetRenderer))
		{
			m_CustomRenderers.Add(targetRenderer, submeshes);
			if (renderBuffer != null)
			{
				renderBuffer.Clear();
				UpdateBuffer();
			}
		}
	}

	private void UpdateBuffer()
	{
		renderBuffer.EnableShaderKeyword("GLOBAL");
		foreach (KeyValuePair<Renderer, int[]> customRenderer in m_CustomRenderers)
		{
			for (int i = 0; i < customRenderer.Value.Length; i++)
			{
				renderBuffer.SetGlobalTexture(GlobalTexID, customRenderer.Key.sharedMaterials[i].mainTexture);
				renderBuffer.DrawRenderer(customRenderer.Key, renderingMaterial, customRenderer.Value[i]);
			}
		}
		renderBuffer.SetGlobalTexture(GlobalTexID, Texture2D.whiteTexture);
	}

	public void RemoveCustomRendererID(Renderer id)
	{
		if (m_CustomRenderers.ContainsKey(id))
		{
			m_CustomRenderers.Remove(id);
			if (renderBuffer != null)
			{
				renderBuffer.Clear();
				UpdateBuffer();
			}
		}
	}
}
