#pragma warning disable 649
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ciber_Turtle.UI;

namespace Ciber_Turtle.GameStats
{
	[AddComponentMenu("Ciber_Turtle/Game Stats/Game Stats Renderer"), RequireComponent(typeof(GameStatsCore)), DisallowMultipleComponent]
	public class GameStatsRenderer : MonoBehaviour
	{
		#region Varibles > [SerializeField]
		[Header("General")]
		[SerializeField] float timeBtwRefresh = 1;
		[Header("Coloring")]
		[SerializeField] float goodFps = 50f;
		[SerializeField] Color goodColor = Color.green;
		[SerializeField] float okFps = 30f;
		[SerializeField] Color okColor = Color.yellow;
		[Space]
		//[SerializeField] float badFps;
		[SerializeField] Color badColor = Color.red;
		[Header("Text")]
		[SerializeField] string fpsStarterString = "Fps: ";
		[SerializeField] string fpsAverageStarterString = "Avg: ";
		[Space]
		[SerializeField] Vector2Int fpsGraphSize;
		[Space]
		[SerializeField] UIBitText fpsText;
		[SerializeField] UIBitText fpsAverageText;
		[SerializeField] Image fpsGraph;
		[SerializeField, Tooltip("The Rect Transform that gets scaled")] RectTransform fpsGraphHolder;
		#endregion

		#region Varibles > Private
		float refreshCooldown;
		int fpsGraphXPos;
		Texture2D fpsGraphRend;

		GameStatsCore core;
		#endregion

		void Awake()
		{
			core = GetComponent<GameStatsCore>();

			fpsGraphRend = new Texture2D(fpsGraphSize.x, fpsGraphSize.y);
			fpsGraphHolder.sizeDelta = fpsGraphSize;
			fpsGraphRend.filterMode = FilterMode.Point;
			fpsGraphRend.wrapMode = TextureWrapMode.Clamp;
		}

		void LateUpdate()
		{
			refreshCooldown -= Time.unscaledDeltaTime;
			if (refreshCooldown < 0)
			{
				refreshCooldown = timeBtwRefresh;
				Refresh();
			}

			GetFPSGraph();
		}

		public void Refresh()
		{
			fpsText.text = $"{ fpsStarterString }{ Mathf.RoundToInt(core.fps).ToString() }";

			float sum = 0;
			foreach (float num in core.fpsHistory)
			{
				sum += num;
			}
			sum /= core.fpsHistory.Length;

			fpsAverageText.text = $"{ fpsAverageStarterString }{ Mathf.RoundToInt(sum).ToString() }";

			if (core.fps > goodFps)
				fpsText.color = goodColor;
			else if (core.fps > okFps)
				fpsText.color = okColor;
			else
				fpsText.color = badColor;

			if (sum > goodFps)
				fpsAverageText.color = goodColor;
			else if (sum > okFps)
				fpsAverageText.color = okColor;
			else
				fpsAverageText.color = badColor;
		}

		void GetFPSGraph()
		{
			float fpsSnapshot = core.fps;

			fpsGraphXPos++;
			if (fpsGraphXPos > fpsGraphSize.x)
			{
				fpsGraphXPos = 0;
			}

			Color color;
			if (fpsSnapshot + 1 > goodFps)
				color = goodColor;
			else if (fpsSnapshot + 1 > okFps)
				color = okColor;
			else
				color = badColor;

			for (int i = 0; i < fpsGraphSize.y; i++)
			{
				fpsGraphRend.SetPixel(fpsGraphXPos, i, Color.clear);
			}

			if (fpsGraphXPos < fpsGraphSize.x)
			{
				for (int i = 0; i < fpsGraphSize.y; i++)
				{
					fpsGraphRend.SetPixel(fpsGraphXPos + 1, Mathf.Clamp(i, 0, fpsGraphRend.height), Color.black);
				}
			}

			for (int i = 0; i < Mathf.RoundToInt(fpsSnapshot); i++)
			{
				fpsGraphRend.SetPixel(fpsGraphXPos, Mathf.Clamp(i, 0, fpsGraphRend.height), color);
			}

			for (int i = 0; i < 3; i++)
			{
				fpsGraphRend.SetPixel(fpsGraphXPos, Mathf.Clamp(Mathf.RoundToInt(fpsSnapshot) - i, 0, fpsGraphRend.height), new Color(color.r - 0.25f, color.g - 0.25f, color.b - 0.25f));
			}

			fpsGraphRend.Apply();

			fpsGraph.sprite = Sprite.Create(fpsGraphRend, new Rect(Vector2.zero, new Vector2(fpsGraphRend.width, fpsGraphRend.height)), Vector2.zero);
		}
	}
}