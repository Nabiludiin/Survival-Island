using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Steamworks;
using Unity.Netcode;
using UnityEngine;

public class TikiPole_Head : TikiPole_Piece, IBlockPlaced
{
	[Serializable]
	public class Message_TikiPoleComplete : Message_NetworkBehaviour
	{
		public uint[] pieceBlockIndexes;

		protected bool Equals(Message_TikiPoleComplete other)
		{
			if (pieceBlockIndexes == null && other.pieceBlockIndexes != null)
			{
				return false;
			}
			if (pieceBlockIndexes != null && other.pieceBlockIndexes == null)
			{
				return false;
			}
			if (Equals((Message_NetworkBehaviour)other))
			{
				if (pieceBlockIndexes != null || other.pieceBlockIndexes != null)
				{
					return Enumerable.SequenceEqual(pieceBlockIndexes, other.pieceBlockIndexes);
				}
				return true;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((Message_TikiPoleComplete)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), pieceBlockIndexes);
		}

		public static bool operator ==(Message_TikiPoleComplete left, Message_TikiPoleComplete right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(Message_TikiPoleComplete left, Message_TikiPoleComplete right)
		{
			return !object.Equals(left, right);
		}

		public Message_TikiPoleComplete()
		{
		}

		public Message_TikiPoleComplete(Messages type, MonoBehaviour_Network behaviour, TikiPole_Piece[] tikis)
			: base(type, behaviour)
		{
			List<uint> list = new List<uint>();
			for (int i = 0; i < tikis.Length; i++)
			{
				TikiPole_Piece tikiPole_Piece = tikis[i];
				if (!(tikiPole_Piece == null) && !(tikiPole_Piece.block == null))
				{
					list.Add(tikis[i].block.ObjectIndex);
				}
			}
			pieceBlockIndexes = list.ToArray();
		}

		public override void SerializeFast(FastBufferWriter writer)
		{
			base.SerializeFast(writer);
			writer.WriteValueSafe(pieceBlockIndexes, default(FastBufferWriter.ForPrimitives));
		}

		public override void DeserializeFast(FastBufferReader reader)
		{
			base.DeserializeFast(reader);
			reader.ReadValue(out pieceBlockIndexes, default(FastBufferWriter.ForPrimitives));
		}
	}

	[SerializeField]
	private Item_Base completedTotem;

	[SerializeField]
	private Item_Base tikiMask;

	[SerializeField]
	private float tikiPiecesCheckDownwardsRange = 5f;

	[SerializeField]
	private float spinSpeed = 5f;

	[SerializeField]
	private ParticleSystem completeTikiParticles;

	[SerializeField]
	private StudioEventEmitterSustain emitter_spin;

	[SerializeField]
	private StudioEventEmitter emitter_finishSpin;

	public TikiPole_Piece[] polePieces;

	private static TIKIPIECEINDEX[] targetTikiPoleStructure = new TIKIPIECEINDEX[4]
	{
		TIKIPIECEINDEX.HEAD,
		TIKIPIECEINDEX.MID2,
		TIKIPIECEINDEX.MID1,
		TIKIPIECEINDEX.FEET
	};

	private Network_Player hostPlayer;

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		if (Raft_Network.IsHost)
		{
			hostPlayer = ComponentManager<Network_Player>.Value;
			Invoke("OnPlacementCheck", 1f);
		}
	}

	public void StartSpinningPieces()
	{
		StartCoroutine(SpinPieces());
	}

