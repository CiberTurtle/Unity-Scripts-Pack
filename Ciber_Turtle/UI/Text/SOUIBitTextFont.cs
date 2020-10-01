#pragma warning disable 649
using System.Linq;
using UnityEngine;

namespace Ciber_Turtle.UI
{
	[CreateAssetMenu(fileName = "Bitmap Text Font", menuName = "Ciber_Turtle/Bitmap Text Font", order = 3)]
	public class SOUIBitTextFont : ScriptableObject
	{
		public enum CharSetMode
		{
			ASCII,
			Unicode,
			Custom
		}

		public Texture2D font = null;
		[Space]
		public Vector2Int pixelsPerChar = new Vector2Int(10, 16);
		public int kerning;
		[Space]
		public int offset;
		[Space]
		[SerializeField] CharSetMode charSetMode;
		[SerializeField] bool useLookupTable;
		[TextArea] public string charSet;
		[SerializeField] TextAsset charLookupTable;

		public int FindCharIndex(char letter)
		{
			// switch (charSetMode)
			// {
			// 	case CharSetMode.ASCII:
			// 		goto case default;

			// 	case CharSetMode.Unicode:
			// 		return System.Text.Encoding.Unicode.GetBytes(letter.ToString())[0];

			// 	case CharSetMode.Custom:
			// 		if (useLookupTable)
			// 			return charLookupTable.text.First(x => x == letter);
			// 		else
			// 			return charSet.First(x => x == letter);

			// 	default:
			// 		return System.Text.Encoding.ASCII.GetBytes(letter.ToString())[0];
			// }

			if (useLookupTable)
				return charLookupTable.text.First(x => x == letter);
			else
				return charSet.First(x => x == letter);
		}
	}
}