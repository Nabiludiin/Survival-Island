using UnityEngine;

public class RecieverFrequency
{
	public static RecieverFrequency[] AllFrequencies;

	public ChunkPointType chunkPointType;

	public int[] numbers;

	public static void InitializeAllFrequencies()
	{
		AllFrequencies = new RecieverFrequency[8];
		AllFrequencies[0] = new RecieverFrequency(ChunkPointType.Landmark_RadioTower, randomizeFrequency: true);
		AllFrequencies[1] = new RecieverFrequency(ChunkPointType.Landmark_Vasagatan, randomizeFrequency: true);
		AllFrequencies[2] = new RecieverFrequency(ChunkPointType.Landmark_Balboa, randomizeFrequency: true);
		AllFrequencies[3] = new RecieverFrequency(ChunkPointType.Landmark_CaravanIsland, randomizeFrequency: true);
		AllFrequencies[4] = new RecieverFrequency(ChunkPointType.Landmark_Tangaroa, randomizeFrequency: true);
		AllFrequencies[5] = new RecieverFrequency(ChunkPointType.Landmark_VarunaPoint, randomizeFrequency: true);
		AllFrequencies[6] = new RecieverFrequency(ChunkPointType.Landmark_Temperance, randomizeFrequency: true);
		AllFrequencies[7] = new RecieverFrequency(ChunkPointType.Landmark_Utopia, randomizeFrequency: true);
	}

	public static void DoubleCheckDuplicateFrequencies()
	{
		if (AllFrequencies == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < AllFrequencies.Length; i++)
		{
			for (int j = 0; j < AllFrequencies.Length; j++)
			{
				if (i != j)
				{
					RecieverFrequency recieverFrequency = AllFrequencies[i];
					RecieverFrequency recieverFrequency2 = AllFrequencies[j];
					if (recieverFrequency != null && recieverFrequency2 != null && recieverFrequency.Equals(recieverFrequency2))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			InitializeAllFrequencies();
		}
	}

	public RecieverFrequency(ChunkPointType chunkPointType, bool randomizeFrequency)
	{
		this.chunkPointType = chunkPointType;
		numbers = new int[4];
		if (randomizeFrequency)
		{
			RandomizeFrequency();
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is RecieverFrequency { numbers: not null } recieverFrequency) || numbers == null || recieverFrequency.numbers.Length != numbers.Length)
		{
			return false;
		}
		for (int i = 0; i < recieverFrequency.numbers.Length; i++)
		{
			if (numbers[i] != recieverFrequency.numbers[i])
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		string text = "#";
		if (numbers != null)
		{
			for (int i = 0; i < numbers.Length; i++)
			{
				text += numbers[i];
			}
		}
		return text;
	}

	public bool IsValid()
	{
		if (ComponentManager<NoteBook>.Value == null || !NoteBook.unlockedChunkPointType.Contains(chunkPointType))
		{
			return false;
		}
		if (AllFrequencies != null)
		{
			for (int i = 0; i < AllFrequencies.Length; i++)
			{
				RecieverFrequency recieverFrequency = AllFrequencies[i];
				if (recieverFrequency != null && recieverFrequency.Equals(this))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RandomizeFrequency()
	{
		bool flag = true;
		while (flag)
		{
			for (int i = 0; i < numbers.Length; i++)
			{
				numbers[i] = Random.Range(0, 10);
			}
			if (AllFrequencies == null)
			{
				continue;
			}
			bool flag2 = false;
			for (int j = 0; j < AllFrequencies.Length; j++)
			{
				if (AllFrequencies[j] != null && Equals(AllFrequencies[j]))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				break;
			}
		}
	}
}
