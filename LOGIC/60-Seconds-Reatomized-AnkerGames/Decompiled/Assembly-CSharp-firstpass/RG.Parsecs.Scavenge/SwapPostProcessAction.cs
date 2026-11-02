using Cinemachine.PostFX;
using UnityEngine.PostProcessing;

namespace RG.Parsecs.Scavenge;

public class SwapPostProcessAction : EventAction
{
	public CinemachinePostFX CinemachinePostFX;

	public PostProcessingBehaviour PostProcessingBehaviour;

	public PostProcessingProfile ToProfile;

	protected override void Act(EventActionArgs args = null)
	{
		CinemachinePostFX.m_Profile = ToProfile;
		PostProcessingBehaviour.profile = ToProfile;
	}
}
