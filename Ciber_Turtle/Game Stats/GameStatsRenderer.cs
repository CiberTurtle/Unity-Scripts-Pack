#pragma warning disable 649
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ciber_Turtle.GameStats
{
	[AddComponentMenu("Ciber_Turtle/Game Stats/Game Stats Renderer"), RequireComponent(typeof(GameStatsCore)), DisallowMultipleComponent]
	public class GameStatsRenderer : MonoBehaviour
	{
		#region Varibles > Public
		[Header("General")]
		public int timeBtwRefresh = 1;
		[Header("Coloring")]
		public float goodFps = 50f;
		public Color goodColor = Color.green;
		public float okFps = 30f;
		public Color okColor = Color.yellow;
		[Space]
		//public float badFps;
		public Color badColor = Color.red;
		[Header("Text")]
		public string fpsStarterString = "Fps: ";
		public string fpsAverageStarterString = "Avg: ";
		[Space]
		public TMP_Text fpsText;
		public TMP_Text fpsAverageText;
		public Image fpsGraph;
		#endregion

		#region Varibles > Private
		float refreshCooldown;
		Texture2D blankTex;

		GameStatsCore core;
		#endregion

		private void Awake()
		{
			core = GetComponent<GameStatsCore>();
			blankTex = new Texture2D(core.historySize, 120);

			Color[] pixels = Enumerable.Repeat(new Color(0, 0, 0, 0.75f), Screen.width * Screen.height).ToArray();
			blankTex.SetPixels(pixels);
		}

		private void LateUpdate()
		{
			refreshCooldown -= Time.unscaledDeltaTime;
			if (refreshCooldown < 0)
			{
				refreshCooldown = timeBtwRefresh;
				Refresh();
			}
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
			float[] snapFPSHistory;
			snapFPSHistory = core.fpsHistory;

			Texture2D tex = new Texture2D(core.historySize, 120);

			Graphics.CopyTexture(blankTex, tex);

			for (int i = 0; i < snapFPSHistory.Length - 1; i++)
			{
				Color color;
				if (snapFPSHistory[i] + 1 > goodFps)
					color = goodColor;
				else if (snapFPSHistory[i] + 1 > okFps)
					color = okColor;
				else
					color = badColor;

				for (int ii = 0; ii < 4; ii++)
				{
					tex.SetPixel(i, Mathf.RoundToInt(snapFPSHistory[i] - ii), color);
				}
			}

			tex.Apply();

			Sprite spr = Sprite.Create(
				tex,
				new Rect(
					0,
					0,
					core.historySize,
					120
				),
				Vector2.zero
			);

			fpsGraph.sprite = spr;
		}
	}
}