using UnityEngine;
using UnityEngine.UI;

public class RandomizeMainMenuRaft : MonoBehaviour
{
	[SerializeField]
	private bool forceASpecificRaft;

	[SerializeField]
	private string specificRaftCreator;

	[SerializeField]
	private Text builtByText;

	[SerializeField]
	private RaftBounds bounds;

	[SerializeField]
	private Transform chair;

	[SerializeField]
	private MainMenuRaft originalRaft;

	[SerializeField]
	private MainMenuRaft[] additionalRafts;

	private void Start()
	{
		InitializeRaft();
	}

	private void InitializeRaft()
	{
		if (forceASpecificRaft)
		{
			MainMenuRaft mainMenuRaftFromCreatorName = GetMainMenuRaftFromCreatorName(specificRaftCreator);
			ApplyMenuRaft((mainMenuRaftFromCreatorName == null) ? originalRaft : mainMenuRaftFromCreatorName);
		}
		else
		{
			RandomizeMenuRaft();
		}
	}

	private MainMenuRaft GetMainMenuRaftFromCreatorName(string creatorName)
	{
		MainMenuRaft[] array = additionalRafts;
		foreach (MainMenuRaft mainMenuRaft in array)
		{
			if (mainMenuRaft != null && mainMenuRaft.creatorName == creatorName)
			{
				return mainMenuRaft;
			}
		}
		return null;
	}

	private void RandomizeMenuRaft()
	{
		if (Application.isPlaying)
		{
			TurnOfAllRafts();
			bool flag = false;
			if (additionalRafts.ContainsItems())
			{
				int num = Random.Range(0, additionalRafts.Length);
				flag = ApplyMenuRaft(additionalRafts[num]);
			}
			if (!flag)
			{
				ApplyMenuRaft(originalRaft);
			}
		}
	}

	private bool ApplyMenuRaft(MainMenuRaft raft)
	{
		if (raft != null && raft.IsValid())
		{
			raft.raftParent.SetActiveSafe(value: true);
			chair.SetParentSafe(raft.chairTransform);
			Transform transform = chair;
			Vector3 localPosition = (chair.localEulerAngles = Vector3.zero);
			transform.localPosition = localPosition;
			UpdateBounds(raft);
			if (!string.IsNullOrEmpty(raft.creatorName))
			{
				LocalizationParameters.mainMenuRaftBuiltByName = raft.creatorName;
				builtByText.gameObject.SetActiveSafe(value: true);
				builtByText.text = Helper.GetTerm("Menu/RaftBuiltBy", applyParameters: true);
			}
			else
			{
				builtByText.gameObject.SetActiveSafe(value: false);
			}
			return true;
		}
		return false;
	}

	private void UpdateBounds(MainMenuRaft raft)
	{
		if (!(bounds != null))
		{
			return;
		}
		bounds.ClearWalkableBlocks();
		foreach (Block walkableBlock in raft.walkableBlocks)
		{
			if (walkableBlock != null)
			{
				bounds.AddWalkableBlock(walkableBlock, replicating: true, updateBounds: false);
			}
		}
		bounds.UpdateBounds();
	}

	private void TurnOfAllRafts()
	{
		originalRaft.raftParent.SetActiveSafe(value: false);
		if (!additionalRafts.ContainsItems())
		{
			return;
		}
		MainMenuRaft[] array = additionalRafts;
		foreach (MainMenuRaft mainMenuRaft in array)
		{
			if (mainMenuRaft != null && mainMenuRaft.raftParent != null)
			{
				mainMenuRaft.raftParent.SetActiveSafe(value: false);
			}
		}
	}
}
