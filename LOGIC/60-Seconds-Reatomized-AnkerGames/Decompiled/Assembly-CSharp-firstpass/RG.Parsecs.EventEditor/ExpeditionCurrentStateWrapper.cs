using System;

namespace RG.Parsecs.EventEditor;

[Serializable]
internal struct ExpeditionCurrentStateWrapper
{
	public EExpeditionStatus CurrentState;

	public int ExpeditionLength;

	public EParsecsEventPhase CurrentPhase;

	public bool WasEventExecuted;

	public bool WasEventSuccessful;
}
