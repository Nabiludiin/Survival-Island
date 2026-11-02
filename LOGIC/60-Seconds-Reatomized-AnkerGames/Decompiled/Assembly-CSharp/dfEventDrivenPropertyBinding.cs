using System;
using UnityEngine;

[Serializable]
[AddComponentMenu("Daikon Forge/Data Binding/Event-Driven Property Binding")]
public class dfEventDrivenPropertyBinding : dfPropertyBinding
{
	public string SourceEventName;

	public string TargetEventName;

	protected dfEventBinding sourceEventBinding;

	protected dfEventBinding targetEventBinding;

	public override void Update()
	{
	}

	public static dfEventDrivenPropertyBinding Bind(Component sourceComponent, string sourceProperty, string sourceEvent, Component targetComponent, string targetProperty, string targetEvent)
	{
		return Bind(sourceComponent.gameObject, sourceComponent, sourceProperty, sourceEvent, targetComponent, targetProperty, targetEvent);
	}

	public static dfEventDrivenPropertyBinding Bind(GameObject hostObject, Component sourceComponent, string sourceProperty, string sourceEvent, Component targetComponent, string targetProperty, string targetEvent)
	{
		if (hostObject == null)
		{
			throw new ArgumentNullException("hostObject");
		}
		if (sourceComponent == null)
		{
			throw new ArgumentNullException("sourceComponent");
		}
		if (targetComponent == null)
		{
			throw new ArgumentNullException("targetComponent");
		}
		if (string.IsNullOrEmpty(sourceProperty))
		{
			throw new ArgumentNullException("sourceProperty");
		}
		if (string.IsNullOrEmpty(targetProperty))
		{
			throw new ArgumentNullException("targetProperty");
		}
		if (string.IsNullOrEmpty(sourceEvent))
		{
			throw new ArgumentNullException("sourceEvent");
		}
		dfEventDrivenPropertyBinding dfEventDrivenPropertyBinding2 = hostObject.AddComponent<dfEventDrivenPropertyBinding>();
		dfEventDrivenPropertyBinding2.DataSource = new dfComponentMemberInfo
		{
			Component = sourceComponent,
			MemberName = sourceProperty
		};
		dfEventDrivenPropertyBinding2.DataTarget = new dfComponentMemberInfo
		{
			Component = targetComponent,
			MemberName = targetProperty
		};
		dfEventDrivenPropertyBinding2.SourceEventName = sourceEvent;
		dfEventDrivenPropertyBinding2.TargetEventName = targetEvent;
		dfEventDrivenPropertyBinding2.Bind();
		return dfEventDrivenPropertyBinding2;
	}

	public override void Bind()
	{
		if (isBound)
		{
			return;
		}
		if (!DataSource.IsValid || !DataTarget.IsValid)
		{
			Debug.LogError($"Invalid data binding configuration - Source:{DataSource}, Target:{DataTarget}");
			return;
		}
		sourceProperty = DataSource.GetProperty();
		targetProperty = DataTarget.GetProperty();
		if (sourceProperty != null && targetProperty != null)
		{
			if (!string.IsNullOrEmpty(SourceEventName) && SourceEventName.Trim() != "")
			{
				bindSourceEvent();
			}
			if (!string.IsNullOrEmpty(TargetEventName) && TargetEventName.Trim() != "")
			{
				bindTargetEvent();
			}
			else if (targetProperty.PropertyType == typeof(string) && sourceProperty.PropertyType != typeof(string))
			{
				useFormatString = !string.IsNullOrEmpty(FormatString);
			}
			MirrorSourceProperty();
			isBound = sourceEventBinding != null;
		}
	}

	public override void Unbind()
	{
		if (isBound)
		{
			isBound = false;
			if (sourceEventBinding != null)
			{
				sourceEventBinding.Unbind();
				UnityEngine.Object.Destroy(sourceEventBinding);
				sourceEventBinding = null;
			}
			if (targetEventBinding != null)
			{
				targetEventBinding.Unbind();
				UnityEngine.Object.Destroy(targetEventBinding);
				targetEventBinding = null;
			}
		}
	}

	public void MirrorSourceProperty()
	{
		targetProperty.Value = formatValue(sourceProperty.Value);
	}

	public void MirrorTargetProperty()
	{
		sourceProperty.Value = targetProperty.Value;
	}

	private object formatValue(object value)
	{
		try
		{
			if (useFormatString && !string.IsNullOrEmpty(FormatString))
			{
				return string.Format(FormatString, value);
			}
		}
		catch (FormatException message)
		{
			Debug.LogError(message, this);
			if (Application.isPlaying)
			{
				enabled = false;
			}
		}
		return value;
	}

	private void bindSourceEvent()
	{
		sourceEventBinding = gameObject.AddComponent<dfEventBinding>();
		sourceEventBinding.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
		sourceEventBinding.DataSource = new dfComponentMemberInfo
		{
			Component = DataSource.Component,
			MemberName = SourceEventName
		};
		sourceEventBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = this,
			MemberName = "MirrorSourceProperty"
		};
		sourceEventBinding.Bind();
	}

	private void bindTargetEvent()
	{
		targetEventBinding = gameObject.AddComponent<dfEventBinding>();
		targetEventBinding.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
		targetEventBinding.DataSource = new dfComponentMemberInfo
		{
			Component = DataTarget.Component,
			MemberName = TargetEventName
		};
		targetEventBinding.DataTarget = new dfComponentMemberInfo
		{
			Component = this,
			MemberName = "MirrorTargetProperty"
		};
		targetEventBinding.Bind();
	}

	public override string ToString()
	{
		string text = ((DataSource != null && DataSource.Component != null) ? DataSource.Component.GetType().Name : "[null]");
		string text2 = ((DataSource != null && !string.IsNullOrEmpty(DataSource.MemberName)) ? DataSource.MemberName : "[null]");
		string text3 = ((DataTarget != null && DataTarget.Component != null) ? DataTarget.Component.GetType().Name : "[null]");
		string text4 = ((DataTarget != null && !string.IsNullOrEmpty(DataTarget.MemberName)) ? DataTarget.MemberName : "[null]");
		return $"Bind {text}.{text2} -> {text3}.{text4}";
	}
}
