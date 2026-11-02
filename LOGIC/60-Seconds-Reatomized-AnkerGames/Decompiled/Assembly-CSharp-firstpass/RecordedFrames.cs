using System;
using System.Collections.Generic;

[Serializable]
public class RecordedFrames
{
	public List<ByteArrayContainer> Frames = new List<ByteArrayContainer>();

	public List<string> FrameNames = new List<string>();
}
