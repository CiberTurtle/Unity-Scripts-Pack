using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ciber_Turtle.UI
{
	[ExecuteInEditMode(), AddComponentMenu("UI/Progress Bar"), DisallowMultipleComponent]
	public class UIProgressBar : MonoBehaviour
	{
#if UNITY_EDITOR
		[MenuItem("GameObject/UI/Progress Bar/Linear Progress Bar")]
		public static void AddLinear()
		{
			GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Linear Progress Bar"));
			obj.name = "Linear Progress Bar";
			obj.transform.SetParent(Selection.activeGameObject.transform, false);
		}

		[MenuItem("GameObject/UI/Progress Bar/Radial Progress Bar")]
		public static void AddRadial()
		{
			GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Radial Progress Bar"));
			obj.name = "Radial Progress Bar";
			obj.transform.SetParent(Selection.activeGameObject.transform, false);
		}
#endif

		public enum BarType
		{
			linear,
			radial
		}

		[SerializeField, FormerlySerializedAs("type")] BarType m_type = BarType.linear;
		public BarType type { get => m_type; set { m_type = value; RefreshBar(); } }
		[Space]
		[SerializeField, FormerlySerializedAs("value")] float m_value = 0;
		public float value { get => m_value; set { m_value = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("minValue")] float m_minValue = 0;
		public float minValue { get => m_minValue; set { m_minValue = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("maxValue")] float m_maxValue = 100;
		public float maxValue { get => m_maxValue; set { m_maxValue = value; RefreshBar(); } }
		[Space]
		[SerializeField, FormerlySerializedAs("colorGradient")] Gradient m_gradient = new Gradient();
		public Gradient gradient { get => m_gradient; set { m_gradient = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("roundValueText")] bool m_roundValueText = true;
		public bool roundValueText { get => m_roundValueText; set { m_roundValueText = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("roundMinAndMaxText")] bool m_roundMinMaxText = true;
		public bool roundMinMaxText { get => m_roundMinMaxText; set { m_roundMinMaxText = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("colorValueText")] bool m_colorValueText = false;
		public bool colorValueText { get => m_colorValueText; set { m_colorValueText = value; RefreshBar(); } }
		[Space]
		[SerializeField, FormerlySerializedAs("valueText")] UIBitText m_valueText;
		public UIBitText valueText { get => m_valueText; set { m_valueText = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("minValueText")] UIBitText m_valueMinText;
		public UIBitText minText { get => m_valueMinText; set { m_valueMinText = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("maxValueText")] UIBitText m_valueMaxText;
		public UIBitText maxText { get => m_valueMaxText; set { m_valueMaxText = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("mask")] Image m_mask;
		public Image mask { get => m_mask; set { m_mask = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("fill")] Image m_fill;
		public Image fill { get => m_fill; set { m_fill = value; RefreshBar(); } }

		public UIProgressBar(float newValue, float max, float min = 0) { m_value = newValue; m_maxValue = max; m_minValue = min; RefreshBar(); }

		private void Awake()
		{
			RefreshBar();
		}

#if UNITY_EDITOR
		private void LateUpdate()
		{
			if (Selection.transforms.Any(x => x.transform == transform))
			{
				RefreshBar();
			}
		}
#endif

		public void RefreshBar()
		{
			m_value = Mathf.Clamp(m_value, m_minValue, m_maxValue);

			float amount = GetFillAmount();

			switch (m_type)
			{
				case BarType.linear:
					if (m_mask) { m_mask.fillAmount = amount; }

					if (m_fill) { m_fill.color = m_gradient.Evaluate(amount); }
					break;
				case BarType.radial:
					if (m_fill) { m_mask.fillAmount = amount; m_fill.color = m_gradient.Evaluate(amount); }

					// if (mask) {  }
					break;
			}

			if (m_valueText)
			{
				if (m_roundValueText)
				{
					m_valueText.text = Mathf.RoundToInt(m_value).ToString();
				}
				else
				{
					m_valueText.text = m_value.ToString();
				}

				if (m_colorValueText)
				{
					m_valueText.color = m_gradient.Evaluate(amount);
				}
			}

			if (m_valueMinText)
			{
				if (m_roundMinMaxText)
				{
					m_valueMinText.text = Mathf.RoundToInt(m_minValue).ToString();
				}
				else
				{
					m_valueMinText.text = m_minValue.ToString();
				}
			}

			if (m_valueMaxText)
			{
				if (m_roundMinMaxText)
				{
					m_valueMaxText.text = Mathf.RoundToInt(m_maxValue).ToString();
				}
				else
				{
					m_valueMaxText.text = m_maxValue.ToString();
				}
			}
		}

		public float GetFillAmount()
		{
			return Mathf.Clamp01((m_value - m_minValue) / (m_maxValue - m_minValue));
		}
	}
}