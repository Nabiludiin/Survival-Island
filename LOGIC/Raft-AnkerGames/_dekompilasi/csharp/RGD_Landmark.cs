using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_Landmark : SerializableBehaviour
{
	public int uniqueLandmarkIndex;

	public RGD_LandmarkItem[] rgdLandmarkItems;

	public RGD_LandmarkSpawner[] rgdLandmarkSpawners;

	protected bool Equals(RGD_Landmark other)
	{
		if (rgdLandmarkItems == null && other.rgdLandmarkItems != null)
		{
			return false;
		}
		if (rgdLandmarkItems != null && other.rgdLandmarkItems == null)
		{
			return false;
		}
		if (rgdLandmarkSpawners == null && other.rgdLandmarkSpawners != null)
		{
			return false;
		}
		if (rgdLandmarkSpawners != null && other.rgdLandmarkSpawners == null)
		{
			return false;
		}
		for (int i = 0; i < rgdLandmarkItems.Length; i++)
		{
			if (!rgdLandmarkItems[i].Equals(other.rgdLandmarkItems[i]))
			{
				Debug.LogError("Error in landmark item match");
			}
		}
		if (uniqueLandmarkIndex == other.uniqueLandmarkIndex && ((rgdLandmarkItems == null && other.rgdLandmarkItems == null) || Enumerable.SequenceEqual(rgdLandmarkItems, other.rgdLandmarkItems)))
		{
			if (rgdLandmarkSpawners != null || other.rgdLandmarkSpawners != null)
			{
				return Enumerable.SequenceEqual(rgdLandmarkSpawners, other.rgdLandmarkSpawners);
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
		return Equals((RGD_Landmark)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(uniqueLandmarkIndex, rgdLandmarkItems, rgdLandmarkSpawners);
	}

	public static bool operator ==(RGD_Landmark left, RGD_Landmark right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Landmark left, RGD_Landmark right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Landmark()
	{
	}

	public RGD_Landmark(Landmark landmark)
	{
		uniqueLandmarkIndex = (int)landmark.uniqueLandmarkIndex;
		rgdLandmarkItems = new RGD_LandmarkItem[landmark.landmarkItems.Length];
		int num = 0;
		LandmarkItem[] landmarkItems = landmark.landmarkItems;
		foreach (LandmarkItem landmarkItem in landmarkItems)
		{
			rgdLandmarkItems[num++] = landmarkItem.GetSave();
		}
		LandmarkEntitySpawner[] spawners = landmark.spawners;
		if (spawners.Length == 0)
		{
			return;
		}
		List<RGD_LandmarkSpawner> list = new List<RGD_LandmarkSpawner>(spawners.Length);
		for (int j = 0; j < spawners.Length; j++)
		{
			if (spawners[j].spawnedEntityBehaviour != null)
			{
				list.Add(new RGD_LandmarkSpawner(spawners[j]));
			}
		}
		rgdLandmarkSpawners = ((list.Count > 0) ? list.ToArray() : null);
	}

	public virtual void RestoreLandmark(Landmark landmark, bool isHost)
	{
		if (landmark == null)
		{
			Debug.LogError("NULL, Tried to restore a landmark which was NULL");
			return;
		}
		if (rgdLandmarkItems != null)
		{
			for (int i = 0; i < rgdLandmarkItems.Length; i++)
			{
				RGD_LandmarkItem rGD_LandmarkItem = rgdLandmarkItems[i];
				if (rGD_LandmarkItem == null)
				{
					Debug.LogError("NULL, RGDLandmarkItem is null when searching on landmark " + landmark.uniqueLandmarkIndex);
					continue;
				}
				LandmarkItem landmarkItemByLocalPosition = landmark.GetLandmarkItemByLocalPosition(rGD_LandmarkItem.LocalPosition);
				if (!(landmarkItemByLocalPosition == null))
				{
					rGD_LandmarkItem.RestoreLandmarkItem(landmarkItemByLocalPosition);
				}
			}
		}
		if (rgdLandmarkSpawners == null)
		{
			return;
		}
		LandmarkEntitySpawner[] componentsInChildren = landmark.GetComponentsInChildren<LandmarkEntitySpawner>();
		if (componentsInChildren == null)
		{
			return;
		}
		for (int j = 0; j < rgdLandmarkSpawners.Length; j++)
		{
			RGD_LandmarkSpawner rGD_LandmarkSpawner = rgdLandmarkSpawners[j];
			if (rGD_LandmarkSpawner == null)
			{
				continue;
			}
			foreach (LandmarkEntitySpawner landmarkEntitySpawner in componentsInChildren)
			{
				if (!(landmarkEntitySpawner == null) && landmarkEntitySpawner.spawnerIndex == rGD_LandmarkSpawner.spawnerIndex)
				{
					landmarkEntitySpawner.Restore(rGD_LandmarkSpawner);
					break;
				}
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("UniqueLandmarkIndex", uniqueLandmarkIndex);
		info.AddValue("LandmarkItems", rgdLandmarkItems);
		info.AddValue("RGDLandmarkSpawners", rgdLandmarkSpawners);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in uniqueLandmarkIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>(rgdLandmarkItems.Length, default(FastBufferWriter.ForPrimitives));
		RGD_LandmarkItem[] array = rgdLandmarkItems;
		foreach (RGD_LandmarkItem rGD_LandmarkItem in array)
		{
			if (!(rGD_LandmarkItem is RGD_LandmarkItem_CarryNetworked))
			{
				if (!(rGD_LandmarkItem is RGD_LandmarkItem_ChallengePipe))
				{
					if (!(rGD_LandmarkItem is RGD_LandmarkItem_CharacterUnlock))
					{
						if (!(rGD_LandmarkItem is RGD_LandmarkItem_ClawChallenge))
						{
							if (!(rGD_LandmarkItem is RGD_LandmarkItem_ComputerChallenge))
							{
								if (!(rGD_LandmarkItem is RGD_LandmarkItem_Container))
								{
									if (!(rGD_LandmarkItem is RGD_LandmarkItem_Elevator))
									{
										if (!(rGD_LandmarkItem is RGD_LandmarkItem_JusticeScale))
										{
											if (!(rGD_LandmarkItem is RGD_LandmarkItem_Keypad))
											{
												if (!(rGD_LandmarkItem is RGD_LandmarkItem_PickupItem))
												{
													if (!(rGD_LandmarkItem is RGD_LandmarkItem_QuestEvent_OneWay))
													{
														if (!(rGD_LandmarkItem is RGD_LandmarkItem_WaterLoggedHatch))
														{
															if (!(rGD_LandmarkItem is RGD_LandmarkItem_QuestEvent))
															{
																if (!(rGD_LandmarkItem is RGD_LandmarkItem_SeleneFightChallenge))
																{
																	if (!(rGD_LandmarkItem is RGD_LandmarkItem_SnowmobileShed))
																	{
																		if (!(rGD_LandmarkItem is RGD_LandmarkItem_StarChallenge))
																		{
																			if (!(rGD_LandmarkItem is RGD_LandmarkItem_TangaroaCockpitLaunch))
																			{
																				if (!(rGD_LandmarkItem is RGD_LandmarkItem_TradingPost))
																				{
																					if (!(rGD_LandmarkItem is RGD_LandmarkItem_VendingMachine))
																					{
																						if (!(rGD_LandmarkItem is RGD_LandmarkItem_WaterWell))
																						{
																							if (!(rGD_LandmarkItem is RGD_TriggerBoxAchievementUnlocker))
																							{
																								if ((object)rGD_LandmarkItem == null)
																								{
																									throw new ArgumentOutOfRangeException("landmarkItem");
																								}
																								writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.Normal, default(FastBufferWriter.ForEnums));
																							}
																							else
																							{
																								writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.AchievementUnlocker, default(FastBufferWriter.ForEnums));
																							}
																						}
																						else
																						{
																							writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.WaterWell, default(FastBufferWriter.ForEnums));
																						}
																					}
																					else
																					{
																						writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.VendingMachine, default(FastBufferWriter.ForEnums));
																					}
																				}
																				else
																				{
																					writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.TradingPost, default(FastBufferWriter.ForEnums));
																				}
																			}
																			else
																			{
																				writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.TangaroaCockpitLaunch, default(FastBufferWriter.ForEnums));
																			}
																		}
																		else
																		{
																			writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.StarChallenge, default(FastBufferWriter.ForEnums));
																		}
																	}
																	else
																	{
																		writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.SnowmobileShed, default(FastBufferWriter.ForEnums));
																	}
																}
																else
																{
																	writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.SeleneFight, default(FastBufferWriter.ForEnums));
																}
															}
															else
															{
																writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.QuestEvent, default(FastBufferWriter.ForEnums));
															}
														}
														else
														{
															writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.WaterLoggedHatch, default(FastBufferWriter.ForEnums));
														}
													}
													else
													{
														writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.QuestEventOneWay, default(FastBufferWriter.ForEnums));
													}
												}
												else
												{
													writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.PickupItem, default(FastBufferWriter.ForEnums));
												}
											}
											else
											{
												writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.Keypad, default(FastBufferWriter.ForEnums));
											}
										}
										else
										{
											writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.JusticeScale, default(FastBufferWriter.ForEnums));
										}
									}
									else
									{
										writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.Elevator, default(FastBufferWriter.ForEnums));
									}
								}
								else
								{
									writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.Container, default(FastBufferWriter.ForEnums));
								}
							}
							else
							{
								writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.ComputerChallenge, default(FastBufferWriter.ForEnums));
							}
						}
						else
						{
							writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.ClawChallenge, default(FastBufferWriter.ForEnums));
						}
					}
					else
					{
						writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.CharacterUnlock, default(FastBufferWriter.ForEnums));
					}
				}
				else
				{
					writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.ChallengePipe, default(FastBufferWriter.ForEnums));
				}
			}
			else
			{
				writer.WriteValueSafe<LandmarkItemType>(LandmarkItemType.Carry, default(FastBufferWriter.ForEnums));
			}
			rGD_LandmarkItem.SerializeFast(writer);
		}
		writer.WriteValueSafe<int>((rgdLandmarkSpawners != null) ? rgdLandmarkSpawners.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdLandmarkSpawners != null)
		{
			RGD_LandmarkSpawner[] array2 = rgdLandmarkSpawners;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out uniqueLandmarkIndex, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdLandmarkItems = new RGD_LandmarkItem[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out LandmarkItemType value2, default(FastBufferWriter.ForEnums));
				switch (value2)
				{
				case LandmarkItemType.Carry:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_CarryNetworked();
					break;
				case LandmarkItemType.ChallengePipe:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_ChallengePipe();
					break;
				case LandmarkItemType.CharacterUnlock:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_CharacterUnlock();
					break;
				case LandmarkItemType.ClawChallenge:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_ClawChallenge();
					break;
				case LandmarkItemType.ComputerChallenge:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_ComputerChallenge();
					break;
				case LandmarkItemType.Container:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_Container();
					break;
				case LandmarkItemType.Elevator:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_Elevator();
					break;
				case LandmarkItemType.JusticeScale:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_JusticeScale();
					break;
				case LandmarkItemType.Keypad:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_Keypad();
					break;
				case LandmarkItemType.PickupItem:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_PickupItem();
					break;
				case LandmarkItemType.QuestEvent:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_QuestEvent();
					break;
				case LandmarkItemType.QuestEventOneWay:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_QuestEvent_OneWay();
					break;
				case LandmarkItemType.SeleneFight:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_SeleneFightChallenge();
					break;
				case LandmarkItemType.SnowmobileShed:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_SnowmobileShed();
					break;
				case LandmarkItemType.StarChallenge:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_StarChallenge();
					break;
				case LandmarkItemType.TangaroaCockpitLaunch:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_TangaroaCockpitLaunch();
					break;
				case LandmarkItemType.TradingPost:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_TradingPost();
					break;
				case LandmarkItemType.VendingMachine:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_VendingMachine();
					break;
				case LandmarkItemType.WaterLoggedHatch:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_WaterLoggedHatch();
					break;
				case LandmarkItemType.WaterWell:
					rgdLandmarkItems[i] = new RGD_LandmarkItem_WaterWell();
					break;
				case LandmarkItemType.AchievementUnlocker:
					rgdLandmarkItems[i] = new RGD_TriggerBoxAchievementUnlocker();
					break;
				case LandmarkItemType.Normal:
					rgdLandmarkItems[i] = new RGD_LandmarkItem();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				rgdLandmarkItems[i].DeserializeFast(reader);
			}
		}
		int value3 = 0;
		reader.ReadValue(out value3, default(FastBufferWriter.ForPrimitives));
		if (value3 > 0)
		{
			rgdLandmarkSpawners = new RGD_LandmarkSpawner[value3];
			for (int j = 0; j < value3; j++)
			{
				rgdLandmarkSpawners[j] = new RGD_LandmarkSpawner();
				rgdLandmarkSpawners[j].DeserializeFast(reader);
			}
		}
	}

	public RGD_Landmark(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "UniqueLandmarkIndex":
				uniqueLandmarkIndex = (int)item.Value;
				break;
			case "LandmarkItems":
				rgdLandmarkItems = (RGD_LandmarkItem[])item.Value;
				break;
			case "RGDLandmarkSpawners":
				rgdLandmarkSpawners = (RGD_LandmarkSpawner[])item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		uniqueLandmarkIndex = -1;
		rgdLandmarkItems = null;
		rgdLandmarkSpawners = null;
	}
}
