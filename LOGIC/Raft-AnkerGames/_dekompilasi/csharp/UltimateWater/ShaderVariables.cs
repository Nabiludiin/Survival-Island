using UnityEngine;

namespace UltimateWater;

public class ShaderVariables
{
	private static int _LocalDisplacementMap = -1;

	private static int _LocalNormalMap = -1;

	private static int _LocalDiffuseMap = -1;

	private static int _LocalMapsCoords = -1;

	private static int _LocalMapsCoordsPrevious = -1;

	private static int _UnderwaterMask = -1;

	private static int _UnderwaterMask2 = -1;

	private static int _AdditiveMask = -1;

	private static int _SubtractiveMask = -1;

	private static int _DisplacementsMask = -1;

	private static int _Gbuffer0 = -1;

	private static int _Gbuffer1 = -1;

	private static int _Gbuffer2 = -1;

	private static int _Gbuffer3 = -1;

	private static int _OcclusionMap = -1;

	private static int _OcclusionMap2 = -1;

	private static int _OcclusionMapProjection = -1;

	private static int _FoamParameters = -1;

	private static int _FoamShoreIntensity = -1;

	private static int _FoamIntensity = -1;

	private static int _WaterTileSizeInvSrt = -1;

	private static int _WaterTileSize = -1;

	private static int _WaterTileSizeInv = -1;

	private static int _WaterTileSizeScales = -1;

	private static int _MaxDisplacement = -1;

	private static int _TotalDisplacementMap = -1;

	private static int _DisplacementMask = -1;

	private static int _WaterDepthTexture = -1;

	private static int _UnityMatrixVPInverse = -1;

	private static int _DepthClipMultiplier = -1;

	private static int _RefractTex = -1;

	private static int _WaterlessDepthTexture = -1;

	private static int _WaterShadowmap = -1;

	private static int _Velocity = -1;

	private static int _CameraDepthTexture2 = -1;

	private static int _PlanarReflectionTex = -1;

	private static int _ButterflyTex = -1;

	private static int _ButterflyPass = -1;

	private static int _Offset = -1;

	private static int _MaxDistance = -1;

	private static int _AbsorptionColorPerPixel = -1;

	private static int _SurfaceOffset = -1;

	private static int _WaterId = -1;

	private static int _Cull = -1;

	private static int _Period = -1;

	private static int _BumpMapST = -1;

	private static int _DetailAlbedoMapST = -1;

	private static int _HeightTex = -1;

	private static int _DisplacementTex = -1;

	private static int _HorizontalDisplacementScale = -1;

	private static int _JacobianScale = -1;

	private static int _RenderTime = -1;

	private static int[] _DisplacementDeltaMaps;

	public static int LocalDisplacementMap => Cache(ref _LocalDisplacementMap, "_LocalDisplacementMap");

	public static int LocalNormalMap => Cache(ref _LocalNormalMap, "_LocalNormalMap");

	public static int LocalDiffuseMap => Cache(ref _LocalDiffuseMap, "_LocalDiffuseMap");

	public static int LocalMapsCoords => Cache(ref _LocalMapsCoords, "_LocalMapsCoords");

	public static int LocalMapsCoordsPrevious => Cache(ref _LocalMapsCoordsPrevious, "_LocalMapsCoordsPrevious");

	public static int UnderwaterMask => Cache(ref _UnderwaterMask, "_UnderwaterMask");

	public static int UnderwaterMask2 => Cache(ref _UnderwaterMask2, "_UnderwaterMask2");

	public static int AdditiveMask => Cache(ref _AdditiveMask, "_AdditiveMask");

	public static int SubtractiveMask => Cache(ref _SubtractiveMask, "_SubtractiveMask");

	public static int DisplacementsMask => Cache(ref _DisplacementsMask, "_DisplacementsMask");

	public static int Gbuffer0 => Cache(ref _Gbuffer0, "_CameraGBufferTextureOriginal0");

	public static int Gbuffer1 => Cache(ref _Gbuffer1, "_CameraGBufferTextureOriginal1");

	public static int Gbuffer2 => Cache(ref _Gbuffer2, "_CameraGBufferTextureOriginal2");

	public static int Gbuffer3 => Cache(ref _Gbuffer3, "_CameraGBufferTextureOriginal3");

	public static int OcclusionMap => Cache(ref _OcclusionMap, "_OcclusionMap");

	public static int OcclusionMap2 => Cache(ref _OcclusionMap2, "_OcclusionMap2");

