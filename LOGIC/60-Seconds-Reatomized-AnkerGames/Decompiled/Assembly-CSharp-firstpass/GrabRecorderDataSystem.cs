using System.Collections.Generic;
using System.IO;
using Moments;
using Moments.Encoder;
using RG.Parsecs.Scavenge;
using UnityEngine;
using UnityToolbag;

[CreateAssetMenu(fileName = "GrabRecorderDataSystem", menuName = "Share2Socials/GrabRecorderDataSystem")]
public class GrabRecorderDataSystem : ScriptableObject
{
	private GrabRecorder _recorder;

	private GrabRecorderAdder _recorderAdder;

	private string _path;

	private float _progress;

	private bool _ready;

	private List<GifFrame> _frames;

	public GenericEvent<float> OnProgressChanged = new GenericEvent<float>();

	public GenericEvent OnReady = new GenericEvent();

	public string Path
	{
		get
		{
			if (string.IsNullOrEmpty(_path))
			{
				return StandardPaths.gifsDataDirectory + "\\valhalla.gif";
			}
			return _path;
		}
	}

	public bool Ready => _ready;

	public float Progress => _progress;

	public List<GifFrame> Frames => _frames;

	public void SetRecorder(GrabRecorder recorder, GrabRecorderAdder adder)
	{
		_path = string.Empty;
		_progress = 0f;
		_ready = false;
		_recorderAdder = adder;
		_recorder = recorder;
		_recorder.OnFileSaved.AddOneTime(OnFileSaved);
		_recorder.OnFileSaveProgress.Add(OnFileSaveProgress);
	}

	public void Clear()
	{
		_path = string.Empty;
		_progress = 0f;
		_ready = false;
		foreach (GifFrame frame in _frames)
		{
			frame?.Flush();
		}
		_frames = null;
		_recorderAdder = null;
		if (_recorder != null)
		{
			_recorder.FlushMemory(force: true);
			_recorder.OnFileSaved.Remove(OnFileSaved);
			_recorder.OnFileSaveProgress.Remove(OnFileSaveProgress);
			_recorder = null;
		}
	}

	public void DeleteGIF()
	{
		if (Ready)
		{
			if (string.IsNullOrEmpty(_path))
			{
				Clear();
				return;
			}
			File.Delete(_path);
			Clear();
		}
		else if (_recorder != null)
		{
			_recorder.OnFileSaved.AddOneTime((int i, string s, List<GifFrame> f) =>
			{
				DeleteGIF(s);
			});
		}
	}

	public void DeleteGIF(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			Clear();
			return;
		}
		File.Delete(path);
		Clear();
	}

	private void OnFileSaveProgress(int id, float progress)
	{
		_progress = progress;
		OnProgressChanged.Invoke(_progress);
	}

	private void OnFileSaved(int id, string filepath, List<GifFrame> frames)
	{
		_path = filepath;
		_recorder.OnFileSaveProgress.Remove(OnFileSaveProgress);
		_ready = true;
		_frames = frames;
		OnReady.Invoke();
	}
}
