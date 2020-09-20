using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using DG.Tweening;

namespace Ciber_Turtle.UI
{
	[AddComponentMenu("UI/Popup"), RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
	public class UIPopup : MonoBehaviour
	{
		bool isOpen = true;
		public float inTime = 0.25f;
		public float outTime = 0.15f;
		public float delayTime = 2f;

		public string TITLE { get => m_TITLE; set { m_TITLE = value; title.text = m_TITLE; } }
		[Space]
		[SerializeField, FormerlySerializedAs("TITLE")] string m_TITLE;

		public string SUBTITLE { get => m_SUBTITLE; set { m_SUBTITLE = value; subtitle.text = m_SUBTITLE; } }
		[SerializeField, FormerlySerializedAs("SUBTITLE")] string m_SUBTITLE;
		[Space]
		public TMP_Text title;
		public TMP_Text subtitle;

		bool isDoingOpen = false;
		bool isDoingClose = false;

		RectTransform rect;

		private void Awake()
		{
			rect = GetComponent<RectTransform>();

			StartCoroutine(BeginClose());
		}

		public void OPEN()
		{
			if (!isOpen)
			{
				title.text = m_TITLE;
				subtitle.text = m_SUBTITLE;
				StartCoroutine(BeginOpen());
			}
		}

		public void CLOSE()
		{
			if (isOpen)
			{
				StartCoroutine(BeginClose());
			}
		}

		void Open()
		{
			GetComponent<RectTransform>().rotation = Quaternion.Euler(-90, -90, 0);
		}

		IEnumerator BeginOpen()
		{
			yield return new WaitUntil(() => !isDoingClose);
			isOpen = true;
			isDoingOpen = true;
			rect.rotation = Quaternion.Euler(0, 90, 0);
			rect.DORotateQuaternion(Quaternion.Euler(0, 0, 0), inTime).OnComplete(() => StartCoroutine(Close())).SetUpdate(true);
		}

		[ContextMenu("Close")]
		IEnumerator BeginClose()
		{
			yield return new WaitUntil(() => !isDoingOpen);
			isOpen = false;
			isDoingClose = true;
			rect.rotation = Quaternion.Euler(0, 0, 0);
			rect.DORotateQuaternion(Quaternion.Euler(0, -90, 0), outTime).OnComplete(() => isDoingClose = false).SetUpdate(true);
		}

		IEnumerator Close()
		{
			isDoingOpen = false;
			yield return new WaitForSecondsRealtime(delayTime);
			CLOSE();
		}
	}
}