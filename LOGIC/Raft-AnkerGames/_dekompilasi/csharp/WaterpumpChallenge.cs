using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(RaycastInteractable))]
public class WaterpumpChallenge : MonoBehaviour, IRaycastable
{
	[SerializeField]
	private MeshPathBase_Electricity meshPathElectricity;

	[SerializeField]
	private GameObject pumpedParticleEmitter;

	[SerializeField]
	private Renderer pumpPipeRenderer;

	[SerializeField]
	private float vertexOffsetMultiplier = 0.05f;

	[SerializeField]
	private string localizationTerm = string.Empty;

	[SerializeField]
	private VFX_Electricity electricityVFX;

	[SerializeField]
	private Animator generatorAnimator;

	[SerializeField]
	private StudioEventEmitter generatorEmitter;

	[SerializeField]
	private List<Transform> electricityPathPoints = new List<Transform>();

	[SerializeField]
	private StudioEventEmitter eventEmitter_PumpWater;

	private DisplayTextManager displayTextManager;

	private MaterialPropertyBlock mpb;

	private bool active;

	public bool Active => active;

	private void Start()
	{
		if (electricityVFX != null)
		{
			List<Vector3> list = new List<Vector3>();
			foreach (Transform electricityPathPoint in electricityPathPoints)
			{
				list.Add(electricityPathPoint.position);
			}
			electricityVFX.Init(list);
			electricityVFX.SetActive(value: false);
		}
		CheckElectricity();
	}

	private void Update()
	{
		if (meshPathElectricity == null)
		{
			Debug.Log("Meshpath base is null, assign it", gameObject);
		}
		else if (active != meshPathElectricity.HasElectricity)
		{
			CheckElectricity();
		}
	}

	public void OnIsRayed()
	{
		if (!active)
		{
			string term = Helper.GetTerm(localizationTerm);
			if (term.IsNullOrEmpty())
			{
				term = localizationTerm;
			}
			displayTextManager.ShowText(term);
		}
	}

	public void OnRayEnter()
	{
		if (displayTextManager == null)
		{
			displayTextManager = ComponentManager<DisplayTextManager>.Value;
		}
	}

	public void OnRayExit()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	private void CheckElectricity()
	{
		active = meshPathElectricity.HasElectricity;
		ToggleMisc(active);
	}

	private void ToggleMisc(bool value)
	{
		if (pumpedParticleEmitter != null)
		{
			pumpedParticleEmitter.gameObject.SetActiveSafe(value);
		}
		if (mpb == null)
		{
			mpb = new MaterialPropertyBlock();
		}
		pumpPipeRenderer.GetPropertyBlock(mpb, 1);
		mpb.SetFloat("_VertexOffsetMultiplier", value ? vertexOffsetMultiplier : 0f);
		pumpPipeRenderer.SetPropertyBlock(mpb, 1);
		if (electricityVFX != null)
		{
			electricityVFX.SetActive(value);
		}
		if (value)
		{
			eventEmitter_PumpWater.PlaySafe();
			generatorEmitter.PlaySafe();
		}
		else
		{
			eventEmitter_PumpWater.StopSafe();
			generatorEmitter.StopSafe();
		}
		generatorAnimator.SetBool("On", value);
	}
}
