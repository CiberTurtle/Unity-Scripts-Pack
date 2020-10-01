using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct MinMax
{
	[SerializeField] float m_min;
	[SerializeField] float m_max;

	public float min { get => this.m_min; set => this.m_min = value; }

	public float max { get => this.m_max; set => this.m_max = value; }

	public float randomValue { get => UnityEngine.Random.Range(this.m_min, this.m_max); }

	public MinMax(float m_min, float m_max)
	{
		this.m_min = m_min;
		this.m_max = m_max;
	}

	public float Clamp(float value) { return Mathf.Clamp(value, this.m_min, this.m_max); }
	public bool IsInRange(float value) { return value > m_min && value < m_max; }
}

public class MinMaxSliderAttribute : PropertyAttribute
{
	public readonly float Min;
	public readonly float Max;

	public MinMaxSliderAttribute(float min, float max)
	{
		Min = min;
		Max = max;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMax)), CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
	{
		if (prop.serializedObject.isEditingMultipleObjects) return 0f;
		return base.GetPropertyHeight(prop, label) + 16f;
	}

	public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
	{
		if (prop.serializedObject.isEditingMultipleObjects) return;

		var minProperty = prop.FindPropertyRelative("m_min");
		var maxProperty = prop.FindPropertyRelative("m_max");
		var minmax = attribute as MinMaxSliderAttribute ?? new MinMaxSliderAttribute(0, 1);
		position.height -= 16f;

		label = EditorGUI.BeginProperty(position, label, prop);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		var min = minProperty.floatValue;
		var max = maxProperty.floatValue;

		var left = new Rect(position.x, position.y, position.width / 2 - 11f, position.height);
		var right = new Rect(position.x + position.width - left.width, position.y, left.width, position.height);
		var mid = new Rect(left.xMax, position.y, 22, position.height);
		min = Mathf.Clamp(EditorGUI.FloatField(left, min), minmax.Min, max);
		EditorGUI.LabelField(mid, " to ");
		max = Mathf.Clamp(EditorGUI.FloatField(right, max), min, minmax.Max);

		position.y += 16f;
		EditorGUI.MinMaxSlider(position, GUIContent.none, ref min, ref max, minmax.Min, minmax.Max);

		minProperty.floatValue = min;
		maxProperty.floatValue = max;
		EditorGUI.EndProperty();
	}
}
#endif