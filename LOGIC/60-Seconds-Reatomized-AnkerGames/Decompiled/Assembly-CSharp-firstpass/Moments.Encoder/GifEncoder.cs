using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Moments.Encoder;

public class GifEncoder
{
	protected int m_Width;

	protected int m_Height;

	protected int m_Repeat = -1;

	protected int m_FrameDelay;

	protected bool m_HasStarted;

	protected FileStream m_FileStream;

	protected GifFrame m_CurrentFrame;

	protected GifFrame m_PreviousFrame;

	protected byte[] m_Pixels;

	protected byte[] m_IndexedPixels;

	protected int m_ColorDepth;

	protected byte[] m_ColorTab;

	protected bool[] m_UsedEntry = new bool[256];

	protected int m_PaletteSize = 7;

	protected int m_DisposalCode = -1;

	protected bool m_ShouldCloseStream;

	protected bool m_IsFirstFrame = true;

	protected bool m_IsSizeSet;

	protected int m_SampleInterval = 10;

	protected int m_FramesPerColorSample = 6;

	protected NeuQuant nq;

	public bool hasTransparency = true;

	private byte _transparentColorIndex = Convert.ToByte(0);

	private byte _transparentR;

	private byte _transparentG;

	private byte _transparentB;

	public GifEncoder()
		: this(-1, 10)
	{
	}

	public float GetDifferenceFactor(GifFrame frame, GifFrame previousFrame)
	{
		if (previousFrame == null)
		{
			return 1f;
		}
		Color32[] data = frame.Data;
		Color32[] data2 = previousFrame.Data;
		int num = 0;
		int num2 = data.Length;
		for (int i = 0; i < data.Length; i++)
		{
			if (!data[i].IsEqualTo(data2[i]))
			{
				num++;
			}
		}
		return (float)num / (float)num2;
	}

	public bool IsDifferent(GifFrame frame, GifFrame previousFrame, float threshold)
	{
		if (threshold == 0f)
		{
			return true;
		}
		if (previousFrame == null)
		{
			return true;
		}
		float differenceFactor = GetDifferenceFactor(frame, previousFrame);
		Debug.LogFormat("Frame Difference: {0}", differenceFactor);
		return differenceFactor > threshold;
	}

	public void SetFramesPerColorSample(int frames)
	{
		m_FramesPerColorSample = frames;
	}

	public void BuildPalette(ref List<GifFrame> frames)
	{
		if (m_FramesPerColorSample == 0)
		{
			return;
		}
		byte[] array = new byte[3 * frames[0].Width * frames[0].Height * (1 + frames.Count / m_FramesPerColorSample)];
		int num = 0;
		for (int i = 0; i < frames.Count; i += m_FramesPerColorSample)
		{
			Color32[] data = frames[i].Data;
			for (int num2 = frames[i].Height - 1; num2 >= 0; num2--)
			{
				for (int j = 0; j < frames[i].Width; j++)
				{
					Color32 color = data[num2 * frames[i].Width + j];
					array[num] = color.r;
					num++;
					array[num] = color.g;
					num++;
					array[num] = color.b;
					num++;
				}
			}
		}
		nq = new NeuQuant(array, array.Length, m_SampleInterval);
		m_ColorTab = nq.Process();
	}

	public GifEncoder(int repeat, int quality)
	{
		if (repeat >= 0)
		{
			m_Repeat = repeat;
		}
		m_SampleInterval = Mathf.Clamp(quality, 1, 100);
	}

	public void SetDelay(int ms)
	{
		m_FrameDelay = Mathf.RoundToInt((float)ms / 10f);
	}

	public void SetFrameRate(float fps)
	{
		if (fps > 0f)
		{
			m_FrameDelay = Mathf.RoundToInt(100f / fps);
		}
	}

