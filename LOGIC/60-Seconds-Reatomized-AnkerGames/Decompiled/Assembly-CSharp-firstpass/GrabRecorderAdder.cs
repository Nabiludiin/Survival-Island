using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Moments;
using Moments.Encoder;
using RG.Core.SaveSystem;
using RG.Parsecs.Survival;
using UnityEngine;
using UnityToolbag;

public class GrabRecorderAdder : MonoBehaviour, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	public GrabRecorderDataSystem GrabRecorderDataSystem;

	private bool AllowRecording = true;

	private bool autoAspect = true;

	private int width = 700;

	private int height = 338;

	private int fps = 2;

	private float bufferSize = 45f;

	private int repeat;

	private int quality = 10;

	private int framesPerColorSample = 1;

	private float minimalDifference = 0.065f;

	private int skippedFramesLimit = 1;

	private bool CanSkipFrames = true;

	private int BigSkipFrameCount = 57;

	private int SmallSkipFrameCount = 25;

	private float BigSkipThreshold = 0.05f;

	private float SmallSkipThreshold = 0.03f;

	public bool Exist;

	public EndOfDayListenerList ListenerList;

	public EndGameData EndGameData;

	public Camera Camera;

	public SurvivalData SurvivalData;

	private GrabRecorder _recorder;

	private bool _saving;

	public RecordedFrames SerializationFrames = new RecordedFrames();

	private Texture2D serializationTexture;

	public static GrabRecorderAdder I { get; protected set; }

	public string ID => "MB_GrabRecorderAdder";

	private void Awake()
	{
		Register();
	}

	public void OnDayStarted()
	{
		if (EndGameData.RuntimeData.ShouldEndGame)
		{
			Save();
		}
	}

	public void OnDayEnded()
	{
		Grab();
	}

	public void OnFirstDayStarted()
	{
		if (SchedulerManager.Instance.FromSave)
		{
			Grab();
			ListenerList.UnregisterOnEndOfDay(OnFirstDayStarted, "Visuals");
		}
		else if (SurvivalData.CurrentDay == 1)
		{
			Grab();
			ListenerList.UnregisterOnEndOfDay(OnFirstDayStarted, "Visuals");
		}
	}

	private void SetUp()
	{
		if (AllowRecording && !Exist)
		{
			Exist = true;
			GrabRecorder grabRecorder = (_recorder = GetComponent<GrabRecorder>());
			if (grabRecorder == null)
			{
				grabRecorder = (_recorder = gameObject.AddComponent<GrabRecorder>());
			}
			grabRecorder.Setup(autoAspect, width, height, fps, bufferSize, repeat, quality, framesPerColorSample, skippedFramesLimit);
			grabRecorder.CanSkipFrames = CanSkipFrames;
			grabRecorder.BigSkipFrameCount = BigSkipFrameCount;
			grabRecorder.SmallSkipFrameCount = SmallSkipFrameCount;
			grabRecorder.BigSkipThreshold = BigSkipThreshold;
			grabRecorder.SmallSkipThreshold = SmallSkipThreshold;
			grabRecorder.OnFileSaved.Add(OnFileSaved);
			grabRecorder.OnFileSaveProgress.Add(OnFileSaveProgress);
			_saving = false;
			ListenerList.RegisterOnDayStarted(OnDayStarted, "ChangeFinished", int.MaxValue);
			ListenerList.RegisterOnEndOfDay(OnDayEnded, "Reset", int.MaxValue, this);
			ListenerList.RegisterOnEndOfDay(OnFirstDayStarted, "Visuals", int.MaxValue, this);
			serializationTexture = new Texture2D(_recorder.GetWidth(), _recorder.GetHeight(), TextureFormat.RGB24, mipChain: false);
			serializationTexture.hideFlags = HideFlags.HideAndDontSave;
			serializationTexture.wrapMode = TextureWrapMode.Clamp;
			serializationTexture.filterMode = FilterMode.Bilinear;
			serializationTexture.anisoLevel = 0;
		}
	}

	private void Start()
	{
		I = this;
		SetUp();
	}

	private void OnDestroy()
	{
		if (AllowRecording)
		{
			_recorder.Flush(serializationTexture);
		}
		Unregister();
		I = null;
		Exist = false;
		ListenerList.UnregisterOnDayStarted(OnDayStarted, "ChangeFinished");
		ListenerList.UnregisterOnEndOfDay(OnDayEnded, "Reset");
		ListenerList.UnregisterOnEndOfDay(OnFirstDayStarted, "Visuals");
	}

	private void OnFileSaveProgress(int id, float progress)
	{
	}

	private void OnFileSaved(int id, string filepath, List<GifFrame> frames)
	{
		_saving = false;
	}

	public void Grab()
	{
		StartCoroutine(GrabCoroutine());
	}

	private IEnumerator GrabCoroutine()
	{
		Camera.enabled = true;
		_recorder.Record();
		yield return null;
		Camera.enabled = false;
		yield return null;
	}

	[ContextMenu("Save100")]
	public void Save100()
	{
		StartCoroutine(Grab100());
	}

	private IEnumerator Grab100()
	{
		for (int i = 0; i < 100; i++)
		{
			Grab();
			yield return null;
			yield return null;
			yield return null;
			yield return null;
		}
		Save();
	}

	[ContextMenu("Save")]
	public void Save()
	{
		if (!AllowRecording || _saving || SteamManager.IsRunningOnSteamDeck())
		{
			return;
		}
		_saving = true;
		if (GrabRecorderDataSystem != null)
		{
			GrabRecorderDataSystem.SetRecorder(_recorder, this);
		}
		foreach (string frameName in SerializationFrames.FrameNames)
		{
			if (File.Exists(frameName))
			{
				File.Delete(frameName);
			}
		}
		SerializationFrames.FrameNames.Clear();
		StartCoroutine(DelayedSave());
	}

	private IEnumerator DelayedSave()
	{
		if (_recorder.State != RecorderState.PreProcessing)
		{
			_recorder.Save();
			yield return null;
		}
	}

	public string Serialize()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		_ = string.Empty;
		List<RenderTexture> list = new List<RenderTexture>(_recorder.GetFrames());
		for (int i = SerializationFrames.FrameNames.Count; i < list.Count; i++)
		{
			ToTexture2D(list[i], serializationTexture);
			SerializationFrames.FrameNames.Add(SaveFrame(i, serializationTexture));
		}
		string result = JsonUtility.ToJson(SerializationFrames);
		_ = SerializationFrames.Frames.Count;
		stopwatch.Stop();
		return result;
	}

	private string SaveFrame(int i, Texture2D serializationTexture)
	{
		string text = StandardPaths.gifsDataDirectory + "/Raw/";
		Directory.CreateDirectory(text);
		string text2 = text + i + ".t2d";
		byte[] rawTextureData = serializationTexture.GetRawTextureData();
		FrameSaveData state = new FrameSaveData
		{
			Path = text2,
			Bytes = rawTextureData
		};
		ThreadPool.QueueUserWorkItem(saveFile, state);
		return text2;
	}

	private void saveFile(object state)
	{
		FrameSaveData frameSaveData = (FrameSaveData)state;
		File.WriteAllBytes(frameSaveData.Path, frameSaveData.Bytes);
	}

	private void ToTexture2D(RenderTexture source, Texture2D target)
	{
		RenderTexture.active = source;
		target.ReadPixels(new Rect(0f, 0f, source.width, source.height), 0, 0);
		target.Apply();
		RenderTexture.active = null;
	}

	public void Deserialize(string jsonData)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		int num = 0;
		SetUp();
		SerializationFrames = JsonUtility.FromJson<RecordedFrames>(jsonData);
		if (SerializationFrames.Frames != null && SerializationFrames.Frames.Count > 0)
		{
			Texture2D texture2D = new Texture2D(_recorder.GetWidth(), _recorder.GetHeight(), TextureFormat.RGB24, mipChain: false);
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.anisoLevel = 0;
			foreach (ByteArrayContainer frame in SerializationFrames.Frames)
			{
				texture2D.LoadImage(frame.Bytes);
				_recorder.AddFrame(texture2D);
			}
			_recorder.Flush(texture2D);
			num = SerializationFrames.Frames.Count;
			SerializationFrames.Frames.Clear();
		}
		else
		{
			Texture2D texture2D2 = new Texture2D(_recorder.GetWidth(), _recorder.GetHeight(), TextureFormat.RGB24, mipChain: false);
			texture2D2.hideFlags = HideFlags.HideAndDontSave;
			texture2D2.wrapMode = TextureWrapMode.Clamp;
			texture2D2.filterMode = FilterMode.Bilinear;
			texture2D2.anisoLevel = 0;
			foreach (string frameName in SerializationFrames.FrameNames)
			{
				try
				{
					byte[] array = File.ReadAllBytes(frameName);
					if (array.Length == 0)
					{
						UnityEngine.Debug.LogError("Tried to read frame " + frameName + " but bytes lenght is 0");
						continue;
					}
					texture2D2.LoadRawTextureData(array);
					texture2D2.Apply();
					_recorder.AddFrame(texture2D2);
				}
				catch (DirectoryNotFoundException ex)
				{
					UnityEngine.Debug.LogErrorFormat("GIF deserialization. Gracefully handled: {0}", ex.Message);
				}
				catch (FileNotFoundException ex2)
				{
					UnityEngine.Debug.LogErrorFormat("GIF deserialization. Gracefully handled: {0}", ex2.Message);
				}
				catch (UnityException ex3)
				{
					UnityEngine.Debug.LogErrorFormat("GIF deserialization. Gracefully handled: {0}", ex3.Message);
				}
			}
			_recorder.Flush(texture2D2);
			num = SerializationFrames.FrameNames.Count;
		}
		stopwatch.Stop();
		UnityEngine.Debug.LogFormat("Grab recorder Deserialization. Frames: {0}, Time: {1}", num, stopwatch.ElapsedMilliseconds);
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
	}
}
