using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_TradingPost : SerializableBehaviour
{
	[Serializable]
	public class RGD_TradingPostItem
	{
		public int uniqueItemIndex;

		public int stock;

		protected bool Equals(RGD_TradingPostItem other)
		{
			if (uniqueItemIndex == other.uniqueItemIndex)
			{
				return stock == other.stock;
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
			return Equals((RGD_TradingPostItem)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(uniqueItemIndex, stock);
		}

		public static bool operator ==(RGD_TradingPostItem left, RGD_TradingPostItem right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(RGD_TradingPostItem left, RGD_TradingPostItem right)
		{
			return !object.Equals(left, right);
		}

		public RGD_TradingPostItem()
		{
		}

		public RGD_TradingPostItem(int uniqueItemIndex, int stock)
		{
			this.uniqueItemIndex = uniqueItemIndex;
			this.stock = stock;
		}
	}

	public RGD_TradingPostItem[] items;

	protected bool Equals(RGD_TradingPost other)
	{
		if (items == null && other.items != null)
		{
			return false;
		}
		if (items != null && other.items == null)
		{
			return false;
		}
		if (items != null || other.items != null)
		{
			return Enumerable.SequenceEqual(items, other.items);
		}
		return true;
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
		return Equals((RGD_TradingPost)obj);
	}

	public override int GetHashCode()
	{
		if (items == null)
		{
			return 0;
		}
		return items.GetHashCode();
	}

	public static bool operator ==(RGD_TradingPost left, RGD_TradingPost right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_TradingPost left, RGD_TradingPost right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(items.Length, default(FastBufferWriter.ForPrimitives));
		RGD_TradingPostItem[] array = items;
		foreach (RGD_TradingPostItem rGD_TradingPostItem in array)
		{
			writer.WriteValueSafe(in rGD_TradingPostItem.uniqueItemIndex, default(FastBufferWriter.ForPrimitives));
			writer.WriteValueSafe(in rGD_TradingPostItem.stock, default(FastBufferWriter.ForPrimitives));
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			items = new RGD_TradingPostItem[value];
			for (int i = 0; i < value; i++)
			{
				items[i] = new RGD_TradingPostItem();
				reader.ReadValue(out items[i].uniqueItemIndex, default(FastBufferWriter.ForPrimitives));
				reader.ReadValue(out items[i].stock, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public RGD_TradingPost()
	{
	}

	public RGD_TradingPost(TradingPost tradingPost)
	{
		items = new RGD_TradingPostItem[tradingPost.buyableItems.Count];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new RGD_TradingPostItem(tradingPost.buyableItems[i].reward.item.UniqueIndex, tradingPost.buyableItems[i].stock);
		}
	}

	public void Restore(TradingPost tradingPost)
	{
		if (!(tradingPost != null) || items == null)
		{
			return;
		}
		tradingPost.ResetItemsForSale();
		RGD_TradingPostItem[] array = items;
		foreach (RGD_TradingPostItem rGD_TradingPostItem in array)
		{
			if (rGD_TradingPostItem == null)
			{
				continue;
			}
			foreach (SO_TradingPost_Buyable.Instance buyableItem in tradingPost.buyableItems)
			{
				if (buyableItem.reward.item.UniqueIndex == rGD_TradingPostItem.uniqueItemIndex)
				{
					buyableItem.stock = rGD_TradingPostItem.stock;
					break;
				}
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Items", items);
	}

	public RGD_TradingPost(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Items")
			{
				items = (RGD_TradingPostItem[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		items = null;
	}
}
