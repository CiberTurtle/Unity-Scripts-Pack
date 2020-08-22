using UnityEngine;

namespace Ciber_Turtle.Audio
{
	[System.Serializable]
	public class SFXSound
	{
		public SFXSound(string name, bool alt = false)
		{
			this.name = name;
			this.alt = alt;
		}

		public string name;
		[MinMaxSlider(0, 1)] public MinMax volume = new MinMax(1, 1);
		[MinMaxSlider(0, 2)] public MinMax pitch = new MinMax(1, 1);
		[Space]
		public bool alt = false;
		[Space]
		public AudioClip[] sounds = new AudioClip[0];
	}
}