using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Extractor_Create : Message_NetworkBehaviour
{
	public RGD_Extractor[] rgdExtractors;

	protected bool Equals(Message_Extractor_Create other)
	{
		if (rgdExtractors == null && other.rgdExtractors != null)
		{
			return false;
		}
		if (rgdExtractors != null && other.rgdExtractors == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdExtractors != null || other.rgdExtractors != null)
			{
				return Enumerable.SequenceEqual(rgdExtractors, other.rgdExtractors);
			}
			return true;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Message_Extractor_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdExtractors);
	}

	public static bool operator ==(Message_Extractor_Create left, Message_Extractor_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Extractor_Create left, Message_Extractor_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Extractor_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdExtractors != null) ? rgdExtractors.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdExtractors == null)
		{
			return;
		}
		RGD_Extractor[] array = rgdExtractors;
		foreach (RGD_Extractor rGD_Extractor in array)
		{
			if (!(rGD_Extractor is RGD_BiofuelExtactor))
			{
				if (rGD_Extractor is RGD_Recycler)
				{
					writer.WriteValueSafe<ExtractorType>(ExtractorType.Recycler, default(FastBufferWriter.ForEnums));
				}
				else
				{
					writer.WriteValueSafe<ExtractorType>(ExtractorType.Normal, default(FastBufferWriter.ForEnums));
				}
			}
			else
			{
				writer.WriteValueSafe<ExtractorType>(ExtractorType.Biofuel, default(FastBufferWriter.ForEnums));
			}
			rGD_Extractor.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value <= 0)
		{
			return;
		}
		rgdExtractors = new RGD_Extractor[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out ExtractorType value2, default(FastBufferWriter.ForEnums));
			switch (value2)
			{
			case ExtractorType.Biofuel:
				rgdExtractors[i] = new RGD_BiofuelExtactor();
				break;
			case ExtractorType.Recycler:
				rgdExtractors[i] = new RGD_Recycler();
				break;
			case ExtractorType.Normal:
				rgdExtractors[i] = new RGD_Extractor();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			rgdExtractors[i].DeserializeFast(reader);
		}
	}

	public Message_Extractor_Create(Messages type, MonoBehaviour_Network behaviour, Placeable_Extractor[] extractors)
		: base(type, behaviour)
	{
		if (extractors == null)
		{
			return;
		}
		List<RGD_Extractor> list = new List<RGD_Extractor>();
		foreach (Placeable_Extractor placeable_Extractor in extractors)
		{
			if (placeable_Extractor != null)
			{
				RGD_Extractor save = placeable_Extractor.GetSave();
				if (save != null)
				{
					list.Add(save);
				}
			}
		}
		rgdExtractors = list.ToArray();
	}

	public void RestoreExtractors(Placeable_Extractor[] extractors)
	{
		if (extractors == null || !extractors.ContainsItems() || rgdExtractors == null || !rgdExtractors.ContainsItems())
		{
			return;
		}
		RGD_Extractor[] array = rgdExtractors;
		foreach (RGD_Extractor rGD_Extractor in array)
		{
			if (!(rGD_Extractor == null))
			{
				GetExtractorFromObjectIndex(extractors, rGD_Extractor.objectIndex).RestoreExtractor(rGD_Extractor);
			}
		}
	}

	private Placeable_Extractor GetExtractorFromObjectIndex(Placeable_Extractor[] extractors, uint objectIndex)
	{
		for (int i = 0; i < extractors.Length; i++)
		{
			if (extractors[i].ObjectIndex == objectIndex)
			{
				return extractors[i];
			}
		}
		return null;
	}
}
