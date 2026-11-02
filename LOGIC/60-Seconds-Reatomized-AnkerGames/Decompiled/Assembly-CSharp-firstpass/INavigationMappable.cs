using System.Collections.Generic;
using UnityEngine.UI;

public interface INavigationMappable
{
	InputMappingTag MappingTag { get; }

	void AddNavigationMapping(List<Selectable> down, List<Selectable> left, List<Selectable> up, List<Selectable> right);
}
