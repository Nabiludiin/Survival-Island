public class MonoBehaviour_Network : MonoBehaviour_ID_Network
{
	private uint behaviourIndex;

	private uint currentSendIndex;

	private uint lastSentIndex;

	private bool dead;

	public bool IsDirty => currentSendIndex != lastSentIndex;

	public bool IsKilled => dead;

	public uint BehaviourIndex
	{
		get
		{
			return behaviourIndex;
		}
		set
		{
			behaviourIndex = value;
		}
	}

	public void SetDirty()
	{
		currentSendIndex = lastSentIndex + 1;
	}

	public override void Clean()
	{
		base.Clean();
		lastSentIndex = currentSendIndex;
	}

	public void Kill()
	{
		dead = true;
	}

	public void Revive()
	{
		dead = false;
		Clean();
	}

	public virtual void Serialize_Update()
	{
	}
}
