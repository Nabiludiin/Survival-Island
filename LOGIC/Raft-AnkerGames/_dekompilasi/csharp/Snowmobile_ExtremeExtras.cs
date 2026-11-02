using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snowmobile_ExtremeExtras : MonoBehaviour
{
	public Snowmobile snowMobile;

	public Animator animator;

	public Rigidbody rigid;

	public float jumpForce = 300f;

	public VelocityCalculator velocityCalculator;

	[SerializeField]
	public StudioEventEmitter eventRef_footStep;

	[SerializeField]
	public StudioEventEmitter eventRef_jump;

	[SerializeField]
	public StudioEventEmitter eventRef_protect;

	public float stepCD = 0.2f;

	private Raft_Network network;

	private bool jumping;

	private float timeSinceGrounded;

	private bool isProtecting;

	private bool saidHello;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	public void Update()
	{
		animator.SetFloat("Speed", velocityCalculator.velocityMagnitude);
		animator.SetFloat("VelocityModifier", velocityCalculator.NormalizedVelocity);
		if (!(snowMobile.DrivingPlayer != null) || !(snowMobile.DrivingPlayer == network.GetLocalPlayer()))
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Jump"];
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Jump") && !jumping && snowMobile.IsGrounded)
		{
			jumping = true;
			animator.SetTrigger("Attack_Jump");
			Invoke("Jump", 1.1f);
		}
		if (snowMobile.IsGrounded)
		{
			timeSinceGrounded = 0f;
			animator.SetBool("IsProtecting", value: false);
			isProtecting = false;
			return;
		}
		timeSinceGrounded += Time.deltaTime;
		if (!isProtecting && timeSinceGrounded > 0.8f)
		{
			isProtecting = true;
			animator.SetBool("IsProtecting", value: true);
			if (eventRef_protect != null)
			{
				eventRef_protect.Play();
			}
		}
	}

	private void Jump()
	{
		jumping = false;
		rigid.AddForce((transform.forward + transform.up).normalized * jumpForce + Vector3.up * jumpForce / 2f, ForceMode.Impulse);
	}

	public void Animation_IdleSound()
	{
	}

	public void AnimationString(string stuff)
	{
	}

	public void PlaySound()
	{
		eventRef_jump.PlaySafe();
	}

	public void Animation_Footstep()
	{
		if (eventRef_footStep != null && !(snowMobile == null) && snowMobile.IsGrounded)
		{
			eventRef_footStep.Play();
			eventRef_footStep.SetParameter("material", 1f);
			eventRef_footStep.SetParameter("speed", 1f);
		}
	}
}
