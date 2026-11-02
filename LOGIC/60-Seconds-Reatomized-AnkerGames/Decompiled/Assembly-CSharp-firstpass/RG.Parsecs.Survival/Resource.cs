using I2.Loc;
using RG.Core.Base;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(fileName = "New Resource", menuName = "60 Parsecs!/Crafting/New resource")]
public class Resource : RGScriptableObject
{
	[SerializeField]
	private string _id;

	public LocalizedString Name;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private LocalizedString _iconTerm;

	public string ID => _id;

	public Sprite Icon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_icon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _icon;
		}
	}

	public LocalizedString IconTerm => _iconTerm;

	public bool Equals(Resource res)
	{
		if (res == null)
		{
			return false;
		}
		return _id.Equals(res._id);
	}
}
