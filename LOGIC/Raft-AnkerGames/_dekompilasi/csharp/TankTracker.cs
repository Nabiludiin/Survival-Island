using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TankTracker : ManagerTracker<Tank>
{
	public PipeGroup pipeGroup;

	public Dictionary<PipeSocketType, List<PipeSocket_Tank>> inputSockets;

	public Dictionary<PipeSocketType, List<PipeSocket_Tank>> outputSockets;

	public List<Tank> tanksToBeNetworked;

	public TankTracker(PipeGroup p_pipeGroup, float updateRate)
		: base(updateRate)
	{
		this.pipeGroup = p_pipeGroup;
		inputSockets = new Dictionary<PipeSocketType, List<PipeSocket_Tank>>();
		outputSockets = new Dictionary<PipeSocketType, List<PipeSocket_Tank>>();
		AddInputTanksFromPipeGroup(p_pipeGroup);
		AddOutPutTanksFromPipeGroup(p_pipeGroup);
		tanksToBeNetworked = new List<Tank>();
		PipeGroup pipeGroup = this.pipeGroup;
		pipeGroup.OnAddSocket = (Action<PipeSocket>)Delegate.Combine(pipeGroup.OnAddSocket, new Action<PipeSocket>(OnSocketAdded));
	}

	public void AddOutPutTanksFromPipeGroup(PipeGroup pipeGroup)
	{
		List<PipeSocketType> list = new List<PipeSocketType>();
		foreach (PipeSocketTypeConnection typeConnection in PipeSocket.TypeConnections)
		{
			if (!list.Contains(typeConnection.output))
			{
				List<PipeSocket_Tank> socketsFromType = pipeGroup.GetSocketsFromType<PipeSocket_Tank>(typeConnection.output);
				if (socketsFromType != null && socketsFromType.Count > 0)
				{
					outputSockets.Add(typeConnection.output, socketsFromType);
				}
				else
				{
					outputSockets.Add(typeConnection.output, new List<PipeSocket_Tank>());
				}
				list.Add(typeConnection.output);
			}
		}
	}

	public void AddInputTanksFromPipeGroup(PipeGroup pipeGroup)
	{
		List<PipeSocketType> list = new List<PipeSocketType>();
		foreach (PipeSocketTypeConnection typeConnection in PipeSocket.TypeConnections)
		{
			if (!list.Contains(typeConnection.input))
			{
				List<PipeSocket_Tank> socketsFromType = pipeGroup.GetSocketsFromType<PipeSocket_Tank>(typeConnection.input);
				if (socketsFromType != null && socketsFromType.Count > 0)
				{
					inputSockets.Add(typeConnection.input, socketsFromType);
				}
				else
				{
					inputSockets.Add(typeConnection.input, new List<PipeSocket_Tank>());
				}
				list.Add(typeConnection.input);
			}
		}
	}

	public void AverageOutNumbers(List<PipeSocket_Tank> outPutSockets, List<PipeSocket_Tank> inputSockets)
	{
		if (outPutSockets == null || inputSockets == null || outPutSockets.Count <= 0 || inputSockets.Count <= 0)
		{
			return;
		}
		Dictionary<PipeSocket_Tank, List<PipeSocket_Tank>> dictionary = new Dictionary<PipeSocket_Tank, List<PipeSocket_Tank>>();
		foreach (PipeSocket_Tank outPutSocket in outPutSockets)
		{
			foreach (PipeSocket_Tank inputSocket in inputSockets)
			{
				if (!(outPutSocket.connectedTank == inputSocket.connectedTank))
				{
					if (dictionary.ContainsKey(outPutSocket))
					{
						dictionary[outPutSocket].Add(inputSocket);
						continue;
					}
					dictionary.Add(outPutSocket, new List<PipeSocket_Tank> { inputSocket });
				}
			}
		}
		if (dictionary.Count == 0)
		{
			return;
		}
		float[] array = new float[dictionary.Count];
		for (int i = 0; i < dictionary.Count; i++)
		{
			array[i] = dictionary.ElementAt(i).Key.connectedTank.CurrentTankAmount;
		}
		int num = 0;
		foreach (KeyValuePair<PipeSocket_Tank, List<PipeSocket_Tank>> item in dictionary)
		{
			if (array[num] == 0f)
			{
				num++;
				continue;
			}
			float num2 = 0f;
			float num3 = 0f;
			foreach (PipeSocket_Tank item2 in item.Value)
			{
				num2 += item2.connectedTank.maxCapacity - item2.connectedTank.CurrentTankAmount;
			}
			if (array[num] > num2)
			{
				array[num] = num2;
			}
			foreach (PipeSocket_Tank item3 in item.Value)
			{
				float num4 = item3.connectedTank.maxCapacity - item3.connectedTank.CurrentTankAmount;
				if (num4 != 0f)
				{
					float num5 = num4 / num2;
					float num6 = array[num] * num5;
					num3 += num6;
					item3.connectedTank.SetTankAmount(item3.connectedTank.CurrentTankAmount + num6);
					tanksToBeNetworked.AddUniqueOnly(item3.connectedTank);
				}
			}
			if (num3 > 0f)
			{
				item.Key.connectedTank.SetTankAmount(item.Key.connectedTank.CurrentTankAmount - num3);
				tanksToBeNetworked.AddUniqueOnly(item.Key.connectedTank);
			}
			num++;
		}
	}

	protected override void OnTrackerTrigger()
	{
		base.OnTrackerTrigger();
		foreach (PipeSocketTypeConnection typeConnection in PipeSocket.TypeConnections)
		{
			if (inputSockets.ContainsKey(typeConnection.input) && outputSockets.ContainsKey(typeConnection.output))
			{
				AverageOutNumbers(outputSockets[typeConnection.output], inputSockets[typeConnection.input]);
			}
		}
		SendNeighbourHoodUpdate();
	}

	public void SendNeighbourHoodUpdate()
	{
		foreach (Tank item in tanksToBeNetworked)
		{
			item.SendTankSetValueMessage(null);
		}
		tanksToBeNetworked.Clear();
	}

	public void RemoveTrackerAdditions()
	{
		PipeGroup pipeGroup = this.pipeGroup;
		pipeGroup.OnAddSocket = (Action<PipeSocket>)Delegate.Remove(pipeGroup.OnAddSocket, new Action<PipeSocket>(OnSocketAdded));
	}

	public void OnSocketAdded(PipeSocket socket)
	{
		PipeSocket_Tank pipeSocket_Tank = socket as PipeSocket_Tank;
		if (!(pipeSocket_Tank == null))
		{
			if (inputSockets.ContainsKey(pipeSocket_Tank.socketType))
			{
				inputSockets[pipeSocket_Tank.socketType].Add(pipeSocket_Tank);
			}
			if (outputSockets.ContainsKey(pipeSocket_Tank.socketType))
			{
				outputSockets[pipeSocket_Tank.socketType].Add(pipeSocket_Tank);
			}
		}
	}
}
