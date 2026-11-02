using System.Collections.Generic;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ReferenceSOManager : MonoBehaviour
{
	[SerializeField]
	private List<RGScriptableObject> _referenceList = new List<RGScriptableObject>();
}
