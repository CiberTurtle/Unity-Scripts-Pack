#pragma warning disable 649
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Ciber_Turtle.UI
{
	[RequireComponent(typeof(UIToggle))]
	public class UITab : MonoBehaviour
	{
		[SerializeField] UITabGroup tabGroup;

		bool isSelected;

		Image image;
		[HideInInspector] public UIToggle toggle;

		void Start()
		{
			image = GetComponent<Image>();
			toggle = GetComponent<UIToggle>();

			toggle.onToggle.AddListener(x => Refresh());
		}

		public void Refresh()
		{
			tabGroup.SelectTab(transform.GetSiblingIndex());
		}

		void Reset()
		{
			Start();

			UITabGroup tabGroup = transform.GetComponentInParent<UITabGroup>();
			if (tabGroup == null) tabGroup = transform.parent.GetComponentInParent<UITabGroup>();
			this.tabGroup = tabGroup;
		}
	}
}