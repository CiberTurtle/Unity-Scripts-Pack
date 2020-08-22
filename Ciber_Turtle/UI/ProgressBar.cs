using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ciber_Turtle.UI
{
	[ExecuteInEditMode(), AddComponentMenu("UI/Progress Bar"), DisallowMultipleComponent]
	public class ProgressBar : MonoBehaviour
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

		/// <summary>
		///   Posible Progress Bar types
		/// </summary>
		public enum BarType
		{
			linear,
			radial
		}

		#region Varibles > Public
		/// <summary>
		///   Type of a Progress Bar
		/// </summary>
		public BarType type { get => m_type; set { m_type = value; RefreshBar(); } }
		/// <summary>
		///   The value of a Progress Bar
		/// </summary>
		/// <see cref="minValue">
		/// <see cref="maxValue">
		public float value { get => m_value; set { m_value = value; RefreshBar(); } }
		/// <summary>
		///   The mininum value of a Progress Bar
		/// </summary>
		/// <see cref="value">
		/// <see cref="maxValue">
		public float minValue { get => m_minValue; set { m_minValue = value; RefreshBar(); } }
		/// <summary>
		///   The maxinum value of a Progress Bar
		/// </summary>
		/// <see cref="minValue">
		/// <see cref="value">
		public float maxValue { get => m_maxValue; set { m_maxValue = value; RefreshBar(); } }
		/// <summary>
		///   The gradient the affects Progress Bar fill and value text based on the fill amount
		/// </summary>
		/// <see cref="fill">
		/// <see cref="valueText">
		public Gradient gradient { get => m_gradient; set { m_gradient = value; RefreshBar(); } }
		/// <summary>
		///   Enables rounding the value text instead of displaying the entire value of the Progress Bar
		/// </summary>
		/// <see cref="valueText">
		public bool roundValueText { get => m_roundValueText; set { m_roundValueText = value; RefreshBar(); } }
		/// <summary>
		///   Enables rounding the mininum and maxinum value texts instead of displaying the entire mininum and maxinum values of the Progress Bar
		/// </summary>
		/// <see cref="minText">
		/// <see cref="maxText">
		public bool roundMinMaxText { get => m_roundMinMaxText; set { m_roundMinMaxText = value; RefreshBar(); } }
		/// <summary>
		///   Colors the value text based on the gradient
		/// </summary>
		public bool colorValueText { get => m_colorValueText; set { m_colorValueText = value; RefreshBar(); } }
		/// <summary>
		///   The TMPro text object that displays the value text
		/// </summary>
		/// <see cref="valueText">
		public TMP_Text valueText { get => m_valueText; set { m_valueText = value; RefreshBar(); } }
		/// <summary>
		///   The TMPro text object that displays the mininum value text
		/// </summary>
		public TMP_Text minText { get => m_valueMinText; set { m_valueMinText = value; RefreshBar(); } }
		/// <summary>
		///   The TMPro text object that displays the maxinum value text
		/// </summary>
		public TMP_Text maxText { get => m_valueMaxText; set { m_valueMaxText = value; RefreshBar(); } }
		/// <summary>
		///   The mask image of a progress bar (this is the part that gets scaled)
		/// </summary>
		/// <see cref="fill">
		public Image mask { get => m_mask; set { m_mask = value; RefreshBar(); } }
		/// <summary>
		///   The fill image of a progress bar (this is the part that gets colored)
		/// </summary>
		/// <see cref="mask">
		public Image fill { get => m_fill; set { m_fill = value; RefreshBar(); } }
		#endregion

		#region Varibles > Private
		[SerializeField, FormerlySerializedAs("type")] BarType m_type = BarType.linear;
		[SerializeField, FormerlySerializedAs("value")] float m_value = 0;
		[SerializeField, FormerlySerializedAs("minValue")] float m_minValue = 0;
		[SerializeField, FormerlySerializedAs("maxValue")] float m_maxValue = 100;
		[Space]
		[SerializeField, FormerlySerializedAs("colorGradient")] Gradient m_gradient = new Gradient();
		[SerializeField, FormerlySerializedAs("roundValueText")] bool m_roundValueText = true;
		[SerializeField, FormerlySerializedAs("roundMinAndMaxText")] bool m_roundMinMaxText = true;
		[SerializeField, FormerlySerializedAs("colorValueText")] bool m_colorValueText = false;
		[Space]
		[SerializeField, FormerlySerializedAs("valueText")] TMP_Text m_valueText;
		[SerializeField, FormerlySerializedAs("minValueText")] TMP_Text m_valueMinText;
		[SerializeField, FormerlySerializedAs("maxValueText")] TMP_Text m_valueMaxText;
		[Space]
		[SerializeField, FormerlySerializedAs("mask")] Image m_mask;
		[SerializeField, FormerlySerializedAs("fill")] Image m_fill;
		#endregion

		public ProgressBar(float newValue, float max, float min = 0) { m_value = newValue; m_maxValue = max; m_minValue = min; RefreshBar(); }

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

		/// <summary>
		///   Manualy refreshes a progess bar
		/// </summary>
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

		/// <summary>
		///   Calculates the fill percent as a decimal of a Progress Bar
		/// </summary>
		/// <returns>The fill percent based on the value, minValue, and maxValue</returns>
		public float GetFillAmount()
		{
			return Mathf.Clamp01((m_value - m_minValue) / (m_maxValue - m_minValue));
		}
	}
}