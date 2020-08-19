using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

[AddComponentMenu("UI/Field Display"), RequireComponent(typeof(RectTransform)), DisallowMultipleComponent, ExecuteInEditMode]
public class UIFieldDisplay : MonoBehaviour
{
  public string fieldName { get => m_fieldName; set { m_fieldName = value; UpdateField(); } }
  [SerializeField, FormerlySerializedAs("fieldName")] string m_fieldName;
  public string placeholderText { get => m_placeholderText; set { m_placeholderText = value; UpdateField(); } }
  [SerializeField, FormerlySerializedAs("placeholderText")] string m_placeholderText;

  public TMP_Text placeholder { get => m_placeholder; set { m_placeholder = value; UpdateField(); } }
  [Space]
  [SerializeField, FormerlySerializedAs("placeholder")] TMP_Text m_placeholder;

  TMP_InputField field;

  private void Awake()
  {
    field = GetComponent<TMP_InputField>();

    UpdateField();
  }

#if UNITY_EDITOR
  private void LateUpdate()
  {
    if (UnityEditor.Selection.transforms.Any(x => x == this.transform))
    {
      UpdateField();
    }
  }
#endif

  public void UpdateField()
  {
    transform.name = $"{m_fieldName} Field";
    if (m_placeholder) m_placeholder.text = m_placeholderText;
  }
}