	public void AddFrame(GifFrame frame)
	{
		if (frame == null)
		{
			throw new ArgumentNullException("Can't add a null frame to the gif.");
		}
		if (!m_HasStarted)
		{
			throw new InvalidOperationException("Call Start() before adding frames to the gif.");
		}
		if (!m_IsSizeSet)
		{
			SetSize(frame.Width, frame.Height);
		}
		if (m_IsFirstFrame)
		{
			m_PreviousFrame = m_CurrentFrame;
		}
		m_CurrentFrame = frame;
		GetImagePixels();
		AnalyzePixels();
		if (m_IsFirstFrame)
		{
			WriteLSD();
			WritePalette();
			if (m_Repeat >= 0)
			{
				WriteNetscapeExt();
			}
		}
		WriteGraphicCtrlExt();
		WriteImageDesc();
		if (!m_IsFirstFrame && m_FramesPerColorSample == 0)
		{
			WritePalette();
		}
		WritePixels();
		m_IsFirstFrame = false;
	}

	public void Start(FileStream os)
	{
		if (os == null)
		{
			throw new ArgumentNullException("Stream is null.");
		}
		m_ShouldCloseStream = false;
		m_FileStream = os;
		try
		{
			WriteString("GIF89a");
		}
		catch (IOException ex)
		{
			throw ex;
		}
		m_HasStarted = true;
	}

	public void Start(string file)
	{
		try
		{
			m_FileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			Start(m_FileStream);
			m_ShouldCloseStream = true;
		}
		catch (IOException ex)
		{
			throw ex;
		}
	}

	public void Finish()
	{
		if (!m_HasStarted)
		{
			throw new InvalidOperationException("Can't finish a non-started gif.");
		}
		m_HasStarted = false;
		try
		{
			m_FileStream.WriteByte(59);
			m_FileStream.Flush();
			if (m_ShouldCloseStream)
			{
				m_FileStream.Close();
			}
		}
		catch (IOException ex)
		{
			throw ex;
		}
		m_FileStream = null;
		m_CurrentFrame = null;
		m_Pixels = null;
		m_IndexedPixels = null;
		m_ColorTab = null;
		m_ShouldCloseStream = false;
		m_IsFirstFrame = true;
	}

	protected void SetSize(int w, int h)
	{
		m_Width = w;
		m_Height = h;
		m_IsSizeSet = true;
	}

	protected void GetImagePixels()
	{
		m_Pixels = new byte[3 * m_CurrentFrame.Width * m_CurrentFrame.Height];
		Color32[] data = m_CurrentFrame.Data;
		int num = 0;
		if (m_PreviousFrame == null)
		{
			for (int num2 = m_CurrentFrame.Height - 1; num2 >= 0; num2--)
			{
				for (int i = 0; i < m_CurrentFrame.Width; i++)
				{
					Color32 color = data[num2 * m_CurrentFrame.Width + i];
					m_Pixels[num] = color.r;
					num++;
					m_Pixels[num] = color.g;
					num++;
					m_Pixels[num] = color.b;
					num++;
				}
			}
			return;
		}
		Color32[] data2 = m_PreviousFrame.Data;
		for (int num3 = m_CurrentFrame.Height - 1; num3 >= 0; num3--)
		{
			for (int j = 0; j < m_CurrentFrame.Width; j++)
			{
				Color32 color2 = data[num3 * m_CurrentFrame.Width + j];
				Color32 aRef = data2[num3 * m_CurrentFrame.Width + j];
				if (color2.IsEqualTo(aRef))
				{
					num++;
					num++;
					num++;
					continue;
				}
				data2[num3 * m_CurrentFrame.Width + j] = color2;
				m_Pixels[num] = color2.r;
				num++;
				m_Pixels[num] = color2.g;
				num++;
				m_Pixels[num] = color2.b;
				num++;
			}
		}
	}

