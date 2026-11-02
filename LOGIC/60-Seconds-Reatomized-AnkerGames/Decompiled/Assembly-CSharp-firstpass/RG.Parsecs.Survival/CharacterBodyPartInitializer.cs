using UnityEngine;

namespace RG.Parsecs.Survival;

public class CharacterBodyPartInitializer : MonoBehaviour
{
	[SerializeField]
	private CharacterBodyPart _data;

	private void Awake()
	{
		_data.Handle = gameObject;
		_data.BodyPartName = gameObject.name;
		CharacterBody characterBody = null;
		Transform parent = transform;
		while (characterBody == null && parent != null)
		{
			characterBody = parent.GetComponent<CharacterBody>();
			if (characterBody == null)
			{
				parent = parent.parent;
			}
		}
		if (characterBody != null)
		{
			characterBody.AddBodyPart(_data);
		}
		_data.Handle.SetActive(value: false);
		Object.Destroy(this);
	}
}
