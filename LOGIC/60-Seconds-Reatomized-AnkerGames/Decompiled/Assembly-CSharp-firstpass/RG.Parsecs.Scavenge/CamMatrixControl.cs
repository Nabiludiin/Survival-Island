using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CamMatrixControl : MonoBehaviour
{
	public float rotate_angle;

	public float scale = 1f;

	public Vector2 shearing = Vector2.zero;

	public Vector2 translation = Vector2.zero;

	private Matrix4x4 MatRotate = Matrix4x4.identity;

	private Matrix4x4 MatScale = Matrix4x4.identity;

	private Matrix4x4 MatXShearing = Matrix4x4.identity;

	private Matrix4x4 MatYShearing = Matrix4x4.identity;

	private Matrix4x4 MatTranslation = Matrix4x4.identity;

	private Matrix4x4 OriMatProjection;

	private Camera cam;

	private void Start()
	{
		cam = Camera.main;
		OriMatProjection = cam.projectionMatrix;
	}

	private void Update()
	{
		MatScale[0, 0] = scale;
		MatScale[1, 1] = scale;
		MatRotate[0, 0] = Mathf.Cos(rotate_angle * ((float)Math.PI / 180f));
		MatRotate[0, 1] = 0f - Mathf.Sin(rotate_angle * ((float)Math.PI / 180f));
		MatRotate[1, 0] = Mathf.Sin(rotate_angle * ((float)Math.PI / 180f));
		MatRotate[1, 1] = Mathf.Cos(rotate_angle * ((float)Math.PI / 180f));
		MatXShearing[0, 1] = shearing.x;
		MatYShearing[1, 0] = shearing.y;
		MatTranslation[0, 2] = translation.x;
		MatTranslation[1, 2] = translation.y;
		cam.projectionMatrix = OriMatProjection * MatScale * MatRotate * MatXShearing * MatYShearing * MatTranslation;
	}
}
