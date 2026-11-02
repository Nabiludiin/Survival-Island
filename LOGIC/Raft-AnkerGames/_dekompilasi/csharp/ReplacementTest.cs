using UnityEngine;

[ExecuteAlways]
public class ReplacementTest : MonoBehaviour
{
	public Shader Shader;

	public string Tag;

	private void Start()
	{
	}

	private void Update()
	{
		if (Shader != null)
		{
			GetComponent<Camera>().enabled = false;
			GetComponent<Camera>().RenderWithShader(Shader, Tag);
		}
	}
}
