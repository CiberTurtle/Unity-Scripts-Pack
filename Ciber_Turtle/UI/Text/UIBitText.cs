#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Ciber_Turtle.UI
{
	public enum UIDrawMode
	{
		RawImage,
		Image,
		SpriteRenderer
	}

	[AddComponentMenu("UI/Bitmap Text"), ExecuteInEditMode]
	public class UIBitText : MonoBehaviour
	{

		public enum ColorBlendMode
		{
			Multiply,
			Add,
			Subtract,
			Divide
		}

		public enum SelectionFGDrawMode
		{
			None,
			Invert,
			Colored
		}

		public struct Selection
		{
			public int start;
			public int end;

			public Selection(int start, int end)
			{
				this.start = start;
				this.end = end;
			}
		}

		[Header("Text")]
		[SerializeField, FormerlySerializedAs("text"), TextArea] string m_text;
		public string text { get => m_text; set { m_text = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("color")] Color m_color;
		public Color color { get => m_color; set { m_color = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("uiDrawMode")] UIDrawMode m_uiDrawMode;
		public UIDrawMode uiDrawMode { get => m_uiDrawMode; set { m_uiDrawMode = value; RefreshSprite(); } }
		[Header("Transform")]
		[SerializeField, FormerlySerializedAs("autoSize")] bool m_autoSize = true;
		public bool autoSize { get => m_autoSize; set { m_autoSize = value; RefreshSize(); } }
		[SerializeField, FormerlySerializedAs("size"), Range(1, 4)] int m_size;
		public int size { get => m_size; set { m_size = value; RefreshSize(); } }
		[SerializeField, FormerlySerializedAs("pivot")] Vector2 m_pivot = new Vector2(0.5f, 0.5f);
		public Vector2 pivot { get => m_pivot; set { m_pivot = value; RefreshSprite(); } }
		[Header("Other")]
		[SerializeField, FormerlySerializedAs("caretColor")] Color m_caretColor;
		public Color caretColor { get => m_caretColor; set => m_caretColor = value; }
		[Space]
		[SerializeField, FormerlySerializedAs("selectionFGDrawMode")] SelectionFGDrawMode m_selectionFGDrawMode;
		public SelectionFGDrawMode selectionFGDrawMode { get => m_selectionFGDrawMode; set => m_selectionFGDrawMode = value; }
		[SerializeField, FormerlySerializedAs("selectionBGColor")] Color m_selectionBGColor;
		public Color selectionBGColor { get => m_selectionBGColor; set => m_selectionBGColor = value; }
		[SerializeField, FormerlySerializedAs("selectionFGColor")] Color m_selectionFGColor;
		public Color selectionFGColor { get => m_selectionFGColor; set => m_selectionFGColor = value; }
		[Header("References")]
		[SerializeField, FormerlySerializedAs("font")] SOUIBitTextFont m_font;
		public SOUIBitTextFont font { get => m_font; set { m_font = value; RefreshSprite(); } }

		[Space]

		int m_caretIndex = -1;
		/// <summary> Set to -1 to disable caret </summary>
		public int caretIndex { get => m_caretIndex; set { m_caretIndex = value; RefreshSprite(); } }

		[Space]
		Selection m_selection = new Selection(-1, -1);
		/// <summary> Set start to -1 to disable selection </summary>
		public Selection selection { get => m_selection; set => m_selection = value; }

		Image img;
		RawImage rimg;
		SpriteRenderer spr;
		Texture2D textRend;
		Sprite sp;

		void Reset()
		{
			m_font = Ciber_Turtle.Internal.Settings.settings.defaultBitmapFont;
			RefreshSize();
			RefreshSprite();
		}

		void Awake()
		{
			textRend = new Texture2D(2, 2);
			textRend.filterMode = FilterMode.Point;
			textRend.wrapMode = TextureWrapMode.Clamp;
			textRend.mipMapBias = -10;
			textRend.Apply(true);
		}

		void Start()
		{
			switch (m_uiDrawMode)
			{
				case UIDrawMode.RawImage:
					rimg = GetComponent<RawImage>();
					break;
				case UIDrawMode.Image:
					img = GetComponent<Image>();
					break;
				case UIDrawMode.SpriteRenderer:
					spr = GetComponent<SpriteRenderer>();
					break;
			}

			RefreshSprite();
			RefreshSize();
		}

		void Update()
		{
#if UNITY_EDITOR
			if (UnityEditor.Selection.transforms.Any(x => x.transform == transform))
			{
				RefreshSprite();
				RefreshSize();
			}
#endif
		}

		[ContextMenu("Refresh/Sprite")]
		public void RefreshSprite()
		{
			switch (m_uiDrawMode)
			{
				case UIDrawMode.RawImage:
					rimg = GetComponent<RawImage>();
					break;
				case UIDrawMode.Image:
					img = GetComponent<Image>();
					break;
				case UIDrawMode.SpriteRenderer:
					spr = GetComponent<SpriteRenderer>();
					break;
			}

			textRend.Resize(Mathf.Clamp(m_text.Length * (m_font.pixelsPerChar.x + m_font.kerning), m_font.pixelsPerChar.x, int.MaxValue), m_font.pixelsPerChar.y);

			List<int> textIndexes = new List<int>();

			for (int i = 0; i < m_text.Length; i++)
			{
				textIndexes.Add(m_font.FindCharIndex(m_text[i]));
			}

			List<Color> colorBGBlock = new List<Color>();

			if (m_selection.start >= 0)
			{
				for (int i = 0; i < (m_font.pixelsPerChar.x + m_font.kerning) * m_font.pixelsPerChar.y; i++)
				{
					colorBGBlock.Add(m_selectionBGColor);
				}
			}

			for (int i = 0; i < text.Length; i++)
			{
				if (m_selection.start >= 0)
				{
					if (i >= m_selection.start && i <= m_selection.end)
					{
						textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x + m_font.kerning, m_font.pixelsPerChar.y, colorBGBlock.ToArray());

						switch (m_selectionFGDrawMode)
						{
							case SelectionFGDrawMode.None:
								textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x, m_font.pixelsPerChar.y, CharToColor(textIndexes[i]).Select(x => x * m_color).ToArray());
								break;
							case SelectionFGDrawMode.Invert:
								textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x, m_font.pixelsPerChar.y, CharToColor(textIndexes[i]).Select(x => x * -1).ToArray());
								break;
							case SelectionFGDrawMode.Colored:
								textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x, m_font.pixelsPerChar.y, CharToColor(textIndexes[i]).Select(x => x * m_selectionFGColor).ToArray());
								break;
						}
					}
				}
				else
				{
					textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x + m_font.kerning, m_font.pixelsPerChar.y, m_font.font.GetPixels(0, m_font.font.height - m_font.pixelsPerChar.y, m_font.pixelsPerChar.x + m_font.kerning, m_font.pixelsPerChar.y));

					textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x, m_font.pixelsPerChar.y, CharToColor(textIndexes[i]).Select(x => x * m_color).ToArray());
				}
			}

			if (m_caretIndex >= 0)
			{
				for (int i = 0; i < m_font.pixelsPerChar.y; i++)
				{
					textRend.SetPixel((m_caretIndex - 1) * (m_font.pixelsPerChar.x + m_font.kerning) + m_font.pixelsPerChar.x, i, m_caretColor);
				}
			}

			textRend.Apply(true);

			switch (m_uiDrawMode)
			{
				case UIDrawMode.RawImage:
					rimg.texture = textRend;
					rimg.SetNativeSize();
					break;
				case UIDrawMode.Image:
					img.sprite = Sprite.Create(textRend, new Rect(0, 0, textRend.width, textRend.height), m_pivot);
					img.SetNativeSize();
					break;
				case UIDrawMode.SpriteRenderer:
					spr.sprite = Sprite.Create(textRend, new Rect(0, 0, textRend.width, textRend.height), m_pivot);
					break;
			}
		}

		Color[] CharToColor(int charIndex)
		{
			Vector2Int cord = CharToCord(charIndex + m_font.offset);
			return m_font.font.GetPixels(cord.x, m_font.font.height - cord.y, m_font.pixelsPerChar.x, m_font.pixelsPerChar.y);
		}

		Vector2Int CharToCord(int charIndex)
		{
			int x = 0;
			int y = 0;

			for (int i = 0; i < charIndex; i++)
			{
				if (x >= m_font.font.width / m_font.pixelsPerChar.x - 1)
				{
					y++;
					x = 0;
					continue;
				}
				else
				{
					x++;
				}
			}

			return new Vector2Int(x * m_font.pixelsPerChar.x, y * m_font.pixelsPerChar.y);
		}

		[ContextMenu("Refresh/Transform")]
		public void RefreshSize()
		{
			if (m_autoSize)
			{
				switch (m_uiDrawMode)
				{
					case UIDrawMode.RawImage:
						GetComponent<RectTransform>().localScale = new Vector3(m_size, m_size, 1);
						break;
					case UIDrawMode.Image:
						GetComponent<RectTransform>().localScale = new Vector3(m_size, m_size, 1);
						break;
					case UIDrawMode.SpriteRenderer:
						transform.localScale = new Vector3(m_size, m_size, 1);
						break;
				}
			}
		}
	}
}