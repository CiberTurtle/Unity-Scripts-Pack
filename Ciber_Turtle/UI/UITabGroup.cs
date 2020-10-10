#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciber_Turtle.UI
{
	public class UITabGroup : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("active")] int m_active;
		public int active { get => m_active; set => m_active = value; }
		[SerializeField] Transform tabsHolder;
		[SerializeField] Transform tabAreasHolder;

		void Start()
		{
			SelectTab(m_active);
		}

		public void SelectTab(int index)
		{
			tabsHolder.GetChild(m_active).GetComponent<UIToggle>().active = false;
			tabAreasHolder.GetChild(m_active).gameObject.SetActive(false);
			m_active = index;
			tabAreasHolder.GetChild(m_active).gameObject.SetActive(true);
			tabsHolder.GetChild(m_active).GetComponent<UIToggle>().active = true;
		}
	}
}