using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "60 Parsecs!/Font Materials For Localization")]
public class FontMaterialsForLocalization : ScriptableObject
{
	[SerializeField]
	private List<TMP_FontAsset> _originalFont;

	[SerializeField]
	private List<Material> _fontMaterial;

	public List<TMP_FontAsset> OriginalFont => _originalFont;

	public List<Material> FontMaterial => _fontMaterial;
}
