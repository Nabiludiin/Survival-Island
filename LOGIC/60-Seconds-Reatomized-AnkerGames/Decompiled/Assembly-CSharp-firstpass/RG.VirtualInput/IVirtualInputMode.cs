using UnityEngine;

namespace RG.VirtualInput;

public interface IVirtualInputMode
{
	void OnStartMode();

	void OnFinishMode();

	void Process();

	Vector3 GetMousePosition();

	bool IsPointerOverGameObject();

	bool ShouldProcess();
}
