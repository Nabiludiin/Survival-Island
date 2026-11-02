using UnityEngine;

public class BedComponent : MonoBehaviour
{
	private CanvasHelper canvas;

	private Network_Player playerNetwork;

	private Bed currentBed;

	private bool aimingAtBed;

	private bool sleeping;

	private bool canExit;

	private float exitDelay = 0.1f;

	public Bed CurrentBed
	{
		get
		{
			return currentBed;
		}
		set
		{
			currentBed = value;
		}
	}

	public bool Sleeping
	{
		get
		{
			return sleeping;
		}
		set
		{
			sleeping = value;
		}
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void Update()
	{
		if (Sleeping && currentBed != null && playerNetwork.IsLocalPlayer && canExit && !ChatTextFieldController.IsChatWindowSelected)
		{
			currentBed.HandleAsleep();
		}
	}

	public bool Sleep(Bed bed)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (bed != null && !bed.Busy)
		{
			currentBed = bed;
			currentBed.AttachPlayer(playerNetwork);
			canExit = false;
			Invoke("SetCanExit", exitDelay);
			return true;
		}
		return false;
	}

	public bool StopSleep()
	{
		if (currentBed != null)
		{
			currentBed.DettachPlayer();
			return true;
		}
		return false;
	}

	private void SetCanExit()
	{
		canExit = true;
	}
}