	protected void AnalyzePixels()
	{
		int num = m_Pixels.Length;
		int num2 = num / 3;
		m_IndexedPixels = new byte[num2];
		if (m_FramesPerColorSample == 0)
		{
			nq = new NeuQuant(m_Pixels, num, m_SampleInterval);
			m_ColorTab = nq.Process();
		}
		int num3 = 0;
		for (int i = 0; i < num2; i++)
		{
			int num4 = nq.Map(m_Pixels[num3++] & 0xFF, m_Pixels[num3++] & 0xFF, m_Pixels[num3++] & 0xFF);
			m_UsedEntry[num4] = true;
			m_IndexedPixels[i] = (byte)num4;
		}
		if (!m_IsFirstFrame)
		{
			_transparentColorIndex = (byte)FindClosest(_transparentB & 0xFF, _transparentG & 0xFF, _transparentR & 0xFF);
		}
		m_Pixels = null;
		m_ColorDepth = 8;
		m_PaletteSize = 7;
	}

	protected int FindClosest(int b, int g, int r)
	{
		if (m_ColorTab == null)
		{
			return -1;
		}
		int result = 0;
		int num = 16777216;
		int num2 = m_ColorTab.Length;
		for (int i = 0; i < num2; i++)
		{
			int num3 = r - (m_ColorTab[i++] & 0xFF);
			int num4 = g - (m_ColorTab[i++] & 0xFF);
			int num5 = b - (m_ColorTab[i] & 0xFF);
			int num6 = num3 * num3 + num4 * num4 + num5 * num5;
			int num7 = i / 3;
			if (m_UsedEntry[num7] && num6 < num)
			{
				num = num6;
				result = num7;
			}
		}
		return result;
	}

	protected void WriteGraphicCtrlExt()
	{
		m_FileStream.WriteByte(33);
		m_FileStream.WriteByte(249);
		m_FileStream.WriteByte(4);
		if (hasTransparency && !m_IsFirstFrame)
		{
			int num = 1;
			int num2 = 1;
			num2 <<= 2;
			m_FileStream.WriteByte(Convert.ToByte(0 | num2 | 0 | num));
		}
		else
		{
			m_FileStream.WriteByte(Convert.ToByte(0));
		}
		WriteShort(m_FrameDelay);
		m_FileStream.WriteByte(_transparentColorIndex);
		m_FileStream.WriteByte(0);
	}

	protected void WriteImageDesc()
	{
		m_FileStream.WriteByte(44);
		WriteShort(0);
		WriteShort(0);
		WriteShort(m_Width);
		WriteShort(m_Height);
		if (m_IsFirstFrame)
		{
			m_FileStream.WriteByte(0);
		}
		else if (m_FramesPerColorSample != 0)
		{
			m_FileStream.WriteByte(0);
		}
		else
		{
			m_FileStream.WriteByte(Convert.ToByte(0x80 | m_PaletteSize));
		}
	}

	protected void WriteLSD()
	{
		WriteShort(m_Width);
		WriteShort(m_Height);
		m_FileStream.WriteByte(Convert.ToByte(0xF0 | m_PaletteSize));
		m_FileStream.WriteByte(0);
		m_FileStream.WriteByte(0);
	}

	protected void WriteNetscapeExt()
	{
		m_FileStream.WriteByte(33);
		m_FileStream.WriteByte(byte.MaxValue);
		m_FileStream.WriteByte(11);
		WriteString("NETSCAPE2.0");
		m_FileStream.WriteByte(3);
		m_FileStream.WriteByte(1);
		WriteShort(m_Repeat);
		m_FileStream.WriteByte(0);
	}

	protected void WritePalette()
	{
		m_FileStream.Write(m_ColorTab, 0, m_ColorTab.Length);
		int num = 768 - m_ColorTab.Length;
		for (int i = 0; i < num; i++)
		{
			m_FileStream.WriteByte(0);
		}
	}

	protected void WritePixels()
	{
		new LzwEncoder(m_Width, m_Height, m_IndexedPixels, m_ColorDepth).Encode(m_FileStream);
	}

	protected void WriteShort(int value)
	{
		m_FileStream.WriteByte(Convert.ToByte(value & 0xFF));
		m_FileStream.WriteByte(Convert.ToByte((value >> 8) & 0xFF));
	}

	protected void WriteString(string s)
	{
		char[] array = s.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			m_FileStream.WriteByte((byte)array[i]);
		}
	}
}
