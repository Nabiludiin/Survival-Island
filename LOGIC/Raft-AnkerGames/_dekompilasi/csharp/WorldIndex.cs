using System;

[Serializable]
public class WorldIndex
{
	public DateTime[] writeTimes;

	public int lastWrittenIndex;

	public WorldIndex(int arraySize)
	{
		lastWrittenIndex = 0;
		writeTimes = new DateTime[arraySize];
	}
}
