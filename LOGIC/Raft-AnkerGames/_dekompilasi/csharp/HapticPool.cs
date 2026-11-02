using Unity.Collections;

public struct HapticPool
{
	private BitField32 used;

	private uint resetValue;

	public HapticPool(int maxCount)
	{
		resetValue = (uint)((1 << maxCount) - 1);
		used = new BitField32(resetValue);
	}

	public int FetchNextIndex()
	{
		if (used.CountBits() == 0)
		{
			return -1;
		}
		int num = used.CountTrailingZeros();
		used.SetBits(num, value: false);
		return num;
	}

	public void ReleaseIndex(int index)
	{
		used.SetBits(index, value: true);
	}

	public void Reset()
	{
		used = new BitField32(31u);
	}
}
