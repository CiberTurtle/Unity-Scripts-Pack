using UnityEngine;

namespace Ciber_Turtle.Audio
{
	[AddComponentMenu("Audio/Music Trigger"), RequireComponent(typeof(Collider2D))]
	public class MusicTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player"))
			{
				// Music.current;
			}
		}
	}
}