using UnityEngine;
using Ciber_Turtle.UI;

namespace Ciber_Turtle.Internal
{
	// [CreateAssetMenu(fileName = "Ciber_Turtle Settings", menuName = "Tools/Ciber_Turtle Settings", order = 0)]
	public class SOSettings : ScriptableObject
	{
#if UNITY_EDITOR
		[Header("General")]
		public bool unpackPrefabWhenCreate;
#endif
#if UNITY_EDITOR
		[Header("Pregress Bar")]
		public GameObject progressBarLinearCreate;
		public GameObject progressBarRadialCreate;
#endif
#if UNITY_EDITOR
		[Header("Bitmap Text")]
		public GameObject bitTextCreate;
		public GameObject bitTextFieldCreate;
#endif
		public SOUIBitTextFont defaultBitmapFont;
#if UNITY_EDITOR
		[Header("Tools")]
		public string openInCodeArgs;
#endif

		void Reset()
		{
			defaultBitmapFont = null;
#if UNITY_EDITOR
			progressBarLinearCreate = Resources.Load<GameObject>("ProgressBarLinear");
			progressBarRadialCreate = Resources.Load<GameObject>("ProgressBarRadial");
			bitTextCreate = Resources.Load<GameObject>("BitmapText");
			bitTextFieldCreate = Resources.Load<GameObject>("BitmapTextField");
			defaultBitmapFont = Resources.Load<Ciber_Turtle.UI.SOUIBitTextFont>("Font/Font");
			unpackPrefabWhenCreate = true;
			openInCodeArgs = "-a";
#endif
		}
	}
}