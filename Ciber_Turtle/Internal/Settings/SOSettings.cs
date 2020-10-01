#pragma warning disable 649
using UnityEngine;
using Ciber_Turtle.UI;

namespace Ciber_Turtle.Internal
{
	// [CreateAssetMenu(fileName = "Ciber_Turtle Settings", menuName = "Tools/Ciber_Turtle Settings", order = 0)]
	public class SOSettings : ScriptableObject
	{
		[Header("UI . Text . Bitmap")]
		[Header("UI . Text")]
		[Header("UI")]
		public SOUIBitTextFont defaultBitmapFont;
		[Header("Tools . Open In Code")]
		[Header("Tools")]
#if UNITY_EDITOR
		public string openInCodeArgs;
#endif

		void Reset()
		{
			defaultBitmapFont = null;
#if UNITY_EDITOR
			openInCodeArgs = "-a";
#endif
		}
	}
}