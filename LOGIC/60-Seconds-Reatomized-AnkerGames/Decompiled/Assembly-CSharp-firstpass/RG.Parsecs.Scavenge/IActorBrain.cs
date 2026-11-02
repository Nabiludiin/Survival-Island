using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public interface IActorBrain
{
	Transform Transform { get; }

	GameObject GameObject { get; }

	string Name { get; }

	Transform PositionMarker { get; }

	Player Player { get; }
}
