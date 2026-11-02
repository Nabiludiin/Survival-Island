using System;
using System.Collections.Generic;
using System.Diagnostics;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public sealed class WaveParticlesQuadtree : Quadtree<WaveParticle>
{
	private Mesh _Mesh;

	private Vector3[] _Vertices;

	private Vector4[] _TangentsPack;

	private Vector3[] _DebugData;

	private WaveParticlesGroup[] _ParticleGroups;

	private int _NumParticleGroups;

	private int _LastGroupIndex = -1;

	private float _Stress = 1f;

	private bool _TangentsPackChanged;

	private Stopwatch _Stopwatch;

	private int _LastUpdateIndex;

	private bool _DebugMode;

	private WaveParticlesQuadtree _Qa;

	private WaveParticlesQuadtree _Qb;

	private WaveParticlesQuadtree _Qc;

	private WaveParticlesQuadtree _Qd;

	private readonly WaveParticlesQuadtree _Qroot;

	public bool DebugMode
	{
		get
		{
			return _DebugMode;
		}
		set
		{
			_DebugMode = value;
		}
	}

	public WaveParticlesQuadtree(Rect rect, int maxElementsPerNode, int maxTotalElements)
		: base(rect, maxElementsPerNode, maxTotalElements)
	{
		_Qroot = this;
		_ParticleGroups = new WaveParticlesGroup[maxElementsPerNode >> 3];
		CreateMesh();
	}

	public void UpdateSimulation(float time)
	{
		if (_Qa != null)
		{
			_Qa.UpdateSimulation(time);
			_Qb.UpdateSimulation(time);
			_Qc.UpdateSimulation(time);
			_Qd.UpdateSimulation(time);
		}
		else if (_NumElements != 0)
		{
			UpdateParticles(time);
		}
	}

	public void UpdateSimulation(float time, float maxExecutionTimeExp)
	{
		if (_Stopwatch == null)
		{
			_Stopwatch = new Stopwatch();
		}
		_Stopwatch.Reset();
		_Stopwatch.Start();
		UpdateSimulation(time);
		float num = (float)_Stopwatch.ElapsedTicks / 10000f;
		if (num > 50f)
		{
			num = 50f;
		}
		_Stress = _Stress * 0.98f + (Mathf.Exp(num) - maxExecutionTimeExp) * 0.04f;
		if (_Stress < 1f)
		{
			_Stress = 1f;
		}
		if (!(_Stress < 20f))
		{
			_Stress = 20f;
		}
	}

	public void Render(Rect renderRect)
	{
		if (!Rect.Overlaps(renderRect))
		{
			return;
		}
		if (_Qa != null)
		{
			_Qa.Render(renderRect);
			_Qb.Render(renderRect);
			_Qc.Render(renderRect);
			_Qd.Render(renderRect);
		}
		else if (_NumElements != 0)
		{
			_Mesh.vertices = _Vertices;
			if (_TangentsPackChanged)
			{
				_Mesh.tangents = _TangentsPack;
				_TangentsPackChanged = false;
			}
			if (_Qroot._DebugMode)
			{
				_Mesh.normals = _DebugData;
			}
			Graphics.DrawMeshNow(_Mesh, Matrix4x4.identity, 0);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (_Mesh != null)
		{
			_Mesh.Destroy();
			_Mesh = null;
		}
		_Vertices = null;
		_TangentsPack = null;
	}

	private WaveParticlesQuadtree(WaveParticlesQuadtree root, Rect rect, int maxElementsPerNode)
		: this(rect, maxElementsPerNode, 0)
	{
		_Qroot = root;
	}

	private void UpdateParticles(float time)
	{
		List<WaterCamera> enabledWaterCameras = WaterCamera.EnabledWaterCameras;
		int count = enabledWaterCameras.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			if (Rect.Overlaps(enabledWaterCameras[i].LocalMapsRect))
			{
				flag = true;
				break;
			}
		}
		int num;
		int num2;
		int num3;
		if (!flag)
		{
			num = _LastUpdateIndex;
			num2 = _LastUpdateIndex + 8;
			num3 = num << 2;
			if (num2 >= _Elements.Length)
			{
				num2 = _Elements.Length;
				_LastUpdateIndex = 0;
			}
			else
			{
				_LastUpdateIndex = num2;
			}
		}
		else
		{
			num = 0;
			num2 = _Elements.Length;
			num3 = 0;
		}
		WaveParticlesQuadtree quadtree = (flag ? _Qroot : null);
		float num4 = (flag ? 0.01f : 1.5f);
		float num5 = (flag ? 0.4f : 8f);
		bool flag2 = false;
		num4 *= _Qroot._Stress;
		num5 *= _Qroot._Stress;
		for (int j = 0; _ParticleGroups != null && j < _ParticleGroups.Length; j++)
		{
			WaveParticlesGroup waveParticlesGroup = _ParticleGroups[j];
			if (waveParticlesGroup == null)
			{
				continue;
			}
			if (waveParticlesGroup.LeftParticle == null || !waveParticlesGroup.LeftParticle.IsAlive)
			{
				_NumParticleGroups--;
				_ParticleGroups[j] = null;
			}
			else
			{
				if (!(time >= waveParticlesGroup.LastUpdateTime + num4))
				{
					continue;
				}
				if (time >= waveParticlesGroup.LastCostlyUpdateTime + num5 && !flag2)
				{
					if (!RectContainsParticleGroup(waveParticlesGroup))
					{
						_NumParticleGroups--;
						_ParticleGroups[j] = null;
						continue;
					}
					waveParticlesGroup.CostlyUpdate(quadtree, time);
					flag2 = true;
					if (waveParticlesGroup.LeftParticle == null || !waveParticlesGroup.LeftParticle.IsAlive)
					{
						_NumParticleGroups--;
						_ParticleGroups[j] = null;
						continue;
					}
				}
				waveParticlesGroup.Update(time);
			}
		}
		if (_Elements == null)
		{
			return;
		}
		for (int k = num; k < num2; k++)
		{
			WaveParticle waveParticle = _Elements[k];
			if (waveParticle != null)
			{
				if (waveParticle.IsAlive)
				{
					if (_MarginRect.Contains(waveParticle.Position))
					{
						Vector3 vertexData = waveParticle.VertexData;
						Vector4 packedParticleData = waveParticle.PackedParticleData;
						_Vertices[num3] = vertexData;
						_TangentsPack[num3++] = packedParticleData;
						_Vertices[num3] = vertexData;
						_TangentsPack[num3++] = packedParticleData;
						_Vertices[num3] = vertexData;
						_TangentsPack[num3++] = packedParticleData;
						_Vertices[num3] = vertexData;
						_TangentsPack[num3++] = packedParticleData;
						_TangentsPackChanged = true;
					}
					else
					{
						base.RemoveElementAt(k);
						_Vertices[num3++].x = float.NaN;
						_Vertices[num3++].x = float.NaN;
						_Vertices[num3++].x = float.NaN;
						_Vertices[num3++].x = float.NaN;
						_Qroot.AddElement(waveParticle);
					}
				}
				else
				{
					base.RemoveElementAt(k);
					_Vertices[num3++].x = float.NaN;
					_Vertices[num3++].x = float.NaN;
					_Vertices[num3++].x = float.NaN;
					_Vertices[num3++].x = float.NaN;
					waveParticle.AddToCache();
				}
			}
			else
			{
				num3 += 4;
			}
		}
	}

	private bool HasParticleGroup(WaveParticlesGroup group)
	{
		for (int i = 0; i < _ParticleGroups.Length; i++)
		{
			if (_ParticleGroups[i] == group)
			{
				return true;
			}
		}
		return false;
	}

	private void AddParticleGroup(WaveParticlesGroup group)
	{
		if (_ParticleGroups.Length == _NumParticleGroups)
		{
			Array.Resize(ref _ParticleGroups, _NumParticleGroups << 1);
		}
		_LastGroupIndex++;
		while (_LastGroupIndex < _ParticleGroups.Length)
		{
			if (_ParticleGroups[_LastGroupIndex] == null)
			{
				_NumParticleGroups++;
				_ParticleGroups[_LastGroupIndex] = group;
				return;
			}
			_LastGroupIndex++;
		}
		for (_LastGroupIndex = 0; _LastGroupIndex < _ParticleGroups.Length; _LastGroupIndex++)
		{
			if (_ParticleGroups[_LastGroupIndex] == null)
			{
				_NumParticleGroups++;
				_ParticleGroups[_LastGroupIndex] = group;
				break;
			}
		}
	}

	private bool RectContainsParticleGroup(WaveParticlesGroup group)
	{
		WaveParticle waveParticle = group.LeftParticle;
		if (!waveParticle.IsAlive)
		{
			return false;
		}
		do
		{
			if (_MarginRect.Contains(waveParticle.Position))
			{
				return true;
			}
			waveParticle = waveParticle.RightNeighbour;
		}
		while (waveParticle != null);
		return false;
	}

	protected override void AddElementAt(WaveParticle particle, int index)
	{
		base.AddElementAt(particle, index);
		if (!HasParticleGroup(particle.Group))
		{
			AddParticleGroup(particle.Group);
		}
	}

	protected override void RemoveElementAt(int index)
	{
		base.RemoveElementAt(index);
		int num = index << 2;
		_Vertices[num++].x = float.NaN;
		_Vertices[num++].x = float.NaN;
		_Vertices[num++].x = float.NaN;
		_Vertices[num].x = float.NaN;
	}

	protected override void SpawnChildNodes()
	{
		_Mesh.Destroy();
		_Mesh = null;
		float width = Rect.width * 0.5f;
		float height = Rect.height * 0.5f;
		_A = (_Qa = new WaveParticlesQuadtree(_Qroot, new Rect(Rect.xMin, _Center.y, width, height), _Elements.Length));
		_B = (_Qb = new WaveParticlesQuadtree(_Qroot, new Rect(_Center.x, _Center.y, width, height), _Elements.Length));
		_C = (_Qc = new WaveParticlesQuadtree(_Qroot, new Rect(Rect.xMin, Rect.yMin, width, height), _Elements.Length));
		_D = (_Qd = new WaveParticlesQuadtree(_Qroot, new Rect(_Center.x, Rect.yMin, width, height), _Elements.Length));
		_Vertices = null;
		_TangentsPack = null;
		_ParticleGroups = null;
		_NumParticleGroups = 0;
	}

	private void CreateMesh()
	{
		int num = _Elements.Length << 2;
		_Vertices = new Vector3[num];
		for (int i = 0; i < _Vertices.Length; i++)
		{
			_Vertices[i].x = float.NaN;
		}
		_TangentsPack = new Vector4[num];
		Vector2[] array = new Vector2[num];
		int num2 = 0;
		while (num2 < array.Length)
		{
			array[num2++] = new Vector2(0f, 0f);
			array[num2++] = new Vector2(0f, 1f);
			array[num2++] = new Vector2(1f, 1f);
			array[num2++] = new Vector2(1f, 0f);
		}
		int[] array2 = new int[num];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = j;
		}
		_Mesh = new Mesh
		{
			hideFlags = HideFlags.DontSave,
			name = "Wave Particles",
			vertices = _Vertices,
			uv = array,
			tangents = _TangentsPack
		};
		_Mesh.SetIndices(array2, MeshTopology.Quads, 0);
	}
}
