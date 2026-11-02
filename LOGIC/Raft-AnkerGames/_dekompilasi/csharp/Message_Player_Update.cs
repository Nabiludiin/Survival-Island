using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Player_Update : Message_NetworkBehaviour
{
	public int x;

	public int y;

	public int z;

	public int q;

	public int w;

	public byte c;

	public int vx;

	public int vy;

	public int vz;

	public int vw;

	public bool r;

	public bool g;

	public bool i;

	public new bool t;

	public bool h;

	public bool k;

	public bool l;

	public bool a;

	public int p;

	public string[] anim_triggers;

	public Vector3 Position
	{
		get
		{
			return new Vector3((float)x / 100f, (float)y / 100f, (float)z / 100f);
		}
		set
		{
			x = (int)(value.x * 100f);
			y = (int)(value.y * 100f);
			z = (int)(value.z * 100f);
		}
	}

	public float RotationX
	{
		get
		{
			return (float)q / 100f;
		}
		set
		{
			q = (int)(value * 100f);
		}
	}

	public float RotationY
	{
		get
		{
			return (float)w / 100f;
		}
		set
		{
			w = (int)(value * 100f);
		}
	}

	public ControllerType ControllerType
	{
		get
		{
			return (ControllerType)c;
		}
		set
		{
			c = (byte)value;
		}
	}

	public Vector4 AnimationVelocity
	{
		get
		{
			return new Vector4((float)vx / 100f, (float)vy / 100f, (float)vz / 100f, (float)vw / 100f);
		}
		set
		{
			vx = (int)(value.x * 100f);
			vy = (int)(value.y * 100f);
			vz = (int)(value.z * 100f);
			vw = (int)(value.w * 100f);
		}
	}

	public bool RaftAsParent
	{
		get
		{
			return r;
		}
		set
		{
			r = value;
		}
	}

	public bool ControllerIsGrounded
	{
		get
		{
			return g;
		}
		set
		{
			g = value;
		}
	}

	public bool Anim_ItemHit
	{
		get
		{
			return i;
		}
		set
		{
			i = value;
		}
	}

	public bool Anim_HookThrow
	{
		get
		{
			return t;
		}
		set
		{
			t = value;
		}
	}

	public bool Anim_HookInHand
	{
		get
		{
			return h;
		}
		set
		{
			h = value;
		}
	}

	public bool Anim_Crouching
	{
		get
		{
			return k;
		}
		set
		{
			k = value;
		}
	}

	public bool Anim_JustLanded
	{
		get
		{
			return l;
		}
		set
		{
			l = value;
		}
	}

	public bool Anim_HasAmmo
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	public int Anim_FullBodyIndex
	{
		get
		{
			return p;
		}
		set
		{
			p = value;
		}
	}

	protected bool Equals(Message_Player_Update other)
	{
		if (anim_triggers == null && other.anim_triggers != null)
		{
			return false;
		}
		if (anim_triggers != null && other.anim_triggers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other) && x == other.x && y == other.y && z == other.z && q == other.q && w == other.w && c == other.c && vx == other.vx && vy == other.vy && vz == other.vz && vw == other.vw && r == other.r && g == other.g && i == other.i && t == other.t && h == other.h && k == other.k && l == other.l && a == other.a && p == other.p)
		{
			if (anim_triggers != null || other.anim_triggers != null)
			{
				return Enumerable.SequenceEqual(anim_triggers, other.anim_triggers);
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
		return Equals((Message_Player_Update)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(x);
		hashCode.Add(y);
		hashCode.Add(z);
		hashCode.Add(q);
		hashCode.Add(w);
		hashCode.Add(c);
		hashCode.Add(vx);
		hashCode.Add(vy);
		hashCode.Add(vz);
		hashCode.Add(vw);
		hashCode.Add(r);
		hashCode.Add(g);
		hashCode.Add(i);
		hashCode.Add(t);
		hashCode.Add(h);
		hashCode.Add(k);
		hashCode.Add(l);
		hashCode.Add(a);
		hashCode.Add(p);
		hashCode.Add(anim_triggers);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_Player_Update left, Message_Player_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Player_Update left, Message_Player_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_Player_Update(Messages type, MonoBehaviour_Network behaviour, Network_Player playerNetwork)
		: base(type, behaviour)
	{
		PersonController personController = playerNetwork.PersonController;
		ControllerType = personController.controllerType;
		RaftAsParent = personController.HasRaftAsParent;
		ControllerIsGrounded = personController.controller.isGrounded;
		if (RaftAsParent)
		{
			Position = personController.transform.localPosition;
		}
		else
		{
			Position = personController.transform.position;
		}
		if (playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState)
		{
			RotationY = playerNetwork.playerPivot.eulerAngles.y;
		}
		else
		{
			RotationY = personController.transform.eulerAngles.y;
		}
		RotationX = playerNetwork.playerPivot.eulerAngles.x;
		Anim_JustLanded = personController.justLanded;
		personController.justLanded = false;
		Animator anim = playerNetwork.Animator.anim;
		anim_triggers = playerNetwork.Animator.networkAnimTriggers.ToArray();
		playerNetwork.Animator.networkAnimTriggers.Clear();
		AnimationVelocity = new Vector4(anim.GetFloat("VelocityX"), anim.GetFloat("VelocityY"), anim.GetFloat("VelocityZ"), anim.GetFloat("VelocityW"));
		Anim_ItemHit = anim.GetBool("ItemHit");
		Anim_HookThrow = anim.GetBool("HookThrow");
		Anim_HookInHand = anim.GetBool("HookInHand");
		Anim_Crouching = anim.GetBool("Crouching");
		Anim_HasAmmo = anim.GetBool("HasAmmo");
		Anim_FullBodyIndex = anim.GetInteger("FullBodyIndex");
	}

	public Message_Player_Update()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(77))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in q, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in c, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in vx, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in vy, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in vz, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in vw, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in r, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in g, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in this.i, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in t, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in h, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in k, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in l, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in a, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in p, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue<int>(anim_triggers.Length, default(FastBufferWriter.ForPrimitives));
		for (int i = 0; i < anim_triggers.Length; i++)
		{
			writer.WriteValueSafe(anim_triggers[i]);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out q, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out c, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out vx, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out vy, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out vz, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out vw, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out r, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out g, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out this.i, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out t, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out h, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out k, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out l, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out a, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out p, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		anim_triggers = new string[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out anim_triggers[i], false);
		}
	}
}
