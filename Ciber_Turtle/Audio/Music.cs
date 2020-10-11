#pragma warning disable 649
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Ciber_Turtle.Audio
{
	[AddComponentMenu("Music/Music Manager"), DisallowMultipleComponent]
	public class Music : MonoBehaviour
	{
		public static Music current { get => m_current; }

		static Music m_current;

		List<MusicSound> music = new List<MusicSound>();

		public AudioMixerGroup altMixer = null;
		[Header("Settings")]
		public bool addWhenOveride = true;
		public bool dontDestroyOnLoad = false;
		[Tooltip("Time in seconds after the sound has finished playing before destroying it (use this to prevent cutoff)")] public float delayBeforeDestroy = 0.1f;
		public AudioMixerGroup musicMixer;
		[Header("Debug")]
		public ErrorMode errorMode = ErrorMode.FloatingText;

		public MusicSound currentSong { get => m_currentSong; }
		MusicSound m_currentSong;

		public List<MusicSound> queuedSongs { get => m_queuedSongs; }
		List<MusicSound> m_queuedSongs;

		AudioSource currentSource;
		AudioSource queuedSource;

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

			currentSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			queuedSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			currentSource.playOnAwake = false;
			currentSource.loop = true;
			queuedSource.playOnAwake = false;
			queuedSource.loop = true;
		}

		public MusicSound QueueSoung(string name)
		{
			MusicSound song = music.Find(x => x.name == name);

			if (song != null)
			{
				m_queuedSongs.Add(song);
				ReloadSongs();
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

			return song;
		}

		public void PlayMusicRaw(AudioClip clip, float volume, float pitch, bool alt = false)
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();

			source.clip = clip;
			source.volume = volume;
			source.pitch = pitch;

			if (alt) source.outputAudioMixerGroup = altMixer;
			else source.outputAudioMixerGroup = musicMixer;

			Destroy(source, source.clip.length + delayBeforeDestroy);
			source.Play();
		}

		public void ReloadSongs()
		{
			if (m_queuedSongs.Count > 0)
			{
				currentSource.clip = currentSong.clip;
			}
		}

		public MusicSound AddSong(string name)
		{
			if (!music.Any(x => x.name == name))
			{
				MusicSound song = new MusicSound(name);
				music.Add(song);
				return song;
			}
			else
			{
				return null;
			}
		}
	}
}