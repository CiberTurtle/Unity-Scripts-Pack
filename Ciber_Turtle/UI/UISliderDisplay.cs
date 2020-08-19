using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;

namespace Ciber_Turtle.UI
{
  [AddComponentMenu("UI/Slider Display"), RequireComponent(typeof(Slider)), DisallowMultipleComponent, ExecuteInEditMode]
  public class UISliderDisplay : MonoBehaviour
  {
    public float roundingAmount { get => m_roundingAmount; set { m_roundingAmount = value; OnValueChanged(); } }
    [SerializeField, FormerlySerializedAs("roundingAmount"), Min(1)] float m_roundingAmount;

    public bool flipValueText { get => m_flipValueText; set { m_flipValueText = value; OnValueChanged(); } }
    [SerializeField, FormerlySerializedAs("flipValueText")] bool m_flipValueText;

    public string sliderName { get => m_sliderName; set { m_sliderName = value; OnValueChanged(); } }
    [SerializeField, FormerlySerializedAs("name")] string m_sliderName;

    public TMP_Text value { get => m_value; set { m_value = value; OnValueChanged(); } }
    [Space]
    [SerializeField, FormerlySerializedAs("value")] TMP_Text m_value;

    public TMP_Text nameText { get => m_nameText; set { m_nameText = value; OnValueChanged(); } }
    [SerializeField, FormerlySerializedAs("nameText")] TMP_Text m_nameText;

    public TMP_Text minValue { get => m_minValue; set { m_minValue = value; OnValueChanged(); } }
    [SerializeField, FormerlySerializedAs("minValue")] TMP_Text m_minValue;

    public TMP_Text maxValue { get => m_maxValue; set { m_maxValue = value; OnValueChanged(); } }
    [SerializeField, FormerlySerializedAs("maxValue")] TMP_Text m_maxValue;

    Slider slider;

    private void Awake()
    {
      slider = GetComponent<Slider>();

      slider.onValueChanged.AddListener(OnValueChanged);

      OnValueChanged();
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
      if (UnityEditor.Selection.transforms.Any(x => x == this.transform))
      {
        OnValueChanged();
      }
    }
#endif

    public void OnValueChanged(float _value = 0)
    {
      transform.name = $"{m_sliderName} Slider";

      if (m_nameText) m_nameText.text = $"{m_sliderName}:";
      if (m_value)
      {
        m_value.text = DoRound(slider.value).ToString();
        if (m_flipValueText)
          if (slider.value > 0.5f)
            m_value.alignment = TextAlignmentOptions.Left;
          else
            m_value.alignment = TextAlignmentOptions.Right;
      }
      if (m_minValue) m_minValue.text = DoRound(slider.minValue).ToString();
      if (m_maxValue) m_maxValue.text = DoRound(slider.maxValue).ToString();
    }

    float DoRound(float _value)
    {
      return Mathf.Round(_value * m_roundingAmount) / m_roundingAmount;
    }
  }
}