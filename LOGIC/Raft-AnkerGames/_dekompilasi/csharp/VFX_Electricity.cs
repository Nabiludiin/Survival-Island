using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Electricity : MonoBehaviour
{
	private static float distanceToReach = 0.02f;

	public float movementSpeed = 1f;

	public float pauseTime = 1f;

	public GameObject feedbackParent;

	[HideInInspector]
	public bool reverse;

	private bool initialized;

	private bool isActive;

	private bool paused;

	private List<Vector3> path = new List<Vector3>();

	private int currentPathIndex;

	private Coroutine pauseRoutine;

	private void Update()
	{
		if (!initialized || !isActive)
		{
			return;
		}
		if (paused)
		{
			if (pauseRoutine == null)
			{
				paused = false;
			}
			return;
		}
		Vector3 vector = path[currentPathIndex];
		transform.position = Vector3.MoveTowards(transform.position, vector, movementSpeed * Time.deltaTime);
		if (!(Vector3.Distance(transform.position, vector) < distanceToReach))
		{
			return;
		}
		int num = path.Count - 1;
		if (reverse)
		{
			currentPathIndex.IncrementCycle(-1, 0, num);
		}
		else
		{
			currentPathIndex.IncrementCycle(1, 0, num);
		}
		if (!paused)
		{
			if (!reverse && currentPathIndex == 0)
			{
				pauseRoutine = CoroutineManager.Singleton.AddCoroutine(Paus(path[0]));
			}
			else if (reverse && currentPathIndex == num)
			{
				pauseRoutine = CoroutineManager.Singleton.AddCoroutine(Paus(path[num]));
			}
		}
	}

	private void OnDestroy()
	{
		if (pauseRoutine != null && CoroutineManager.Singleton != null)
		{
			CoroutineManager.Singleton.StopCoroutine(pauseRoutine);
			pauseRoutine = null;
		}
	}

	public void Init(List<Vector3> path)
	{
		this.path = path;
		initialized = true;
		SetActive(value: true);
		currentPathIndex = 0;
	}

	public void SetActive(bool value)
	{
		isActive = value;
		feedbackParent.SetActiveSafe(value);
		if (value && pauseRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(pauseRoutine);
			pauseRoutine = null;
		}
	}

	private IEnumerator Paus(Vector3 afterPausPosition)
	{
		paused = true;
		yield return new WaitForSeconds(pauseTime / 2f);
		if (feedbackParent != null)
		{
			feedbackParent.gameObject.SetActiveSafe(value: false);
		}
		if (transform != null)
		{
			transform.position = afterPausPosition;
		}
		yield return new WaitForSeconds(pauseTime / 2f);
		if (feedbackParent != null)
		{
			feedbackParent.gameObject.SetActiveSafe(value: true);
		}
		paused = false;
	}
}
