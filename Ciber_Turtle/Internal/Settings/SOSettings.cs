using UnityEngine;
using Ciber_Turtle.UI;

namespace Ciber_Turtle.Internal
{
	// [CreateAssetMenu(fileName = "Ciber_Turtle Settings", menuName = "Tools/Ciber_Turtle Settings", order = 0)]
	public class SOSettings : ScriptableObject
	{
		[Header("General")]
#if UNITY_EDITOR
		public bool unpackPrefabWhenCreate;
#endif
		[Header("Pregress Bar")]
#if UNITY_EDITOR
		public GameObject progressBarLinearCreate;
		public GameObject progressBarRadialCreate;
#endif
		[Header("Bitmap Text")]
#if UNITY_EDITOR
		public GameObject bitTextCreate;
		public GameObject bitTextFieldCreate;
#endif
		public SOUIBitTextFont defaultBitmapFont;
		[Header("Tools")]
#if UNITY_EDITOR
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