using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DebugGraphs : MonoBehaviour
{
	public class GraphData
	{
		public float Value;

		public float LastValue;

		public float MinValue;

		public float MaxValue;

		public Color Color;

		public float TextureScroll;

		public RenderTexture GraphTexture;

		public RawImage DisplayImage;
	}

	private static UI_DebugGraphs _Instance;

	public int Height;

	public int Padding;

	public int PixelsPerFrame;

	public RectTransform DisplayRoot;

	public Material LineDrawZOff;

	public Material ClearMaterial;

	private Camera _ViewportCamera;

	private Camera _GraphCamera;

	private UI_GraphDrawer _DrawingComponent;

	private float _UVPerFrame;

	private int _PixelWidth;

	private int _PixelHeight;

	private Dictionary<string, GraphData> _Graphs = new Dictionary<string, GraphData>();

	public static UI_DebugGraphs Singleton
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = Object.FindObjectOfType<UI_DebugGraphs>();
			}
			return _Instance;
		}
	}

	public bool Configured => _ViewportCamera != null;

	public Vector2 Size { get; private set; }

	public Vector2 FrameSize { get; private set; }

	public void Setup(Camera camera)
	{
		_ViewportCamera = camera;
		if (!(_ViewportCamera == null))
		{
			_PixelWidth = _ViewportCamera.pixelWidth;
			_PixelHeight = _ViewportCamera.pixelHeight;
			int num = _PixelWidth - Padding * 2;
			Size = new Vector2(num, Height);
			FrameSize = new Vector2(Size.x + 2f, Size.y + 2f);
			GameObject gameObject = new GameObject("GraphDrawingCamera");
			gameObject.transform.parent = DisplayRoot;
			_GraphCamera = gameObject.AddComponent<Camera>();
			_DrawingComponent = gameObject.AddComponent<UI_GraphDrawer>();
			_DrawingComponent.GraphDrawerSystem = this;
			_GraphCamera.enabled = false;
			_GraphCamera.clearFlags = CameraClearFlags.Nothing;
			_GraphCamera.cullingMask = 0;
			_GraphCamera.targetTexture = null;
			_GraphCamera.orthographic = true;
			_GraphCamera.orthographicSize = Height / 2;
			_GraphCamera.transform.position = new Vector3(num / 2, Height / 2, -10f);
			_UVPerFrame = (float)PixelsPerFrame / (float)num;
		}
	}

	private void ReConfigureResolution()
	{
		int num = _PixelWidth - Padding * 2;
		Size = new Vector2(num, Height);
		FrameSize = new Vector2(Size.x + 1f, Size.y + 1f);
		_UVPerFrame = (float)PixelsPerFrame / (float)num;
		_GraphCamera.transform.position = new Vector3(num / 2, Height / 2, -10f);
		_GraphCamera.orthographicSize = Height / 2;
		foreach (KeyValuePair<string, GraphData> graph in _Graphs)
		{
			Object.Destroy(graph.Value.GraphTexture);
			graph.Value.GraphTexture = null;
		}
	}

	public void TrackValue(string name, float value, float maxValue, Color color)
	{
		TrackValue(name, value, 0f, maxValue, color);
	}

	public void TrackValue(string name, float value, float minValue, float maxValue, Color color)
	{
		if (_Graphs.TryGetValue(name, out var value2))
		{
			value2.Value = value;
			return;
		}
		_Graphs.Add(name, new GraphData
		{
			Value = value,
			LastValue = value,
			MinValue = minValue,
			MaxValue = maxValue,
			Color = color
		});
	}

	private void Update()
	{
		if (_ViewportCamera == null)
		{
			Setup(Camera.main);
			return;
		}
		foreach (KeyValuePair<string, GraphData> graph in _Graphs)
		{
			if (graph.Value.GraphTexture == null)
			{
				SetupGraph(graph.Key, graph.Value);
			}
			_DrawingComponent.ActiveGraph = graph.Value;
			_GraphCamera.targetTexture = graph.Value.GraphTexture;
			_GraphCamera.Render();
			graph.Value.LastValue = graph.Value.Value;
			graph.Value.TextureScroll += _UVPerFrame;
			graph.Value.TextureScroll = graph.Value.TextureScroll % 1f;
			graph.Value.DisplayImage.uvRect = new Rect(graph.Value.TextureScroll, 0f, 1f, 1f);
		}
	}

	private void LateUpdate()
	{
		if (!(_ViewportCamera == null) && (_ViewportCamera.pixelWidth != _PixelWidth || _ViewportCamera.pixelHeight != _PixelHeight))
		{
			_PixelWidth = _ViewportCamera.pixelWidth;
			_PixelHeight = _ViewportCamera.pixelHeight;
			ReConfigureResolution();
		}
	}

	private void SetupGraph(string name, GraphData data)
	{
		data.GraphTexture = new RenderTexture((int)FrameSize.x, (int)FrameSize.y, 0, RenderTextureFormat.ARGB32, 0);
		data.GraphTexture.wrapMode = TextureWrapMode.Repeat;
		if (data.DisplayImage == null)
		{
			GameObject gameObject = new GameObject("Graph " + name);
			gameObject.AddComponent<RectTransform>();
			data.DisplayImage = gameObject.AddComponent<RawImage>();
			data.DisplayImage.raycastTarget = false;
		}
		data.DisplayImage.texture = data.GraphTexture;
		data.DisplayImage.rectTransform.parent = DisplayRoot;
		data.DisplayImage.rectTransform.sizeDelta = new Vector2((int)FrameSize.x, (int)FrameSize.y);
		data.DisplayImage.rectTransform.anchorMin = new Vector2(0.5f, 0f);
		data.DisplayImage.rectTransform.anchorMax = new Vector2(0.5f, 0f);
		data.DisplayImage.rectTransform.pivot = new Vector2(0.5f, 0f);
		data.DisplayImage.rectTransform.anchoredPosition = new Vector3(0f, Padding, 0f);
		data.TextureScroll = 1f - _UVPerFrame;
	}
}
