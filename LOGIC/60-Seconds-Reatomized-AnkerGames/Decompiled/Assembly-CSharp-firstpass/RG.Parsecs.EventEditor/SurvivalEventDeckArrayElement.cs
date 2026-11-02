using System;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
public class SurvivalEventDeckArrayElement
{
	[SerializeField]
	[Tooltip("Reference to the Survival Event that will be held inside this deck.")]
	private SurvivalEvent _survivalEvent;

	[Range(0f, 1f)]
	[SerializeField]
	[Tooltip("Probability of the event to be chosen from the events inside this deck.")]
	private float _probability;

	[SerializeField]
	[Tooltip("Should this event be locked for editing inside this deck?")]
	private bool _locked;

	public SurvivalEvent SurvivalEvent => _survivalEvent;

	public float Probability => _probability;
}
