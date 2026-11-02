using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CanvasPrinter : MonoBehaviour
{
	private static CanvasPrinter instance;

	private Camera canvasCamera;

	public static Texture RenderCanvasToMaterial(Canvas canvas, Material targetMaterial)
	{
		if (instance == null)
		{
			instance = Object.FindObjectOfType<CanvasPrinter>();
		}
		if (instance == null)
		{
			instance = new GameObject("CanvasPrinter").AddComponent<CanvasPrinter>();
			if (Application.isPlaying)
			{
				Object.DontDestroyOnLoad(instance.gameObject);
			}
			instance.canvasCamera = instance.gameObject.AddComponent<Camera>();
			instance.canvasCamera.enabled = false;
			instance.canvasCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
			instance.canvasCamera.clearFlags = CameraClearFlags.Color;
			instance.canvasCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		}
		RenderTexture renderTexture = instance.RenderCanvasToMaterialImp(canvas);
		targetMaterial.SetTexture("_MainTex", renderTexture);
		return renderTexture;
	}

	private RenderTexture RenderCanvasToMaterialImp(Canvas canvas)
	{
		RectTransform component = canvas.GetComponent<RectTransform>();
		RenderTextureDescriptor desc = new RenderTextureDescriptor(Mathf.CeilToInt(component.sizeDelta.x), Mathf.CeilToInt(component.sizeDelta.y), GraphicsFormat.R8G8B8A8_UNorm, 0, 4);
		desc.autoGenerateMips = true;
		desc.useMipMap = true;
		canvasCamera.targetTexture = new RenderTexture(desc);
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = instance.canvasCamera;
		canvasCamera.Render();
		return canvasCamera.targetTexture;
	}
}
