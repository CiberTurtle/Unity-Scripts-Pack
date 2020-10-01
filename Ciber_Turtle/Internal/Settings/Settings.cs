#pragma warning disable 649
using UnityEngine;

namespace Ciber_Turtle.Internal
{
	public class Settings
	{
		public static SOSettings settings
		{
			get
			{
				if (m_settings == null)
				{
					m_settings = Resources.Load<SOSettings>("Ciber_Turtle Settings");
				}

				return m_settings;
			}
			set
			{
				if (m_settings == null)
				{
					m_settings = Resources.Load<SOSettings>("Ciber_Turtle Settings");
				}

				m_settings = value;
			}
		}
		static SOSettings m_settings;
	}
}