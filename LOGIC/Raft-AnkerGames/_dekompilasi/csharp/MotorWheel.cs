using System;
using FMODUnity;
using Steamworks;
using UnityEngine;

[RequireComponent(typeof(RaycastInteractable))]
public class MotorWheel : MonoBehaviour_ID_Network
{
	public enum WeightStrength
	{
		StrongEnough,
		Weak,
		NotStrongEnough
	}

	public Action OnMotorDirty;

	[SerializeField]
	private int _motorStrenght = 10;

	[SerializeField]
	private int _extraMotorStrength = 5;

	[SerializeField]
	private float _raftSpeed = 0.1f;

	private bool _motorState;

	public MotorwheelFuelTank FuelTank;

	[SerializeField]
	private InteractionMotorWheelPowerButton button_EngineOnOff;

	[SerializeField]
	private BlockButton button_ChangeDirection;

	[SerializeField]
	private Transform motorWheel;

	public bool rotatesForward;

	public bool engineSwitchOn;

	[SerializeField]
	private float timePerFuel = 60f;

	[SerializeField]
	private float wheelRotationSpeed = 15f;

	[SerializeField]
	private float waterParticleTimer = 0.4f;

	[SerializeField]
	private float waterParticleInterval = 0.4f;

	[SerializeField]
	private Animator animator_engine;

	[SerializeField]
	private Animator animator_wheel;

	[Range(0f, 1f)]
	[SerializeField]
	private float jitterChance = 0.4f;

	[SerializeField]
	private ParticleSystem[] waterParticles;

	public EventReference er_lever;

	[SerializeField]
	protected StudioEventEmitterSustain eventEmitter_boiler;

	[SerializeField]
	protected StudioEventEmitterSustain eventEmitter_engine;

	[SerializeField]
	protected StudioEventEmitterSustain eventEmitter_waterWheel;

	private DisplayTextManager displayText;

	private Raft_Network network;

	private bool isDisplayingText;

	private bool hasBeenPlaced;

	private float currentRotationSpeed = 1f;

	public bool CanStartEngine
	{
		get
		{
			if (FuelTank.CurrentTankAmount > 0f)
			{
				return !MotorState;
			}
			return false;
		}
	}

	public int MotorStrength
	{
		get
		{
			if (!MotorState)
			{
				return 0;
			}
			return _motorStrenght;
		}
	}

	public int ExtraMotorStrength
	{
		get
		{
			if (!MotorState)
			{
				return 0;
			}
			return _extraMotorStrength;
		}
	}

	public float RaftSpeed
	{
		get
		{
			if (!MotorState)
			{
				return 0f;
			}
			return _raftSpeed;
		}
	}

	public bool MotorState
	{
		get
		{
			bool motorState = _motorState;
			if (FuelTank.CurrentTankAmount <= 0f || !engineSwitchOn)
			{
				_motorState = false;
			}
			else if (engineSwitchOn)
			{
				_motorState = true;
			}
			if (motorState != _motorState)
			{
				SendOnMotorDirty();
			}
			return _motorState;
		}
		set
		{
			_motorState = value;
		}
	}

	public Vector3 PushDirection
	{
		get
		{
			if (!engineSwitchOn || !MotorState)
			{
				return Vector3.zero;
			}
			Vector3 forward = transform.forward;
			forward.y = 0f;
			if (rotatesForward)
			{
				return -forward;
			}
			return forward;
		}
	}

