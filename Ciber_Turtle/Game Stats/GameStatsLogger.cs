#pragma warning disable 649
using System.IO;
using UnityEngine;

namespace Ciber_Turtle.GameStats.Logging
{
	[AddComponentMenu("Ciber_Turtle/Game Stats/Game Stats Logger"), RequireComponent(typeof(GameStatsCore)), DisallowMultipleComponent]
	public class GameStatsLogger : MonoBehaviour
	{
		#region Varibles > Public
		public bool loggingEnable = true;
		#endregion

		#region Varibles > Private
		[SerializeField, TextArea] string logPath = "Assets/Logs/GameStatsLog{version_num}.txt";
		[SerializeField] float timeBtwLogs = 10f;

		int playNum;
		int logNum;
		float logCooldown;

		GameStatsCore core;
		#endregion

		private void Awake()
		{
			core = GetComponent<GameStatsCore>();

			logCooldown = timeBtwLogs;

			Debug.Log($"Logging in {logPath}");
		}

		private void FixedUpdate()
		{
			logCooldown -= Time.unscaledDeltaTime;
			if (logCooldown < 0)
			{
				logCooldown = timeBtwLogs;
				Log();
			}
		}

		public bool Log()
		{
			string logId;
			string logLine;

			logId = $"[v{Application.version} #{playNum}.{logNum} | { System.DateTime.Now.ToShortDateString() } | { System.DateTime.Now.ToLongTimeString()}]";

			logLine = $"fps: {core.fps}";

			Debug.Log($"\n{logId}\n{logLine}");

			return false;
		}
	}
}
