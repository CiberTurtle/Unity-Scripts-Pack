using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Ciber_Turtle.Audio
{
	public class MusicSound
	{
		public MusicSound(string name)
		{
			this.name = name;
		}

		public string name;

		public AudioClip clip;
		public float volume = 0.25f;
		public float fadeInTime = 0.25f;
	}
}