using UnityEngine;

[ExecuteInEditMode]
public class AnimatedProjector : MonoBehaviour
{
	public bool runEditor = true;

	public float fps = 30f;

	public bool reload;

	public string TextureToReload;

	public Texture2D[] frames;

	private int frameIndex;

	private Light projector;

	private void Awake()
	{
		projector = GetComponent<Light>();
		NextFrame();
		InvokeRepeating("NextFrame", 1f / fps, 1f / fps);
	}

	private void NextFrame()
	{
		projector.cookie = frames[frameIndex];
		frameIndex = (frameIndex + 1) % frames.Length;
	}
}
