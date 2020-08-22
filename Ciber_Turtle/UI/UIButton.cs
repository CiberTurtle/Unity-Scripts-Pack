using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

[AddComponentMenu("UI/Button"), RequireComponent(typeof(RectTransform)), DisallowMultipleComponent, ExecuteInEditMode]
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	#region Varibles > Private
	[SerializeField, UnityEngine.Serialization.FormerlySerializedAs("interactable")] bool m_interactable = true;
	#endregion

	#region Varibles > Public
	public bool interactable { get => m_interactable; set { m_interactable = value; Refresh(); } }
	public string text = "Button";
	[Space]
	public Color normalColor = new Color(1f, 1f, 1f, 0.9f);
	public Color hoverColor = new Color(1f, 1f, 1f, 1f);
	public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
	[Space]
	public UnityEvent onClick;
	#endregion

	#region Varibles > Cache
	TMP_Text textObj;
	#endregion

	private void Awake()
	{
		textObj = GetComponent<TMP_Text>();
	}

#if UNITY_EDITOR
	private void LateUpdate()
	{
		if (UnityEditor.Selection.transforms.Any(x => x == this.transform))
		{
			textObj.text = text;
			Refresh();
		}
	}
#endif

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (m_interactable)
		{
			textObj.text = $"[{text}]";
			textObj.color = hoverColor;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (m_interactable)
		{
			textObj.text = text;
			textObj.color = normalColor;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onClick.Invoke();
	}

	public void Refresh()
	{
		transform.name = $"{text} Button";
		if (m_interactable)
			textObj.color = disabledColor;
		else
			textObj.color = normalColor;
	}
}
