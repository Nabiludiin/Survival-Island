using UltimateWater.Internal;
using UnityEngine.Rendering;

namespace UltimateWater;

public interface ILocalDiffuseRenderer : IDynamicWaterEffects
{
	bool IsVisible(WaterCamera camera);

	void RenderLocalDiffuse(CommandBuffer commandBuffer, DynamicWaterCameraData overlays);
}
