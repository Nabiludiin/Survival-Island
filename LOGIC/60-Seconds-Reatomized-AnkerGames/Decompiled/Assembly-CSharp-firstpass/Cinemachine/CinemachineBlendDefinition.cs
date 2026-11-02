using System;
using UnityEngine;

namespace Cinemachine;

[Serializable]
[DocumentationSorting(10.2f, DocumentationSortingAttribute.Level.UserRef)]
public struct CinemachineBlendDefinition(CinemachineBlendDefinition.Style style, float time)
{
	[DocumentationSorting(10.21f, DocumentationSortingAttribute.Level.UserRef)]
	public enum Style
	{
		Cut,
		EaseInOut,
		EaseIn,
		EaseOut,
		HardIn,
		HardOut,
		Linear
	}

	[Tooltip("Shape of the blend curve")]
	public Style m_Style = style;

	[Tooltip("Duration of the blend, in seconds")]
	public float m_Time = time;

	public AnimationCurve BlendCurve
	{
		get
		{
			float timeEnd = Mathf.Max(0f, m_Time);
			switch (m_Style)
			{
			default:
				return new AnimationCurve();
			case Style.EaseInOut:
				return AnimationCurve.EaseInOut(0f, 0f, timeEnd, 1f);
			case Style.EaseIn:
			{
				AnimationCurve animationCurve4 = AnimationCurve.Linear(0f, 0f, timeEnd, 1f);
				Keyframe[] keys4 = animationCurve4.keys;
				keys4[1].inTangent = 0f;
				animationCurve4.keys = keys4;
				return animationCurve4;
			}
			case Style.EaseOut:
			{
				AnimationCurve animationCurve3 = AnimationCurve.Linear(0f, 0f, timeEnd, 1f);
				Keyframe[] keys3 = animationCurve3.keys;
				keys3[0].outTangent = 0f;
				animationCurve3.keys = keys3;
				return animationCurve3;
			}
			case Style.HardIn:
			{
				AnimationCurve animationCurve2 = AnimationCurve.Linear(0f, 0f, timeEnd, 1f);
				Keyframe[] keys2 = animationCurve2.keys;
				keys2[0].outTangent = 0f;
				keys2[1].inTangent = 1.5708f;
				animationCurve2.keys = keys2;
				return animationCurve2;
			}
			case Style.HardOut:
			{
				AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, timeEnd, 1f);
				Keyframe[] keys = animationCurve.keys;
				keys[0].outTangent = 1.5708f;
				keys[1].inTangent = 0f;
				animationCurve.keys = keys;
				return animationCurve;
			}
			case Style.Linear:
				return AnimationCurve.Linear(0f, 0f, timeEnd, 1f);
			}
		}
	}
}
