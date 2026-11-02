using UnityEngine;
using UnityEngine.Serialization;

public class MainMenu_PaintBlock : MonoBehaviour
{
	[FormerlySerializedAs("color")]
	[SerializeField]
	private SO_ColorValue colorA;

	[SerializeField]
	private SO_ColorValue colorB;

	[SerializeField]
	private SO_ColorValue patternColorA;

	[SerializeField]
	private SO_ColorValue patternColorB;

	[SerializeField]
	private SO_Pattern pattern1;

	[SerializeField]
	private SO_Pattern pattern2;

	private void Start()
	{
		Block component = GetComponent<Block>();
		if (component != null)
		{
			if (colorA != null || patternColorA != null)
			{
				component.SetInstanceColorAndPattern(colorA, patternColorA, 1, (!(pattern1 == null)) ? pattern1.uniquePatternIndex : 0u);
			}
			if (colorB != null || patternColorB != null)
			{
				component.SetInstanceColorAndPattern(colorB, patternColorB, 2, (!(pattern2 == null)) ? pattern2.uniquePatternIndex : 0u);
			}
		}
	}
}
