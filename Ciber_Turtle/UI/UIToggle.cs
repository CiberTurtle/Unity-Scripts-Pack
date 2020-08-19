using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;
using TMPro;

[AddComponentMenu("UI/Toggle"), RequireComponent(typeof(RectTransform)), DisallowMultipleComponent, ExecuteInEditMode]
public class UIToggle : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
  [System.Serializable] public class OnClick : UnityEvent<bool> { }

  public bool interactable { get => m_interactable; set { m_interactable = value; Refresh(); } }
  [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("interactable")] bool m_interactable = true;

  public bool isOn;

  public string checkboxName { get => m_checkboxName; set { m_checkboxName = value; Refresh(); } }
  [Space]
  [SerializeField, FormerlySerializedAs("checkboxName")] string m_checkboxName;

  [Space]
  public TMP_Text checkbox;

  public TMP_Text checkboxLabel { get => m_checkboxLabel; set { m_checkboxLabel = value; Refresh(); } }
  [SerializeField, FormerlySerializedAs("checkboxLabel")] TMP_Text m_checkboxLabel;
  [Space]
  public Color normalColor = new Color(1f, 1f, 1f, 0.9f);
  public Color hoverColor = new Color(1f, 1f, 1f, 1f);
  public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
  [Space]
  [Space]
  public OnClick onClick;

#if UNITY_EDITOR
  private void LateUpdate()
  {
    if (UnityEditor.Selection.transforms.Any(x => x == this.transform))
    {
      Refresh();
    }
  }
#endif

  public void Refresh()
  {
    transform.name = $"{m_checkboxName} Toggle";

    if (isOn)
      checkbox.text = "[X]";
    else
      checkbox.text = "[  ]";

    if (interactable)
    {
      checkbox.color = normalColor;
      if (m_checkboxLabel) m_checkboxLabel.color = normalColor;
    }
    else
    {
      checkbox.color = disabledColor;
      if (m_checkboxLabel) m_checkboxLabel.color = disabledColor;
    }

    if (m_checkboxLabel) m_checkboxLabel.text = m_checkboxName;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    isOn = !isOn;
    onClick.Invoke(isOn);
    Refresh();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    checkbox.color = hoverColor;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    checkbox.color = normalColor;
  }
}
