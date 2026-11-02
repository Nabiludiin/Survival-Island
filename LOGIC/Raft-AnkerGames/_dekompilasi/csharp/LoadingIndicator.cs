using System.Collections;
using UnityEngine;

public class LoadingIndicator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_arrows;

	private void OnEnable()
	{
		StartCoroutine("TimeoutLoading");
	}

	private void Update()
	{
		m_arrows.transform.Rotate(new Vector3(0f, 0f, 1f), (0f - Time.deltaTime) * 180f);
	}

	private IEnumerator TimeoutLoading()
	{
		yield return new WaitForSecondsRealtime(30f);
		gameObject.SetActive(value: false);
	}
}
