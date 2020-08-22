using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Ciber_Turtle.UI
{
	[AddComponentMenu("UI/Menu"), RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
	public class UIMenu : MonoBehaviour
	{
		bool isOpen = false;

		public float inTime = 0.25f;
		public float outTime = 0.15f;
		public float backgroundTime = 0.1f;
		[Min(0)] public float backgroundRotateTime;
		[Space]
		[Range(0, 1)] public float closedTransparacy = 1;
		public Quaternion closedRotation = Quaternion.Euler(-90, -90, 0);
		public Vector3 closedScale = new Vector3(1, 1, 1);
		[Space]
		public Image[] backgrounds;
		[Space]
		public RectTransform tBackgroundDetailRotate;
		public RectTransform menu;
		public RectTransform content;

		bool isDoingOpen = false;
		bool isDoingClose = false;
		List<Color> backgroundColors = new List<Color>();

		RectTransform rect;
		DG.Tweening.Core.TweenerCore<UnityEngine.Quaternion, UnityEngine.Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> backgroundDetail;

		private void Awake()
		{
			rect = GetComponent<RectTransform>();

			foreach (Image background in backgrounds)
			{
				backgroundColors.Add(background.color);
			}

			backgroundDetail = tBackgroundDetailRotate.DORotate(new Vector3(0, 0, 360), backgroundRotateTime, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetUpdate(true).Pause();

			gameObject.SetActive(false);
		}

		[ContextMenu("Open")]
		public void OPEN()
		{
			if (!isOpen)
			{
				gameObject.SetActive(true);
				StartCoroutine(BeginOpen());
			}
		}

		[ContextMenu("Close")]
		public void CLOSE()
		{
			if (isOpen)
			{
				gameObject.SetActive(true);
				StartCoroutine(BeginClose());
			}
		}

		IEnumerator BeginOpen()
		{
			yield return new WaitUntil(() => !isDoingClose);

			isOpen = true;
			isDoingOpen = true;

			menu.localScale = new Vector3(0, 1, 1);

			menu.DOScale(new Vector3(1, 1, 1), inTime).OnComplete(() => Finsh(false)).SetUpdate(true);

			for (int i = 0; i < backgrounds.Length; i++)
			{
				backgrounds[i].DOColor(backgroundColors[i], backgroundTime).SetUpdate(true);
			}

			backgroundDetail.Play();
		}

		IEnumerator BeginClose()
		{
			yield return new WaitUntil(() => !isDoingOpen);

			isOpen = false;
			isDoingClose = true;

			menu.localScale = new Vector3(1, 1, 1);

			menu.DOScale(new Vector3(0, 1, 1), inTime).OnComplete(() => Finsh(true)).SetUpdate(true);

			for (int i = 0; i < backgrounds.Length; i++)
			{
				backgrounds[i].DOColor(Color.clear, backgroundTime).SetUpdate(true);
			}

			backgroundDetail.Pause();
		}

		void Finsh(bool closed)
		{
			if (closed)
			{
				isDoingClose = false;
				gameObject.SetActive(false);
			}
			else
			{
				isDoingOpen = false;
			}
		}

		public Transform Add(Transform prefab)
		{
			Transform obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, content);
			obj.transform.SetAsLastSibling();
			obj.localScale = Vector3.one;
			return obj;
		}
	}
}