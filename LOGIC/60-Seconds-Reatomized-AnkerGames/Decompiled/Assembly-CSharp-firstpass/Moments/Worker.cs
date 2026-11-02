using System;
using System.Collections.Generic;
using System.Threading;
using Moments.Encoder;
using UnityToolbag;

namespace Moments;

internal sealed class Worker
{
	private class FrameDiff : IComparable<FrameDiff>
	{
		public GifFrame Frame;

		public float difference;

		public int CompareTo(FrameDiff other)
		{
			return difference.CompareTo(other.difference);
		}
	}

	private static int workerId = 1;

	private Thread m_Thread;

	private int m_Id;

	internal List<GifFrame> m_Frames;

	internal GifEncoder m_Encoder;

	internal string m_FilePath;

	internal Action<int, string, List<GifFrame>> m_OnFileSaved;

	internal Action<int, float> m_OnFileSaveProgress;

	internal bool CanSkipFrames;

	internal int BigSkipFrameCount;

	internal int SmallSkipFrameCount;

	internal float BigSkipThreshold;

	internal float SmallSkipThreshold;

	internal Worker(ThreadPriority priority)
	{
		m_Id = workerId++;
		m_Thread = new Thread(Run);
		m_Thread.Priority = priority;
	}

	internal void Start()
	{
		m_Thread.Start();
	}

	private void SkipFrames(List<GifFrame> source, List<GifFrame> result, int skipFramesLimit, float differenceThreshold)
	{
		GifFrame previousFrame = null;
		int num = 0;
		for (int i = 0; i < source.Count - 1; i++)
		{
			GifFrame frame = source[i];
			if (num >= skipFramesLimit || m_Encoder.IsDifferent(frame, previousFrame, differenceThreshold))
			{
				result.Add(frame);
				previousFrame = frame;
				num = 0;
			}
			else
			{
				Dispatcher.InvokeAsync(() =>
				{
					frame.Flush();
				});
				num++;
			}
		}
	}

	private void SkipNecessaryFrames(List<GifFrame> source, List<GifFrame> result, int framesToSkip)
	{
		List<FrameDiff> list = new List<FrameDiff>(source.Count);
		GifFrame previousFrame = null;
		for (int i = 0; i < source.Count - 1; i++)
		{
			GifFrame gifFrame = source[i];
			FrameDiff frameDiff = new FrameDiff();
			frameDiff.Frame = gifFrame;
			frameDiff.difference = m_Encoder.GetDifferenceFactor(gifFrame, previousFrame);
			list.Add(frameDiff);
			previousFrame = gifFrame;
		}
		list.Sort();
		result.AddRange(source);
		for (int j = 0; j < framesToSkip; j++)
		{
			GifFrame frame = list[j].Frame;
			result.Remove(frame);
			Dispatcher.InvokeAsync(() =>
			{
				frame.Flush();
			});
		}
	}

	private void Run()
	{
		m_Encoder.Start(m_FilePath);
		m_OnFileSaveProgress(m_Id, 0.05f);
		List<GifFrame> frames = new List<GifFrame>(m_Frames.Count);
		int count = m_Frames.Count;
		bool canSkipFrames = CanSkipFrames;
		int num = BigSkipFrameCount - 1;
		int num2 = SmallSkipFrameCount - 1;
		if (canSkipFrames && count > num)
		{
			SkipFrames(m_Frames, frames, 1, BigSkipThreshold);
		}
		else if (canSkipFrames && count > num2)
		{
			SkipFrames(m_Frames, frames, 1, SmallSkipThreshold);
		}
		else
		{
			for (int i = 0; i < m_Frames.Count - 1; i++)
			{
				GifFrame item = m_Frames[i];
				frames.Add(item);
			}
		}
		if (canSkipFrames && frames.Count > num)
		{
			List<GifFrame> list = new List<GifFrame>(frames.Count);
			SkipNecessaryFrames(frames, list, frames.Count - num);
			frames = list;
		}
		frames.Add(m_Frames[m_Frames.Count - 1]);
		frames.Add(m_Frames[m_Frames.Count - 1]);
		m_OnFileSaveProgress(m_Id, 0.15f);
		m_Encoder.BuildPalette(ref frames);
		m_OnFileSaveProgress(m_Id, 0.5f);
		for (int j = 0; j < frames.Count; j++)
		{
			GifFrame frame = frames[j];
			m_Encoder.AddFrame(frame);
			if (m_OnFileSaveProgress != null)
			{
				float num3 = (float)j / (float)frames.Count;
				m_OnFileSaveProgress(m_Id, 0.5f + num3 * 0.5f);
			}
		}
		m_OnFileSaveProgress(m_Id, 1f);
		m_Encoder.Finish();
		if (m_OnFileSaved != null)
		{
			m_OnFileSaved(m_Id, m_FilePath, frames);
		}
	}
}
