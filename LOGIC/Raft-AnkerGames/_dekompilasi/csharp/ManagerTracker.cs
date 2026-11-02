public class ManagerTracker<T>
{
	public float trackerTriggerRate;

	public float currentTrackerProgress;

	public ManagerTracker(float updateRate)
	{
		trackerTriggerRate = updateRate;
	}

	public void UpdateTracker(float deltaTime)
	{
		currentTrackerProgress += deltaTime;
		if (currentTrackerProgress >= trackerTriggerRate)
		{
			OnTrackerTrigger();
		}
	}

	protected virtual void OnTrackerTrigger()
	{
		currentTrackerProgress = 0f;
	}
}
