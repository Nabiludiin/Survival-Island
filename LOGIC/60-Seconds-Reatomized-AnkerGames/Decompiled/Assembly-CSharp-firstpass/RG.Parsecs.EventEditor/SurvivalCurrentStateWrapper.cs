using System;

namespace RG.Parsecs.EventEditor;

[Serializable]
internal struct SurvivalCurrentStateWrapper
{
	public bool WasEventExecute;

	public EEventState CurrentState;

	public EParsecsEventPhase CurrentPhase;

	public bool WasEventSuccessful;
}
