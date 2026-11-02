using System;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct NeedWrapper
{
	public CharacterNeedParameters.ECharacterNeedStatus NeedStatus;

	public float Value;
}
