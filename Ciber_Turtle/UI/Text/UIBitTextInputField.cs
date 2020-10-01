#pragma warning disable 649
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Ciber_Turtle.Input;

namespace Ciber_Turtle.UI
{
	[ExecuteInEditMode]
	public class UIBitTextInputField : Selectable
	{
		public string value { get => Value; set => Value = value; }

		[Space]
		[SerializeField, FormerlySerializedAs("value"), TextArea] string m_value;
		public string Value { get => m_value; set { m_value = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("inputText")] UIBitText m_inputText;
		public UIBitText inputText { get => m_inputText; set { m_inputText = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("placeholderText")] UIBitText m_placeholderText;
		public UIBitText placeholderText { get => m_placeholderText; set { m_placeholderText = value; RefreshSprite(); } }
		[Space]
		[SerializeField] bool disableInputWhenSelected;
		[Space]
		[SerializeField, FormerlySerializedAs("onChange")] UnityEvent<string> m_onChange = new UnityEvent<string>();
		public UnityEvent<string> onChange { get => m_onChange; set { m_onChange = value; } }
		[SerializeField, FormerlySerializedAs("onSubmit")] UnityEvent<string> m_onSubmit = new UnityEvent<string>();
		public UnityEvent<string> onSubmit { get => m_onSubmit; set => m_onSubmit = value; }

		protected override void Start()
		{
			RefreshSprite();
		}

		void Update()
		{
#if UNITY_EDITOR
			if (UnityEditor.Selection.transforms.Any(x => x == transform))
			{
				RefreshSprite();
			}
#endif
		}

		void LateUpdate()
		{
			if (currentSelectionState == SelectionState.Selected)
			{
				if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
				{
					int i = Mathf.Clamp(m_inputText.caretIndex - 1, 0, m_value.Length);

					m_inputText.caretIndex = i;
				}
				else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
				{
					int i = Mathf.Clamp(m_inputText.caretIndex + 1, 0, m_value.Length);

					m_inputText.caretIndex = i;
				}
				else
				{
					if (disableInputWhenSelected)
					{
						BasicInput.enabled = false;
					}

					string lastValue = m_value;

					foreach (char c in UnityEngine.Input.inputString)
					{
						if (c == '\b') // has backspace/delete been pressed?
						{
							if (m_value.Length != 0 && m_inputText.caretIndex != 0)
							{
								// m_value = m_value.Substring(m_value.Length - m_inputText.caretIndex, m_value.Length);
								m_value = m_value.Remove(m_inputText.caretIndex - 1, 1);
								m_inputText.caretIndex = Mathf.Clamp(m_inputText.caretIndex - 1, 0, m_value.Length);
							}
						}
						else if ((c == '\n') || (c == '\r')) // enter/return
						{
							m_onSubmit.Invoke(m_value);
						}
						else
						{
							m_value = m_value.Insert(m_inputText.caretIndex, c.ToString());
							m_inputText.caretIndex = Mathf.Clamp(m_inputText.caretIndex + 1, 0, m_value.Length);
						}
					}

					if (m_value != lastValue)
					{
						m_inputText.text = m_value;
						m_onChange.Invoke(m_value);
					}
				}
			}
			else
			{
				if (disableInputWhenSelected)
				{
					BasicInput.enabled = true;
				}

				if (m_inputText.caretIndex != -1)
				{
					m_inputText.caretIndex = -1;
				}
			}
		}

		public void RefreshSprite()
		{
			m_inputText.text = m_value;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			m_inputText.caretIndex = m_value.Length;
			this.Select();
		}
	}
}