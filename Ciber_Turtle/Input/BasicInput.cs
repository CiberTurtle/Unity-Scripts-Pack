using UnityEngine;

namespace Ciber_Turtle.Input
{
	public class BasicInput
	{
		public static bool enabled = true;

		static Vector2 mousePosition;

		public static bool GetButton(string name)
		{
			if (enabled)
				return UnityEngine.Input.GetButton(name);
			else
				return false;
		}

		public static bool GetButtonDown(string name)
		{
			if (enabled)
				return UnityEngine.Input.GetButtonDown(name);
			else
				return false;
		}

		public static bool GetButtonUp(string name)
		{
			if (enabled)
				return UnityEngine.Input.GetButtonUp(name);
			else
				return false;
		}

		public static bool GetMouseButton(int number)
		{
			if (enabled)
				return UnityEngine.Input.GetMouseButton(number);
			else
				return false;
		}

		public static bool GetMouseButtonDown(int number)
		{
			if (enabled)
				return UnityEngine.Input.GetMouseButtonDown(number);
			else
				return false;
		}

		public static bool GetMouseButtonUp(int number)
		{
			if (enabled)
				return UnityEngine.Input.GetMouseButtonUp(number);
			else
				return false;
		}

		public static float GetAxis(string name)
		{
			if (enabled)
				return UnityEngine.Input.GetAxis(name);
			else
				return 0f;
		}

		public static float GetAxisRaw(string name)
		{
			if (enabled)
				return UnityEngine.Input.GetAxisRaw(name);
			else
				return 0f;
		}

		public static Vector2 GetMousePos()
		{
			if (enabled)
			{
				mousePosition = UnityEngine.Input.mousePosition;
				return mousePosition;
			}
			else return mousePosition;
		}
	}
}