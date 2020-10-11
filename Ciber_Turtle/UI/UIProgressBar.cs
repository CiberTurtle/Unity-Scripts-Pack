using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Ciber_Turtle.Internal;

namespace Ciber_Turtle.UI
{
	[ExecuteInEditMode(), AddComponentMenu("UI/Progress Bar"), DisallowMultipleComponent]
	public class UIProgressBar : MonoBehaviour
	{
#if UNITY_EDITOR
		[MenuItem("GameObject/UI/Progress Bar/Linear Progress Bar")]
		public static void AddLinear()
		{
			Create.CreateObjAtSelection(Settings.settings.progressBarLinearCreate, "Linear Progress Bar");
		}

		[MenuItem("GameObject/UI/Progress Bar/Radial Progress Bar")]
		public static void AddRadial()
		{
			Create.CreateObjAtSelection(Settings.settings.progressBarRadialCreate, "Radial Progress Bar");
		}
#endif

		public enum BarType
		{
			Linear,
			Radial
		}

		[SerializeField, FormerlySerializedAs("type")] BarType m_type = BarType.Linear;
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
		[SerializeField, FormerlySerializedAs("fill")] Image m_fill;
		public Image fill { get => m_fill; set { m_fill = value; RefreshBar(); } }
		[SerializeField, FormerlySerializedAs("colored")] Image m_colored;
		public Image colored { get => m_colored; set { m_colored = value; RefreshBar(); } }

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
				case BarType.Linear:
					if (m_fill) { m_fill.fillAmount = amount; }

					if (m_colored) { m_colored.color = m_gradient.Evaluate(amount); }
					break;
				case BarType.Radial:
					if (m_colored) { m_fill.fillAmount = amount; m_colored.color = m_gradient.Evaluate(amount); }

					// if (fill) {  }
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