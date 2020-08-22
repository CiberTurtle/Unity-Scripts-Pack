#pragma warning disable 649
using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Ciber_Turtle.GameStats
{
	/// <summary> The main class for getting game stats like current fps </summary>
	[AddComponentMenu("Ciber_Turtle/Game Stats/Game Stats Core"), DisallowMultipleComponent]
	public class GameStatsCore : MonoBehaviour
	{
		#region Varibles > Public
		public float fps { get => m_fps; }
		public int historySize = 100;

		public float allocatedRam { get => m_allocatedRam; }
		public float reservedRam { get => m_reservedRam; }
		public float monoRam { get => m_monoRam; }
		public float[] fpsHistory { get => m_fpsHistory.ToArray(); }
		public float[] ramAllocatedHistory { get => m_fpsHistory.ToArray(); }
		public float[] ramReservedHistory { get => m_fpsHistory.ToArray(); }
		public float[] ramMonoHistory { get => m_fpsHistory.ToArray(); }
		#endregion

		#region Varibles > Private
		float m_fps = 0;
		List<float> m_fpsHistory = new List<float>();

		float m_allocatedRam = 0;
		float m_reservedRam = 0;
		float m_monoRam = 0;
		List<float> m_ramAllocatedHistory = new List<float>();
		List<float> m_ramReservedHistory = new List<float>();
		List<float> m_ramMonoHistory = new List<float>();

		float unscaledDeltaTime = 0;
		#endregion

		private void Awake()
		{
		}

		private void Update()
		{
			unscaledDeltaTime = Time.unscaledDeltaTime;

			#region FPS
			m_fps = 1f / unscaledDeltaTime;
			m_fpsHistory.Insert(0, m_fps);
			#endregion

			#region Ram
#if UNITY_5_6_OR_NEWER
			m_allocatedRam = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
			m_reservedRam = Profiler.GetTotalReservedMemoryLong() / 1048576f;
			m_monoRam = Profiler.GetMonoUsedSizeLong() / 1048576f;
#else
        m_allocatedRam = Profiler.GetTotalAllocatedMemory() / 1048576f;
        m_reservedRam = Profiler.GetTotalReservedMemory() / 1048576f;
        m_monoRam = Profiler.GetMonoUsedSize() / 1048576f;
#endif
			m_ramAllocatedHistory.Insert(0, m_allocatedRam);
			m_ramReservedHistory.Insert(0, m_reservedRam);
			m_ramMonoHistory.Insert(0, m_monoRam);
			#endregion

			if (m_fpsHistory.Count > historySize)
			{
				m_fpsHistory.RemoveAt(m_fpsHistory.Count - 1);
				m_ramAllocatedHistory.RemoveAt(m_ramAllocatedHistory.Count - 1);
				m_ramReservedHistory.RemoveAt(m_ramReservedHistory.Count - 1);
				m_ramMonoHistory.RemoveAt(m_ramMonoHistory.Count - 1);
			}
		}
	}
}