	public static int OcclusionMapProjection => Cache(ref _OcclusionMapProjection, "_OcclusionMapProjection");

	public static int FoamParameters => Cache(ref _FoamParameters, "_FoamParameters");

	public static int FoamShoreIntensity => Cache(ref _FoamShoreIntensity, "_FoamShoreIntensity");

	public static int FoamIntensity => Cache(ref _FoamIntensity, "_FoamIntensity");

	public static int WaterTileSizeInvSrt => Cache(ref _WaterTileSizeInvSrt, "_WaterTileSizeInvSRT");

	public static int[] DisplacementDeltaMaps
	{
		get
		{
			if (_DisplacementDeltaMaps == null)
			{
				_DisplacementDeltaMaps = new int[4] { -1, -1, -1, -1 };
				Cache(ref _DisplacementDeltaMaps[0], "_DisplacementDeltaMap");
				Cache(ref _DisplacementDeltaMaps[1], "_DisplacementDeltaMap1");
				Cache(ref _DisplacementDeltaMaps[2], "_DisplacementDeltaMap2");
				Cache(ref _DisplacementDeltaMaps[3], "_DisplacementDeltaMap3");
			}
			return _DisplacementDeltaMaps;
		}
	}

	public static int WaterTileSize => Cache(ref _WaterTileSize, "_WaterTileSize");

	public static int WaterTileSizeInv => Cache(ref _WaterTileSizeInv, "_WaterTileSizeInv");

	public static int WaterTileSizeScales => Cache(ref _WaterTileSizeScales, "_WaterTileSizeScales");

	public static int MaxDisplacement => Cache(ref _MaxDisplacement, "_MaxDisplacement");

	public static int TotalDisplacementMap => Cache(ref _TotalDisplacementMap, "_TotalDisplacementMap");

	public static int DisplacementMask => Cache(ref _DisplacementMask, "_DisplacementsMask");

	public static int WaterDepthTexture => Cache(ref _WaterDepthTexture, "_WaterDepthTexture");

	public static int UnityMatrixVPInverse => Cache(ref _UnityMatrixVPInverse, "UNITY_MATRIX_VP_INVERSE");

	public static int DepthClipMultiplier => Cache(ref _DepthClipMultiplier, "_DepthClipMultiplier");

	public static int RefractTex => Cache(ref _RefractTex, "_RefractionTex");

	public static int WaterlessDepthTexture => Cache(ref _WaterlessDepthTexture, "_WaterlessDepthTexture");

	public static int CameraDepthTexture2 => Cache(ref _CameraDepthTexture2, "_CameraDepthTexture2");

	public static int Velocity => Cache(ref _Velocity, "Velocity");

	public static int WaterShadowmap => Cache(ref _WaterShadowmap, "_WaterShadowmap");

	public static int PlanarReflectionTex => Cache(ref _PlanarReflectionTex, "_PlanarReflectionTex");

	public static int ButterflyTex => Cache(ref _ButterflyTex, "_ButterflyTex");

	public static int ButterflyPass => Cache(ref _ButterflyPass, "_ButterflyPass");

	public static int Offset => Cache(ref _Offset, "_Offset");

	public static int MaxDistance => Cache(ref _MaxDistance, "_MaxDistance");

	public static int AbsorptionColorPerPixel => Cache(ref _AbsorptionColorPerPixel, "_AbsorptionColorPerPixel");

	public static int SurfaceOffset => Cache(ref _SurfaceOffset, "_SurfaceOffset");

	public static int WaterId => Cache(ref _WaterId, "_WaterId");

	public static int Cull => Cache(ref _Cull, "_Cull");

	public static int Period => Cache(ref _Period, "_Period");

	public static int BumpMapST => Cache(ref _BumpMapST, "_BumpMap_ST");

	public static int DetailAlbedoMapST => Cache(ref _DetailAlbedoMapST, "_DetailAlbedoMap_ST");

	public static int HeightTex => Cache(ref _HeightTex, "_HeightTex");

	public static int DisplacementTex => Cache(ref _DisplacementTex, "_DisplacementTex");

	public static int HorizontalDisplacementScale => Cache(ref _HorizontalDisplacementScale, "_HorizontalDisplacementScale");

	public static int JacobianScale => Cache(ref _JacobianScale, "_JacobianScale");

	public static int RenderTime => Cache(ref _RenderTime, "_RenderTime");

	private static int Cache(ref int value, string name)
	{
		if (value != -1)
		{
			return value;
		}
		value = Shader.PropertyToID(name);
		return value;
	}
}
