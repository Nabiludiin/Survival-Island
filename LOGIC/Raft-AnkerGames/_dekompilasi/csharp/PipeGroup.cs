using System;
using System.Collections.Generic;
using System.Linq;

public class PipeGroup
{
	private List<Pipe> pipes;

	public List<PipeSocket> pipeSockets;

	public Action<PipeSocket> OnAddSocket;

	public bool AddPipe(Pipe pipeToAdd)
	{
		if (pipes == null)
		{
			pipes = new List<Pipe>();
		}
		if (pipeToAdd != null)
		{
			pipeToAdd.pipeGroup = this;
			if (!pipes.Contains(pipeToAdd))
			{
				pipes.Add(pipeToAdd);
				TryAddPipeSocket(pipeToAdd);
			}
			return true;
		}
		return false;
	}

	public void AddPipes(List<Pipe> pipesToAdd)
	{
		if (pipesToAdd.ContainsItems())
		{
			for (int i = 0; i < pipesToAdd.Count; i++)
			{
				AddPipe(pipesToAdd[i]);
			}
		}
	}

	public bool RemovePipe(Pipe pipeToRemove)
	{
		if (pipes.ContainsItems())
		{
			pipeToRemove.pipeGroup = null;
			pipes.Remove(pipeToRemove);
		}
		if (pipeSockets.ContainsItems() && pipeToRemove is PipeSocket)
		{
			pipeSockets.Remove(pipeToRemove as PipeSocket);
		}
		return false;
	}

	public void DisbandGroup()
	{
		for (int i = 0; i < pipes.Count; i++)
		{
			pipes[i].pipeGroup = null;
		}
		pipes.Clear();
		if (pipeSockets != null)
		{
			pipeSockets.Clear();
		}
	}

	public List<Pipe> GetPipes()
	{
		return pipes;
	}

	public List<Pipe> GetPipesCopy()
	{
		Pipe[] array = new Pipe[pipes.Count];
		pipes.CopyTo(array);
		return array.ToList();
	}

	public bool ContainsPipe(Pipe pipe)
	{
		if (pipes.ContainsItems())
		{
			return pipes.Contains(pipe);
		}
		return false;
	}

	public bool IsStable()
	{
		for (int i = 0; i < pipes.Count; i++)
		{
			if (pipes[i].block_Pipe.IsPipeStable())
			{
				return true;
			}
		}
		return false;
	}

	public void DestroyGroup()
	{
		for (int i = 0; i < pipes.Count; i++)
		{
			Pipe pipe = pipes[i];
			pipe.isAboutToBeDestroyed = true;
			PipeGroupManager.UntrackPipe(pipe.snappedBuildingPosition);
			BlockCreator.RemoveBlockNetwork(pipe.block_Pipe, null, updateRaftBounds: true);
		}
		PipeGroupManager.RemovePipeGroup(this);
	}

	public List<T> GetSocketsFromType<T>(PipeSocketType pipeType) where T : PipeSocket
	{
		List<T> list = null;
		if (pipeSockets == null)
		{
			return null;
		}
		foreach (PipeSocket pipeSocket in pipeSockets)
		{
			if (pipeSocket == null || pipeSocket.socketType != pipeType)
			{
				continue;
			}
			T val = (T)pipeSocket;
			if (val != null)
			{
				if (list == null)
				{
					list = new List<T>();
				}
				list.AddUniqueOnly(val);
			}
		}
		return list;
	}

	private void TryAddPipeSocket(Pipe pipeToAdd)
	{
		if (pipeSockets == null)
		{
			pipeSockets = new List<PipeSocket>();
		}
		PipeSocket pipeSocket = pipeToAdd as PipeSocket;
		if (pipeSocket != null)
		{
			pipeSockets.AddUniqueOnly(pipeSocket);
			OnAddSocket?.Invoke(pipeSocket);
		}
	}

	private void TryRemovePipeSocket(Pipe pipeToAdd)
	{
		if (pipeSockets != null)
		{
			PipeSocket pipeSocket = pipeToAdd as PipeSocket;
			if (pipeSocket != null)
			{
				pipeSockets.Remove(pipeSocket);
			}
		}
	}
}
