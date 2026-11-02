using UnityEngine;

public class PromoRunner : MonoBehaviour
{
	public float movementSpeed;

	private void Start()
	{
	}

	private void Update()
	{
		transform.position += transform.forward * Time.deltaTime * movementSpeed;
	}
}
