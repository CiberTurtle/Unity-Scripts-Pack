#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Ciber_Turtle.UI
{
	public class UIToggle : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField, FormerlySerializedAs("active")] bool m_active;
		public bool active { get => m_active; set { m_active = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("unselectedInactive")] Sprite m_unselectedInactive;
		public Sprite unselectedInactive { get => m_unselectedInactive; set { m_unselectedInactive = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("selectedInactive")] Sprite m_selectedInactive;
		public Sprite delectedInactive { get => m_selectedInactive; set { m_selectedInactive = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("unselectedActive")] Sprite m_unselectedActive;
		public Sprite unselectedActive { get => m_unselectedActive; set { m_unselectedActive = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("selectedActive")] Sprite m_selectedActive;
		public Sprite delectedActive { get => m_selectedActive; set { m_selectedActive = value; RefreshSprite(); } }
		[Space]
		public UnityEvent<bool> onToggle = new UnityEvent<bool>();

		bool isSelected;

		Image image;

		void Start()
		{
			image = GetComponent<Image>();

			image.sprite = m_unselectedInactive;
		}

		public void RefreshSprite()
		{
			if (isSelected) image.sprite = m_active ? m_selectedActive : m_selectedInactive;
			else image.sprite = m_active ? m_unselectedActive : m_unselectedInactive;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			isSelected = true;
			RefreshSprite();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			isSelected = false;
			RefreshSprite();
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			m_active = !m_active;
			RefreshSprite();
			onToggle.Invoke(m_active);
		}

		void Reset()
		{
			Start();
		}
	}
}