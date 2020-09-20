#pragma warning disable 649
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Ciber_Turtle.UI
{
	[AddComponentMenu("UI/Bitmap Text"), ExecuteInEditMode]
	public class UIBitText : MonoBehaviour
	{
		public enum UIDrawMode
		{
			RawImage,
			Image,
			SpriteRenderer
		}

		public enum ColorBlendMode
		{
			Multiply,
			Add,
			Subtract,
			Divide
		}

		[SerializeField, FormerlySerializedAs("text"), TextArea] string m_text;
		public string text { get => m_text; set { m_text = value; RefreshSprite(); } }
		[Space]
		[SerializeField, FormerlySerializedAs("uiDrawMode")] UIDrawMode m_uiDrawMode;
		public UIDrawMode uiDrawMode { get => m_uiDrawMode; set { m_uiDrawMode = value; RefreshSprite(); } }
		[SerializeField, FormerlySerializedAs("autoSize")] bool m_autoSize = true;
		public bool autoSize { get => m_autoSize; set { m_autoSize = value; RefreshSize(); } }
		[SerializeField, FormerlySerializedAs("size"), Range(1, 4)] int m_size;
		public int size { get => m_size; set { m_size = value; RefreshSize(); } }
		[SerializeField, FormerlySerializedAs("pivot")] Vector2 m_pivot = new Vector2(0.5f, 0.5f);
		public Vector2 pivot { get => m_pivot; set { m_pivot = value; RefreshSprite(); } }
		[Space]
		[SerializeField, FormerlySerializedAs("color")] Color m_color;
		public Color color { get => m_color; set { m_color = value; RefreshSprite(); } }
		[Space]
		[SerializeField, FormerlySerializedAs("font")] SOUIBitTextFont m_font;
		public SOUIBitTextFont font { get => m_font; set { m_font = value; RefreshSprite(); } }

		Image img;
		RawImage rimg;
		SpriteRenderer spr;
		Texture2D textRend;
		Sprite sp;

		void Awake()
		{
			textRend = new Texture2D(2, 2);
			textRend.filterMode = FilterMode.Point;
			textRend.wrapMode = TextureWrapMode.Clamp;
			textRend.Apply();
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

		[ContextMenu("Refresh")]
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

			textRend.Resize(m_text.Length * (m_font.pixelsPerChar.x + m_font.kerning), m_font.pixelsPerChar.y);

			List<int> textIndexes = new List<int>();

			for (int i = 0; i < m_text.Length; i++)
			{
				textIndexes.Add(m_font.FindCharIndex(m_text[i]));
			}

			for (int i = 0; i < text.Length; i++)
			{
				textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x + m_font.kerning, m_font.pixelsPerChar.y, m_font.font.GetPixels(0, m_font.font.height - m_font.pixelsPerChar.y, m_font.pixelsPerChar.x + m_font.kerning, m_font.pixelsPerChar.y));
				textRend.SetPixels(i * (m_font.pixelsPerChar.x + m_font.kerning), 0, m_font.pixelsPerChar.x, m_font.pixelsPerChar.y, CharToColor(textIndexes[i]).Select(x => x * m_color).ToArray());
			}

			textRend.Apply();
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