#pragma warning disable 649
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Ciber_Turtle.Audio
{
	[AddComponentMenu("Ciber_Turtle/Audio Manager"), DisallowMultipleComponent]
	public class SFX : MonoBehaviour
	{
		public static SFX current { get => m_current; }
		static SFX m_current;

		[SerializeField] List<SFXSound> sfxs = new List<SFXSound>();
		[Header("Settings")]
		public bool addWhenOveride = true;
		public bool dontDestroyOnLoad = false;
		[Tooltip("Time in seconds after the sound has finished playing before destroying it (use this to prevent cutoff)")] public float delayBeforeDestroy = 0.1f;
		[Space]
		public AudioMixerGroup sfxMixer = null;
		public AudioMixerGroup altMixer = null;
		[Header("Debug")]
		public ErrorMode errorMode = ErrorMode.FloatingText;

		private void Awake()
		{
			if (m_current != null)
			{
				Destroy(this.gameObject);
				return;
			}
			else
			{
				m_current = this;
				if (dontDestroyOnLoad) DontDestroyOnLoad(this.gameObject);
			}
		}

		public SFXSound PlaySound(string name, float volume = 0, float pitch = 0)
		{
			SFXSound sfx = sfxs.Find(x => x.name == name);

			if (sfx != null)
			{
				PlaySoundRaw(sfx.sounds[Util.GetRandomItem(sfx.sounds.Length)], sfx.volume.randomValue * Convert.ToInt16(addWhenOveride) + volume, sfx.pitch.randomValue * Convert.ToInt16(addWhenOveride) + pitch, sfx.alt);
			}
			else
			{
				switch (errorMode)
				{
					case ErrorMode.None:
						break;
					case ErrorMode.MessageLog:
						Debug.Log($"SFX Manager > Couldn't find sound with name: {name}");
						break;
					case ErrorMode.WarnningLog:
						Debug.LogWarning($"SFX Manager > Couldn't find sound with name: {name}");
						break;
					case ErrorMode.ErrorLog:
						Debug.LogError($"SFX Manager > Couldn't find sound with name: {name}");
						break;
					case ErrorMode.FloatingText:
						break;
				}
			}

			return sfx;
		}

		public void PlaySoundRaw(AudioClip clip, float volume, float pitch, bool alt = false)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();

			source.clip = clip;
			source.volume = volume;
			source.pitch = pitch;

			if (alt) source.outputAudioMixerGroup = altMixer;
			else source.outputAudioMixerGroup = sfxMixer;

			Destroy(source, source.clip.length + delayBeforeDestroy);
			source.Play();
		}

		public SFXSound Add(string name)
		{
			if (!sfxs.Any(x => x.name == name))
			{
				SFXSound sfx = new SFXSound(name);
				sfxs.Add(sfx);
				return sfx;
			}
			else
				return null;
		}
	}
}