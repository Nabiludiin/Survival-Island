using Steamworks;
using UnityEngine;

public class Reciever_Antenna : MonoBehaviour, IRaycastable
{
	public bool toCloseToOtherAntenna;

	public bool toCloseToReciever;

	public bool toFarAwayFromReciever;

	public bool onSameAltitudeAsReciever;

	public int socketNumber;

	[SerializeField]
	private Rope wire;

	[SerializeField]
	private GameObject light_Off;

	[SerializeField]
	private GameObject light_Green;

	[SerializeField]
	private GameObject light_Red;

	private CanvasHelper canvas;

	private Block blockScript;

	private Reciever reciever;

	private bool hasBeenPlaced;

	private bool showingText;

	public bool ConnectedToReciever => reciever != null;

	public bool HasValidConnection
	{
		get
		{
			if (!toCloseToOtherAntenna && !toCloseToReciever && !toFarAwayFromReciever && onSameAltitudeAsReciever)
			{
				return ConnectedToReciever;
			}
			return false;
		}
	}

	private void Awake()
	{
		wire.gameObject.SetActive(value: false);
	}

	public void OnBlockPlaced()
	{
		blockScript = GetComponent<Block>();
		hasBeenPlaced = true;
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	private void Update()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		if (!ConnectedToReciever)
		{
			SetLight(AntennaLightState.Off);
			Reciever reciever = null;
			float num = float.MaxValue;
			for (int i = 0; i < Reciever.AllRecievers.Count; i++)
			{
				Reciever reciever2 = Reciever.AllRecievers[i];
				if (!(reciever2.network != null) || !Raft_Network.IsHost)
				{
					return;
				}
				if (!reciever2.HasMaxAntennas())
				{
					float num2 = Vector3.Distance(transform.position, reciever2.transform.position);
					if (num2 < num)
					{
						num = num2;
						reciever = reciever2;
					}
				}
			}
			if (!(reciever != null))
			{
				return;
			}
			Message_Reciever_Antenna message = new Message_Reciever_Antenna(Messages.Reciever_Antenna_Connect, reciever.network.NetworkIDManager, reciever.ObjectIndex, blockScript.ObjectIndex);
			if (Raft_Network.IsHost)
			{
				if (ConnectToReciever(reciever))
				{
					reciever.network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			else
			{
				reciever.network.SendP2P(reciever.network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else if (this.reciever.battery.On)
		{
			if (HasValidConnection)
			{
				SetLight(AntennaLightState.Green);
			}
			else
			{
				SetLight(AntennaLightState.Red);
			}
		}
		else
		{
			SetLight(AntennaLightState.Off);
		}
	}

	private void OnDestroy()
	{
		DisconnectFromReciever();
	}

	public bool ConnectToReciever(Reciever reciever)
	{
		if (ConnectedToReciever)
		{
			return false;
		}
		this.reciever = reciever;
		if (reciever.AddAntenna(this))
		{
			wire.gameObject.SetActive(value: true);
			wire.SetPosition(0, transform.position);
			wire.SetPosition(1, reciever.sockets[socketNumber].position);
			return true;
		}
		return false;
	}

	public void DisconnectFromReciever()
	{
		if (ConnectedToReciever)
		{
			reciever.RemoveAntenna(this);
			reciever = null;
			wire.gameObject.SetActive(value: false);
		}
	}

	public void OnIsRayed()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			if (showingText)
			{
				canvas.displayTextManager.HideDisplayTexts();
			}
		}
		else if (!ConnectedToReciever)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Radar/AntennaRequiresReciever"));
			showingText = true;
		}
		else if (showingText)
		{
			canvas.displayTextManager.HideDisplayTexts();
			showingText = false;
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (hasBeenPlaced && showingText)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	private void SetLight(AntennaLightState state)
	{
		if (light_Off.activeSelf != (state == AntennaLightState.Off))
		{
			light_Off.SetActive(!light_Off.activeSelf);
		}
		if (light_Green.activeSelf != (state == AntennaLightState.Green))
		{
			light_Green.SetActive(!light_Green.activeSelf);
		}
		if (light_Red.activeSelf != (state == AntennaLightState.Red))
		{
			light_Red.SetActive(!light_Red.activeSelf);
		}
	}
}