	protected void Awake()
	{
		if (Raft_Network.IsHost)
		{
			Tank_Gauge tankGauge = FuelTank.tankGauge;
			tankGauge.OnGaugeUpdateAction = (Action)Delegate.Combine(tankGauge.OnGaugeUpdateAction, new Action(SendUpdateGuageMeterNetworked));
		}
		if (button_EngineOnOff != null)
		{
			InteractionMotorWheelPowerButton interactionMotorWheelPowerButton = button_EngineOnOff;
			interactionMotorWheelPowerButton.OnButtonPressed = (Action)Delegate.Combine(interactionMotorWheelPowerButton.OnButtonPressed, new Action(OnButtonPressed_ToggleEngine));
			BlockButton blockButton = button_ChangeDirection;
			blockButton.OnButtonPressed = (Action)Delegate.Combine(blockButton.OnButtonPressed, new Action(OnButtonPressed_ChangeDirection));
		}
		currentRotationSpeed = wheelRotationSpeed;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced)
		{
			InteractionMotorWheelPowerButton interactionMotorWheelPowerButton = button_EngineOnOff;
			interactionMotorWheelPowerButton.OnButtonPressed = (Action)Delegate.Remove(interactionMotorWheelPowerButton.OnButtonPressed, new Action(OnButtonPressed_ToggleEngine));
			BlockButton blockButton = button_ChangeDirection;
			blockButton.OnButtonPressed = (Action)Delegate.Remove(blockButton.OnButtonPressed, new Action(OnButtonPressed_ChangeDirection));
			NetworkIDManager.RemoveNetworkID(this);
			RaftVelocityManager.RemoveMotor(this);
		}
	}

	public void Update()
	{
		if (hasBeenPlaced && MotorState)
		{
			if (RaftVelocityManager.MotorWheelWeightStrength != WeightStrength.NotStrongEnough)
			{
				FuelTank.ModifyTank(null, (0f - Time.deltaTime) / timePerFuel);
			}
			RotateWheel();
			SimulateWaterInteraction();
			HandleWeightStrengthAnimation();
		}
	}

	private void OnBlockPlaced()
	{
		network = ComponentManager<Raft_Network>.Value;
		displayText = ComponentManager<DisplayTextManager>.Value;
		hasBeenPlaced = true;
		InitializeFuel();
		NetworkIDManager.AddNetworkID(this);
		RaftVelocityManager.AddMotor(this);
	}

	public void OnButtonPressed_ToggleEngine()
	{
		Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.MotorWheel_PowerButton, network.NetworkIDManager, ObjectIndex);
		if (Raft_Network.IsHost)
		{
			ToggleEngine();
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void OnButtonPressed_ChangeDirection()
	{
		Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.MotorWheel_Inverse, network.NetworkIDManager, ObjectIndex);
		if (Raft_Network.IsHost)
		{
			InversePushDirection();
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void SendUpdateGuageMeterNetworked()
	{
		FuelTank.SendTankSetValueMessage(network.GetLocalPlayer());
	}

	private void InitializeFuel()
	{
	}

	private void RotateWheel()
	{
		if (motorWheel == null)
		{
			Debug.Log("Motor wheel is null, add to inspector");
			return;
		}
		bool flag = false;
		float num = 1f - RaftVelocityManager.MotorWheelWeightStrengthNormalized;
		if (num == 0f && UnityEngine.Random.value > 1f - jitterChance)
		{
			flag = true;
		}
		float num2 = 0f;
		num2 = ((num != 0f) ? (wheelRotationSpeed * num * (float)((!rotatesForward) ? 1 : (-1))) : (wheelRotationSpeed * 0.25f * (float)((!rotatesForward) ? 1 : (-1))));
		currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, num2, Time.deltaTime * 2f);
		float target = 0f;
		if (num == 1f)
		{
			target = 3f;
		}
		else if (num > 0f)
		{
			target = 1f;
		}
		if ((num2 > 0f && currentRotationSpeed < 0f) || (num2 < 0f && currentRotationSpeed > 0f))
		{
			target = 0f;
		}
		float value = 0f;
		eventEmitter_engine.eventEmitter.EventInstance.getParameterByName("RPM", out value);
		value = Mathf.MoveTowards(value, target, Time.deltaTime * 4f);
		eventEmitter_engine.eventEmitter.SetParameter("RPM", value);
		eventEmitter_waterWheel.eventEmitter.SetParameter("RPM", value);
		if (!flag)
		{
			motorWheel.Rotate(Vector3.forward * currentRotationSpeed * Time.deltaTime);
		}
	}

	private void SimulateWaterInteraction()
	{
		waterParticleTimer += Time.deltaTime;
		WeightStrength motorWheelWeightStrength = RaftVelocityManager.MotorWheelWeightStrength;
		if (motorWheelWeightStrength != WeightStrength.Weak)
		{
			_ = waterParticleInterval;
		}
		else
		{
			_ = waterParticleInterval;
		}
		if (motorWheelWeightStrength != WeightStrength.NotStrongEnough && waterParticleTimer > waterParticleInterval)
		{
			waterParticleTimer = 0f;
			for (int i = 0; i < waterParticles.Length; i++)
			{
				waterParticles[i].Play();
			}
		}
	}

	private void LocalOnMotorDirty()
	{
		HandleSounds();
		if (Raft_Network.IsHost)
		{
			FuelTank.SendTankSetValueMessage(null);
		}
		if (animator_engine != null)
		{
			animator_engine.SetBool("On", _motorState);
		}
		if (animator_wheel != null)
		{
			animator_wheel.SetBool("On", _motorState);
		}
	}

	private void HandleWeightStrengthAnimation()
	{
		if (animator_engine != null)
		{
			animator_engine.SetFloat("HeavyAmount", 2f * RaftVelocityManager.MotorWheelWeightStrengthNormalized);
		}
	}

	private void HandleSounds()
	{
		if (_motorState)
		{
			eventEmitter_engine.eventEmitter.Stop();
			eventEmitter_waterWheel.eventEmitter.Stop();
			eventEmitter_boiler.eventEmitter.Stop();
			eventEmitter_engine.eventEmitter.Play();
			eventEmitter_waterWheel.eventEmitter.Play();
			eventEmitter_boiler.eventEmitter.Play();
		}
		else
		{
			eventEmitter_engine.eventEmitter.EventInstance.keyOff();
			eventEmitter_boiler.eventEmitter.EventInstance.keyOff();
			eventEmitter_waterWheel.eventEmitter.EventInstance.keyOff();
		}
	}

	public void RestoreMotor(RGD_MotorWheel rgd_motor)
	{
		rotatesForward = rgd_motor.isRotatingForward;
		engineSwitchOn = rgd_motor.isEngineTurnedOn;
		if (rgd_motor.rgdTank != null)
		{
			rgd_motor.rgdTank.RestoreTank(FuelTank);
		}
		if (animator_engine != null)
		{
			animator_engine.SetBool("On", _motorState);
			animator_engine.SetBool("Lever", engineSwitchOn);
		}
		if (animator_wheel != null)
		{
			animator_wheel.SetBool("On", _motorState);
			animator_wheel.SetBool("Lever", !rotatesForward);
		}
	}

	public void SendOnMotorDirty()
	{
		OnMotorDirty?.Invoke();
		LocalOnMotorDirty();
	}

	public void ToggleEngine()
	{
		engineSwitchOn = !engineSwitchOn;
		if (animator_engine != null)
		{
			animator_engine.SetBool("Lever", engineSwitchOn);
		}
		RuntimeManager.PlayOneShot(er_lever, button_EngineOnOff.transform.position);
	}

	public void InversePushDirection()
	{
		rotatesForward = !rotatesForward;
		if (Raft_Network.IsHost)
		{
			SendOnMotorDirty();
		}
		if (animator_wheel != null)
		{
			animator_wheel.SetBool("Lever", !rotatesForward);
		}
		RuntimeManager.PlayOneShot(er_lever, button_ChangeDirection.transform.position);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_MotorWheel(RGDType.Block_Motor_Wheel, this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.MotorWheel_Inverse:
			InversePushDirection();
			return true;
		case Messages.MotorWheel_PowerButton:
			ToggleEngine();
			return true;
		case Messages.Tank:
			return FuelTank.OnTankModifyMessage(msg as Message_Tank);
		case Messages.Tank_SetValue:
			FuelTank.OnTankSetValueMessage(msg as Message_Tank);
			return false;
		default:
			return false;
		}
	}
}
