using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkItem_TradingPost : RGD_LandmarkItem
{
	public RGD_TradingPost rgdTradingPost;

	protected bool Equals(RGD_LandmarkItem_TradingPost other)
	{
		if (Equals((RGD_LandmarkItem)other))
		{
			return object.Equals(rgdTradingPost, other.rgdTradingPost);
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
		return Equals((RGD_LandmarkItem_TradingPost)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdTradingPost);
	}

	public static bool operator ==(RGD_LandmarkItem_TradingPost left, RGD_LandmarkItem_TradingPost right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkItem_TradingPost left, RGD_LandmarkItem_TradingPost right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdTradingPost.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdTradingPost = new RGD_TradingPost();
		rgdTradingPost.DeserializeFast(reader);
	}

	public RGD_LandmarkItem_TradingPost()
	{
	}

	public RGD_LandmarkItem_TradingPost(LandmarkItem landmarkItem)
		: base(landmarkItem)
	{
		TradingPost component = landmarkItem.GetComponent<TradingPost>();
		if (component != null)
		{
			rgdTradingPost = new RGD_TradingPost(component);
		}
		else
		{
			rgdTradingPost = null;
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		rgdTradingPost.Restore(landmarkItem.GetComponent<TradingPost>());
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("TradingPost", rgdTradingPost);
	}

	public RGD_LandmarkItem_TradingPost(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "TradingPost")
			{
				rgdTradingPost = (RGD_TradingPost)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdTradingPost = null;
	}
}
