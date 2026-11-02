using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public sealed class WaterShadowCastingLight : MonoBehaviour
{
	private CommandBuffer _CommandBuffer;

	private void Start()
	{
		int waterShadowmap = ShaderVariables.WaterShadowmap;
		_CommandBuffer = new CommandBuffer
		{
			name = "Water: Copy Shadowmap"
		};
		_CommandBuffer.GetTemporaryRT(waterShadowmap, Screen.width, Screen.height, 32, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		_CommandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, waterShadowmap);
		GetComponent<Light>().AddCommandBuffer(LightEvent.AfterScreenspaceMask, _CommandBuffer);
	}
}
