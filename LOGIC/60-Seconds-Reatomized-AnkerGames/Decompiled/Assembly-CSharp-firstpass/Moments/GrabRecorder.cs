using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Moments.Encoder;
using RG.Parsecs.Scavenge;
using UnityEngine;
using UnityToolbag;

namespace Moments;

[AddComponentMenu("Miscellaneous/Moments Recorder")]
[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public sealed class GrabRecorder : MonoBehaviour
{
	[SerializeField]
	[Min(8f)]
	private int m_Width = 320;

	[SerializeField]
	[Min(8f)]
	private int m_Height = 200;

	[SerializeField]
	private bool m_AutoAspect = true;

	[SerializeField]
	[Range(1f, 30f)]
	private int m_FramePerSecond = 15;

	[SerializeField]
	[Min(-1f)]
	private int m_Repeat;

	[SerializeField]
	[Range(1f, 100f)]
	private int m_Quality = 15;

	[SerializeField]
	[Min(0.1f)]
	private float m_BufferSize = 3f;

	[SerializeField]
	[Range(0f, 60f)]
	private int m_FramesPerColorSample = 6;

	[SerializeField]
	[Range(0f, 100f)]
	private int m_SkippedFramesLimit;

	[SerializeField]
	[Range(0f, 1f)]
	private float m_MinimalDifference;

	[SerializeField]
	private bool m_FlushFrames = true;

	internal bool CanSkipFrames;

	internal int BigSkipFrameCount;

	internal int SmallSkipFrameCount;

	internal float BigSkipThreshold;

	internal float SmallSkipThreshold;

	public System.Threading.ThreadPriority WorkerPriority;

	public Action OnPreProcessingDone;

	public GenericEvent<int, float> OnFileSaveProgress = new GenericEvent<int, float>();

	public GenericEvent<int, string, List<GifFrame>> OnFileSaved = new GenericEvent<int, string, List<GifFrame>>();

	private int m_MaxFrameCount;

	private float m_Time;

	private float m_TimePerFrame;

	private Queue<RenderTexture> m_Frames;

	private RenderTexture m_RecycledRenderTexture;

	private ReflectionUtils<GrabRecorder> m_ReflectionUtils;

	public RecorderState State { get; private set; }

	public string SaveFolder { get; set; }

	public float EstimatedMemoryUse => (float)m_FramePerSecond * m_BufferSize * (float)(m_Width * m_Height * 4) / 1048576f;

	public Queue<RenderTexture> GetFrames()
	{
		return m_Frames;
	}

	public void Setup(bool autoAspect, int width, int height, int fps, float bufferSize, int repeat, int quality, int framesPerColorSample, int skippedFramesLimit)
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to setup the component during the pre-processing step.");
			return;
		}
		FlushMemory(force: true);
		m_AutoAspect = autoAspect;
		m_ReflectionUtils.ConstrainMin((GrabRecorder x) => x.m_Width, width);
		if (!autoAspect)
		{
			m_ReflectionUtils.ConstrainMin((GrabRecorder x) => x.m_Height, height);
		}
		m_ReflectionUtils.ConstrainRange((GrabRecorder x) => x.m_FramePerSecond, fps);
		m_ReflectionUtils.ConstrainMin((GrabRecorder x) => x.m_BufferSize, bufferSize);
		m_ReflectionUtils.ConstrainMin((GrabRecorder x) => x.m_Repeat, repeat);
		m_ReflectionUtils.ConstrainRange((GrabRecorder x) => x.m_Quality, quality);
		m_ReflectionUtils.ConstrainRange((GrabRecorder x) => x.m_FramesPerColorSample, framesPerColorSample);
		m_ReflectionUtils.ConstrainRange((GrabRecorder x) => x.m_SkippedFramesLimit, skippedFramesLimit);
		Init();
	}

	internal int GetWidth()
	{
		return m_Width;
	}

	internal int GetHeight()
	{
		return m_Height;
	}

	public void Pause()
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to pause recording during the pre-processing step. The recorder is automatically paused when pre-processing.");
		}
		else
		{
			State = RecorderState.Paused;
		}
	}

	public void Record()
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to resume recording during the pre-processing step.");
		}
		else
		{
			State = RecorderState.Recording;
		}
	}

	public void FlushMemory(bool force = false)
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to flush memory during the pre-processing step.");
			return;
		}
		Init();
		if (m_FlushFrames | force)
		{
			FlushFrames();
		}
	}

	public void FlushFrames()
	{
		if (m_RecycledRenderTexture != null)
		{
			Flush(m_RecycledRenderTexture);
		}
		if (m_Frames == null)
		{
			return;
		}
		foreach (RenderTexture frame in m_Frames)
		{
			Flush(frame);
		}
		m_Frames.Clear();
	}

	public void Save()
	{
		Save(GenerateFileName());
	}

	public void Save(string filename)
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to save during the pre-processing step.");
			return;
		}
		if (m_Frames.Count == 0)
		{
			Debug.LogWarning("Nothing to save. Maybe you forgot to start the recorder ?");
			return;
		}
		State = RecorderState.PreProcessing;
		if (string.IsNullOrEmpty(filename))
		{
			filename = GenerateFileName();
		}
		StartCoroutine(PreProcess(filename));
	}

	private void Awake()
	{
		m_ReflectionUtils = new ReflectionUtils<GrabRecorder>(this);
		m_Frames = new Queue<RenderTexture>();
		Init();
	}

	private void OnDestroy()
	{
		FlushMemory();
	}

	public void AddFrame(Texture tex)
	{
		if (m_Frames.Count >= m_MaxFrameCount)
		{
			m_RecycledRenderTexture = m_Frames.Dequeue();
		}
		RenderTexture renderTexture = m_RecycledRenderTexture;
		m_RecycledRenderTexture = null;
		if (renderTexture == null)
		{
			renderTexture = new RenderTexture(m_Width, m_Height, 0, RenderTextureFormat.ARGB32);
			renderTexture.wrapMode = TextureWrapMode.Clamp;
			renderTexture.filterMode = FilterMode.Bilinear;
			renderTexture.anisoLevel = 0;
		}
		Graphics.Blit(tex, renderTexture);
		m_Frames.Enqueue(renderTexture);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (State != RecorderState.Recording)
		{
			Graphics.Blit(source, destination);
			return;
		}
		State = RecorderState.Paused;
		if (m_Frames.Count >= m_MaxFrameCount)
		{
			m_RecycledRenderTexture = m_Frames.Dequeue();
		}
		RenderTexture renderTexture = m_RecycledRenderTexture;
		m_RecycledRenderTexture = null;
		if (renderTexture == null)
		{
			renderTexture = new RenderTexture(m_Width, m_Height, 0, RenderTextureFormat.ARGB32);
			renderTexture.wrapMode = TextureWrapMode.Clamp;
			renderTexture.filterMode = FilterMode.Bilinear;
			renderTexture.anisoLevel = 0;
		}
		Graphics.Blit(source, renderTexture);
		m_Frames.Enqueue(renderTexture);
		Graphics.Blit(source, destination);
	}

	private void Init()
	{
		State = RecorderState.Paused;
		ComputeHeight();
		m_MaxFrameCount = Mathf.RoundToInt(m_BufferSize * (float)m_FramePerSecond);
		m_TimePerFrame = 1f / (float)m_FramePerSecond;
		m_Time = 0f;
		if (string.IsNullOrEmpty(SaveFolder))
		{
			SaveFolder = StandardPaths.gifsDataDirectory;
		}
	}

	public void ComputeHeight()
	{
		if (m_AutoAspect)
		{
			m_Height = Mathf.RoundToInt((float)m_Width / GetComponent<Camera>().aspect);
		}
	}

	public void Flush(UnityEngine.Object obj)
	{
		UnityEngine.Object.Destroy(obj);
	}

	private string GenerateFileName()
	{
		string text = DateTime.Now.ToString("yyyyMMddHHmmssffff");
		return "GifCapture-" + text;
	}

	private IEnumerator PreProcess(string filename)
	{
		string filepath = SaveFolder + "/" + filename + ".gif";
		List<GifFrame> frames = new List<GifFrame>(m_Frames.Count);
		while (m_Frames.Count > 0)
		{
			Texture2D texture2D = new Texture2D(m_Width, m_Height, TextureFormat.RGB24, mipChain: false);
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.anisoLevel = 0;
			GifFrame item = ToGifFrame(m_Frames.Dequeue(), texture2D);
			frames.Add(item);
			yield return null;
		}
		State = RecorderState.Paused;
		if (OnPreProcessingDone != null)
		{
			OnPreProcessingDone();
		}
		GifEncoder gifEncoder = new GifEncoder(m_Repeat, m_Quality);
		gifEncoder.SetDelay(Mathf.RoundToInt(m_TimePerFrame * 1000f));
		gifEncoder.SetFramesPerColorSample(m_FramesPerColorSample);
		Worker worker = new Worker(WorkerPriority);
		worker.m_Encoder = gifEncoder;
		worker.m_Frames = frames;
		worker.m_FilePath = filepath;
		worker.m_OnFileSaved = OnFileSaved.Invoke;
		worker.m_OnFileSaveProgress = OnFileSaveProgress.Invoke;
		worker.CanSkipFrames = CanSkipFrames;
		worker.BigSkipFrameCount = BigSkipFrameCount;
		worker.SmallSkipFrameCount = SmallSkipFrameCount;
		worker.BigSkipThreshold = BigSkipThreshold;
		worker.SmallSkipThreshold = SmallSkipThreshold;
		worker.Start();
	}

	private GifFrame ToGifFrame(RenderTexture source, Texture2D target)
	{
		RenderTexture.active = source;
		target.ReadPixels(new Rect(0f, 0f, source.width, source.height), 0, 0);
		target.Apply();
		RenderTexture.active = null;
		return new GifFrame
		{
			Width = target.width,
			Height = target.height,
			Data = target.GetPixels32(),
			Texture = target
		};
	}
}
