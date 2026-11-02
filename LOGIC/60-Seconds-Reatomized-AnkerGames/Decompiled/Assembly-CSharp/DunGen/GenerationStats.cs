using System.Diagnostics;

namespace DunGen;

public sealed class GenerationStats
{
	private Stopwatch stopwatch = new Stopwatch();

	private GenerationStatus generationStatus;

	public int MainPathRoomCount { get; private set; }

	public int BranchPathRoomCount { get; private set; }

	public int TotalRoomCount { get; private set; }

	public int MaxBranchDepth { get; private set; }

	public int TotalRetries { get; private set; }

	public float PreProcessTime { get; private set; }

	public float MainPathGenerationTime { get; private set; }

	public float BranchPathGenerationTime { get; private set; }

	public float PostProcessTime { get; private set; }

	public float TotalTime { get; private set; }

	internal void Clear()
	{
		int num = (TotalRetries = 0);
		int num3 = (MaxBranchDepth = num);
		int num5 = (TotalRoomCount = num3);
		int mainPathRoomCount = (BranchPathRoomCount = num5);
		MainPathRoomCount = mainPathRoomCount;
		float num8 = (TotalTime = 0f);
		float num10 = (PostProcessTime = num8);
		float num12 = (BranchPathGenerationTime = num10);
		float preProcessTime = (MainPathGenerationTime = num12);
		PreProcessTime = preProcessTime;
	}

	internal void IncrementRetryCount()
	{
		TotalRetries++;
	}

	internal void SetRoomStatistics(int mainPathRoomCount, int branchPathRoomCount, int maxBranchDepth)
	{
		MainPathRoomCount = mainPathRoomCount;
		BranchPathRoomCount = branchPathRoomCount;
		MaxBranchDepth = maxBranchDepth;
		TotalRoomCount = MainPathRoomCount + BranchPathRoomCount;
	}

	internal void BeginTime(GenerationStatus status)
	{
		if (stopwatch.IsRunning)
		{
			EndTime();
		}
		generationStatus = status;
		stopwatch.Reset();
		stopwatch.Start();
	}

	internal void EndTime()
	{
		stopwatch.Stop();
		float num = (float)stopwatch.Elapsed.TotalMilliseconds;
		switch (generationStatus)
		{
		case GenerationStatus.PreProcessing:
			PreProcessTime += num;
			break;
		case GenerationStatus.MainPath:
			MainPathGenerationTime += num;
			break;
		case GenerationStatus.Branching:
			BranchPathGenerationTime += num;
			break;
		case GenerationStatus.PostProcessing:
			PostProcessTime += num;
			break;
		}
		TotalTime = PreProcessTime + MainPathGenerationTime + BranchPathGenerationTime + PostProcessTime;
	}

	public GenerationStats Clone()
	{
		return new GenerationStats
		{
			MainPathRoomCount = MainPathRoomCount,
			BranchPathRoomCount = BranchPathRoomCount,
			TotalRoomCount = TotalRoomCount,
			MaxBranchDepth = MaxBranchDepth,
			TotalRetries = TotalRetries,
			PreProcessTime = PreProcessTime,
			MainPathGenerationTime = MainPathGenerationTime,
			BranchPathGenerationTime = BranchPathGenerationTime,
			PostProcessTime = PostProcessTime,
			TotalTime = TotalTime
		};
	}
}
