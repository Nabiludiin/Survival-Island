using System.Collections;
using System.Collections.Generic;
using Moments;
using Moments.Encoder;
using UnityEngine;

public class RecorderAdder : MonoBehaviour
{
	public bool AllowRecording;

	public bool autoAspect = true;

	public int width = 600;

	public int height = 338;

	public int fps = 24;

	public float bufferSize = 4f;

	public int repeat;

	public int quality = 1;

	public float SavingDelay = 1f;

	public static bool Exist;

	private Recorder _recorder;

	private bool _saving;

	public static RecorderAdder I { get; protected set; }

	private void Start()
	{
		I = this;
		if (AllowRecording)
		{
			Exist = true;
			Recorder recorder = (_recorder = GetComponent<Recorder>());
			if (recorder == null)
			{
				recorder = (_recorder = gameObject.AddComponent<Recorder>());
			}
			recorder.Setup(autoAspect, width, height, fps, bufferSize, repeat, quality);
			recorder.OnFileSaved = OnFileSaved;
			recorder.Record();
			_saving = false;
		}
	}

	private void OnDestroy()
	{
		I = null;
		Exist = false;
	}

	private void OnFileSaved(int id, string filepath, List<GifFrame> frames)
	{
		_saving = false;
		if (AllowRecording)
		{
			_recorder.Record();
		}
	}

	public void Save()
	{
		if (AllowRecording && !_saving)
		{
			_saving = true;
			StartCoroutine(DelayedSave());
		}
	}

	private IEnumerator DelayedSave()
	{
		yield return new WaitForSeconds(SavingDelay);
		if (_recorder.State != RecorderState.PreProcessing)
		{
			_recorder.Save();
		}
	}
}
