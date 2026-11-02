using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Block : RGD
{
	public float x;

	public float y;

	public float z;

	public float q;

	public float w;

	public float e;

	public byte d;

	public int h;

	public int hm;

	public int i;

	public uint o;

	public uint n;

	public uint b;

	public uint u;

	public uint j;

	public uint k;

	public uint l;

	public uint v;

	public uint m;

	public Vector3 LocalBlockPosition
	{
		get
		{
			return new Vector3(x, y, z);
		}
		set
		{
			x = value.x;
			y = value.y;
			z = value.z;
		}
	}

	public Vector3 LocalBlockRotation
	{
		get
		{
			return new Vector3(q, w, e);
		}
		set
		{
			q = value.x;
			w = value.y;
			e = value.z;
		}
	}

	public DPS DPSType
	{
		get
		{
			return (DPS)d;
		}
		set
		{
			d = (byte)value;
		}
	}

	public int BlockHealth
	{
		get
		{
			return h;
		}
		set
		{
			h = value;
		}
	}

	public int BlockMaxHealth
	{
		get
		{
			return hm;
		}
		set
		{
			hm = value;
		}
	}

	public int BlockIndex
	{
		get
		{
			return i;
		}
		set
		{
			i = value;
		}
	}

	public uint BlockObjectIndex
	{
		get
		{
			return o;
		}
		set
		{
			o = value;
		}
	}

	public uint NetworkedObjectIndex
	{
		get
		{
			return n;
		}
		set
		{
			n = value;
		}
	}

	public uint NetworkedBehaviourIndex
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	public uint ColorAIndex
	{
		get
		{
			return u;
		}
		set
		{
			u = value;
		}
	}

	public uint ColorBIndex
	{
		get
		{
			return j;
		}
		set
		{
			j = value;
		}
	}

	public uint PatternColorAIndex
	{
		get
		{
			return k;
		}
		set
		{
			k = value;
		}
	}

	public uint PatternColorBIndex
	{
		get
		{
			return l;
		}
		set
		{
			l = value;
		}
	}

	public uint PatternIndex1
	{
		get
		{
			return v;
		}
		set
		{
			v = value;
		}
	}

	public uint PatternIndex2
	{
		get
		{
			return m;
		}
		set
		{
			m = value;
		}
	}

	protected bool Equals(RGD_Block other)
	{
		if (x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && q.Equals(other.q) && w.Equals(other.w) && e.Equals(other.e) && d == other.d && h == other.h && hm == other.hm && i == other.i && o == other.o && n == other.n && b == other.b && u == other.u && j == other.j && k == other.k && l == other.l && v == other.v)
		{
			return m == other.m;
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
		return Equals((RGD_Block)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(x);
		hashCode.Add(y);
		hashCode.Add(z);
		hashCode.Add(q);
		hashCode.Add(w);
		hashCode.Add(e);
		hashCode.Add(d);
		hashCode.Add(h);
		hashCode.Add(hm);
		hashCode.Add(i);
		hashCode.Add(o);
		hashCode.Add(n);
		hashCode.Add(b);
		hashCode.Add(u);
		hashCode.Add(j);
		hashCode.Add(k);
		hashCode.Add(l);
		hashCode.Add(v);
		hashCode.Add(m);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(RGD_Block left, RGD_Block right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Block left, RGD_Block right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Block(RGDType type, Block block)
		: base(type, block.rgdSortingOrder)
	{
		Constructor(block);
	}

	private void Constructor(Block block)
	{
		LocalBlockPosition = block.transform.localPosition;
		LocalBlockRotation = block.transform.localEulerAngles;
		BlockHealth = block.Health;
		BlockMaxHealth = block.MaxHealth;
		if (block.buildableItem != null)
		{
			BlockIndex = block.buildableItem.UniqueIndex;
		}
		BlockObjectIndex = block.ObjectIndex;
		DPSType = block.dpsType;
		PaintBrush.PreviewPaintData previewData = PaintBrush.PreviewData;
		if (block != null && previewData.block == block)
		{
			ColorAIndex = ((!(previewData.colorA == null)) ? previewData.colorA.uniqueColorIndex : 0u);
			ColorBIndex = ((!(previewData.colorB == null)) ? previewData.colorB.uniqueColorIndex : 0u);
			PatternColorAIndex = ((!(previewData.patternColorA == null)) ? previewData.patternColorA.uniqueColorIndex : 0u);
			PatternColorBIndex = ((!(previewData.patternColorB == null)) ? previewData.patternColorB.uniqueColorIndex : 0u);
			PatternIndex1 = previewData.patternIndex1;
			PatternIndex2 = previewData.patternIndex2;
		}
		else
		{
			ColorAIndex = ((!(block.currentColorA == null)) ? block.currentColorA.uniqueColorIndex : 0u);
			ColorBIndex = ((!(block.currentColorB == null)) ? block.currentColorB.uniqueColorIndex : 0u);
			PatternColorAIndex = ((!(block.currentPatternColorA == null)) ? block.currentPatternColorA.uniqueColorIndex : 0u);
			PatternColorBIndex = ((!(block.currentPatternColorB == null)) ? block.currentPatternColorB.uniqueColorIndex : 0u);
			PatternIndex1 = block.currentPatternIndex1;
			PatternIndex2 = block.currentPatternIndex2;
		}
		if (block.networkedBehaviour != null)
		{
			NetworkedObjectIndex = block.networkedBehaviour.ObjectIndex;
			NetworkedBehaviourIndex = block.networkedBehaviour.BehaviourIndex;
		}
	}

	public virtual void RestoreBlock(Block block)
	{
		if (block == null)
		{
			return;
		}
		if (BlockMaxHealth != -1)
		{
			block.MaxHealth = BlockMaxHealth;
		}
		if (BlockHealth != -1)
		{
			block.SetHealth(BlockHealth);
		}
		if (ColorAIndex != 0 || PatternColorAIndex != 0)
		{
			SO_ColorValue colorFromUniqueIndex = ColorPicker.GetColorFromUniqueIndex(ColorAIndex);
			SO_ColorValue colorFromUniqueIndex2 = ColorPicker.GetColorFromUniqueIndex(PatternColorAIndex);
			if (colorFromUniqueIndex != null && colorFromUniqueIndex2 != null)
			{
				block.SetInstanceColorAndPattern(colorFromUniqueIndex, colorFromUniqueIndex2, 1, PatternIndex1);
			}
		}
		if (ColorBIndex != 0 || PatternColorBIndex != 0)
		{
			SO_ColorValue colorFromUniqueIndex3 = ColorPicker.GetColorFromUniqueIndex(ColorBIndex);
			SO_ColorValue colorFromUniqueIndex4 = ColorPicker.GetColorFromUniqueIndex(PatternColorBIndex);
			if (colorFromUniqueIndex3 != null && colorFromUniqueIndex4 != null)
			{
				block.SetInstanceColorAndPattern(colorFromUniqueIndex3, colorFromUniqueIndex4, 2, PatternIndex2);
			}
		}
		if (block.buildableItem != null)
		{
			AchievementHandler.CheckAchievementForOnRestoreBlockIndex(block.buildableItem.UniqueIndex);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("BlockPositionX", x);
		info.AddValue("BlockPositionY", y);
		info.AddValue("BlockPositionZ", z);
		info.AddValue("BlockRotationX", q);
		info.AddValue("BlockRotationY", w);
		info.AddValue("BlockRotationZ", e);
		info.AddValue("DPSTypeByte", d);
		info.AddValue("BlockHealth", BlockHealth);
		info.AddValue("BlockMaxHealth", BlockMaxHealth);
		info.AddValue("BlockIndex", BlockIndex);
		info.AddValue("BlockObjectIndex", BlockObjectIndex);
		info.AddValue("NetworkedObjectIndex", NetworkedObjectIndex);
		info.AddValue("NetworkedBehaviourIndex", NetworkedBehaviourIndex);
		info.AddValue("CAI", ColorAIndex);
		info.AddValue("CBI", ColorBIndex);
		info.AddValue("PAI", PatternColorAIndex);
		info.AddValue("PBI", PatternColorBIndex);
		info.AddValue("PI1", PatternIndex1);
		info.AddValue("PI2", PatternIndex2);
	}

	public RGD_Block(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "BlockPosition":
				LocalBlockPosition = Helper.ObjToValue<SerializableVector3>(item.Value).Value;
				break;
			case "BlockRotation":
				LocalBlockRotation = Helper.ObjToValue<SerializableVector3>(item.Value).Value;
				break;
			case "BlockPositionX":
				x = Helper.ObjToValue<float>(item.Value);
				break;
			case "BlockPositionY":
				y = Helper.ObjToValue<float>(item.Value);
				break;
			case "BlockPositionZ":
				z = Helper.ObjToValue<float>(item.Value);
				break;
			case "BlockRotationX":
				q = Helper.ObjToValue<float>(item.Value);
				break;
			case "BlockRotationY":
				w = Helper.ObjToValue<float>(item.Value);
				break;
			case "BlockRotationZ":
				e = Helper.ObjToValue<float>(item.Value);
				break;
			case "BlockPositionXInt":
				x = (float)Helper.ObjToValue<int>(item.Value) / 100f;
				break;
			case "BlockPositionYInt":
				y = (float)Helper.ObjToValue<int>(item.Value) / 100f;
				break;
			case "BlockPositionZInt":
				z = (float)Helper.ObjToValue<int>(item.Value) / 100f;
				break;
			case "BlockRotationXInt":
				q = (float)Helper.ObjToValue<int>(item.Value) / 100f;
				break;
			case "BlockRotationYInt":
				w = (float)Helper.ObjToValue<int>(item.Value) / 100f;
				break;
			case "BlockRotationZInt":
				e = (float)Helper.ObjToValue<int>(item.Value) / 100f;
				break;
			case "DPSType":
				DPSType = (DPS)item.Value;
				break;
			case "BlockHealth":
				BlockHealth = (int)item.Value;
				break;
			case "BlockMaxHealth":
				BlockMaxHealth = (int)item.Value;
				break;
			case "BlockIndex":
				BlockIndex = (int)item.Value;
				break;
			case "BlockObjectIndex":
				BlockObjectIndex = (uint)item.Value;
				break;
			case "NetworkedObjectIndex":
				NetworkedObjectIndex = (uint)item.Value;
				break;
			case "NetworkedBehaviourIndex":
				NetworkedBehaviourIndex = (uint)item.Value;
				break;
			case "DPSTypeByte":
				d = Helper.ObjToValue<byte>(item.Value);
				break;
			case "ColorIndex":
			{
				uint num = (PatternColorBIndex = (uint)item.Value);
				uint num3 = (PatternColorAIndex = num);
				uint colorAIndex = (ColorBIndex = num3);
				ColorAIndex = colorAIndex;
				break;
			}
			case "CAI":
				ColorAIndex = (uint)item.Value;
				break;
			case "CBI":
				ColorBIndex = (uint)item.Value;
				break;
			case "PAI":
				PatternColorAIndex = (uint)item.Value;
				break;
			case "PBI":
				PatternColorBIndex = (uint)item.Value;
				break;
			case "PI1":
				PatternIndex1 = (uint)item.Value;
				break;
			case "PI2":
				PatternIndex2 = (uint)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		LocalBlockPosition = Vector3.zero;
		LocalBlockRotation = Vector3.zero;
		BlockHealth = -1;
		BlockMaxHealth = -1;
		BlockIndex = 0;
		BlockObjectIndex = 0u;
		NetworkedObjectIndex = 0u;
		NetworkedBehaviourIndex = 0u;
		DPSType = DPS.Default;
		ColorAIndex = 0u;
		ColorBIndex = 0u;
		PatternColorAIndex = 0u;
		PatternColorBIndex = 0u;
		PatternIndex1 = 0u;
		PatternIndex2 = 0u;
	}

	public RGD_Block()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(73))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in e, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in d, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in h, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hm, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in i, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in o, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in n, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in b, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in u, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in j, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in k, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in l, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in v, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in m, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out e, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out d, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out h, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hm, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out i, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out o, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out n, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out b, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out u, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out j, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out k, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out l, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out v, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out m, default(FastBufferWriter.ForPrimitives));
	}
}
