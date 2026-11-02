using TMPro;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class FPSLabel : MonoBehaviour
{
	public TextMeshProUGUI label;

	private int _fps;

	private float deltaTime;

	private void Awake()
	{
		Object.Destroy(gameObject);
	}

	private void Start()
	{
		label.text = _fps.ToString();
	}

	private void Update()
	{
		float num = Time.deltaTime;
		deltaTime += (num - deltaTime) * 0.1f;
		float num2 = deltaTime * 1000f;
		int num3 = Mathf.CeilToInt(1f / deltaTime);
		if (num3 != _fps)
		{
			_fps = num3;
			label.text = $"{num2:0.0} ms    {num3:0.} fps";
		}
	}
}
