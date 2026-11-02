using System.Runtime.Serialization;

public class RGDTextwriter : RGD
{
	public string text;

	public uint blockObjectIndex;

	public RGDTextwriter(RGDType p_type, RGDTextwriter p_textWriter)
		: base(p_type, RGDSortingOrder.Block_Late)
	{
		string text = p_textWriter.text;
		this.text = text;
	}

	public void RestoreTextWriter(TextWriterObject p_textWriter)
	{
		if (p_textWriter != null)
		{
			p_textWriter.SetTextFiltered(p_textWriter.GetText());
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Text", text);
		info.AddValue("BlockObjectIndex", blockObjectIndex);
	}

	public RGDTextwriter(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Text"))
			{
				if (name == "BlockObjectIndex")
				{
					blockObjectIndex = (uint)item.Value;
				}
			}
			else
			{
				text = (string)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		text = string.Empty;
		blockObjectIndex = 0u;
	}
}