	private IEnumerator SpinPieces()
	{
		AchievementHandler.UnlockAchievement(AchievementType.ach_tiki_completed);
		bool isBroken = false;
		int maxSpinIndex = 0;
		emitter_spin?.Play();
		while (maxSpinIndex + 1 < polePieces.Length)
		{
			int num = 0;
			for (int i = 0; i < maxSpinIndex + 1; i++)
			{
				if (polePieces[i] == null || i + 1 >= polePieces.Length || polePieces[i + 1] == null)
				{
					isBroken = true;
					break;
				}
				Transform transform = polePieces[i].transform;
				Transform transform2 = polePieces[i + 1].transform;
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.MoveTowardsAngle(transform.localEulerAngles.y, transform2.localEulerAngles.y, Time.deltaTime * spinSpeed), transform.localEulerAngles.z);
				if (emitter_spin != null)
				{
					emitter_spin.eventEmitter.transform.position = transform.position;
				}
				if ((double)Mathf.Abs(transform.localEulerAngles.y - transform2.localEulerAngles.y) <= 0.1)
				{
					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform2.localEulerAngles.y, transform.localEulerAngles.z);
					num++;
				}
			}
			if (isBroken)
			{
				break;
			}
			if (num == maxSpinIndex + 1)
			{
				maxSpinIndex++;
				emitter_spin?.Stop();
				yield return new WaitForSeconds(0.5f);
				emitter_spin?.Play();
			}
			yield return null;
		}
		if (!isBroken)
		{
			emitter_finishSpin?.Play();
			float currentSpeed = spinSpeed;
			float targetSpeed = spinSpeed * 10f;
			TikiPole_Piece tikiPole_Piece = polePieces[polePieces.Length - 1];
			float startEulerY = ((tikiPole_Piece != null) ? tikiPole_Piece.transform.localEulerAngles.y : float.MaxValue);
			while (currentSpeed != targetSpeed)
			{
				currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5f);
				if (Mathf.Abs(currentSpeed - targetSpeed) < 0.001f)
				{
					currentSpeed = targetSpeed;
				}
				for (int j = 0; j < polePieces.Length; j++)
				{
					TikiPole_Piece tikiPole_Piece2 = polePieces[j];
					if (!(tikiPole_Piece2 == null))
					{
						if (currentSpeed != targetSpeed)
						{
							Vector3 localEulerAngles = tikiPole_Piece2.transform.localEulerAngles;
							localEulerAngles.y += Time.deltaTime * currentSpeed * (float)((j % 2 != 0) ? 1 : (-1));
							tikiPole_Piece2.transform.localEulerAngles = localEulerAngles;
						}
						else if (startEulerY != float.MaxValue)
						{
							Vector3 localEulerAngles2 = tikiPole_Piece2.transform.localEulerAngles;
							localEulerAngles2.y = startEulerY;
							tikiPole_Piece2.transform.localEulerAngles = localEulerAngles2;
						}
					}
				}
				yield return null;
			}
			if (emitter_finishSpin != null)
			{
				emitter_finishSpin.Stop();
			}
		}
		if (completeTikiParticles != null)
		{
			completeTikiParticles.transform.SetParentSafe(base.transform.parent);
			completeTikiParticles.Play();
		}
		if (Raft_Network.IsHost)
		{
			ReplaceTotemPiecesWithCompletedPrefab();
		}
		emitter_spin?.StopHard();
	}

	private void OnPlacementCheck()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (hostPlayer == null)
		{
			hostPlayer = ComponentManager<Network_Player>.Value;
			if (hostPlayer == null)
			{
				Debug.LogError("Host player is null, when checking for tikipole completion.");
			}
		}
		polePieces = FindTikisUnderneath(tikiPiecesCheckDownwardsRange);
		if (polePieces != null && DoesOrderMatch(polePieces, targetTikiPoleStructure))
		{
			Message_TikiPoleComplete message = new Message_TikiPoleComplete(Messages.TikiPoleCompleted, hostPlayer.Network.NetworkIDManager, polePieces);
			hostPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			StartSpinningPieces();
		}
	}

	private void ReplaceTotemPiecesWithCompletedPrefab()
	{
		TikiPole_Piece[] array = polePieces;
		foreach (TikiPole_Piece tikiPole_Piece in array)
		{
			if (!(tikiPole_Piece == null))
			{
				BlockCreator.RemoveBlockNetwork(tikiPole_Piece.block, null, updateRaftBounds: true);
			}
		}
		TikiPole_Piece tikiPieceByTikiIndex = GetTikiPieceByTikiIndex(polePieces, TIKIPIECEINDEX.FEET);
		if (tikiPieceByTikiIndex == null)
		{
			Debug.LogError("Unable to find tikifeet, wont swap for tikipole :(");
		}
		Vector3 localBuildPosition = ((tikiPieceByTikiIndex != null) ? tikiPieceByTikiIndex.transform.localPosition : (transform.position - transform.up * ((float)targetTikiPoleStructure.Length * 1f)));
		Vector3 localBuildRotation = ((tikiPieceByTikiIndex != null) ? tikiPieceByTikiIndex.transform.localEulerAngles : transform.localEulerAngles);
		TrophyHolder componentInChildren = hostPlayer.BlockCreator.CreateBlockCheat(completedTotem, localBuildPosition, localBuildRotation, DPS.Default, -1).GetComponentInChildren<TrophyHolder>();
		if (componentInChildren != null)
		{
			componentInChildren.PlaceTrophyNetworked(null, tikiMask);
		}
	}
}
