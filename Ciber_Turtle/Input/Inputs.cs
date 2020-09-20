using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ciber_Turtle.Input
{
	public class Inputs
	{
		public static List<InputClass> inputs;

		public void AddInput(string _name)
		{
			inputs.Add(new InputClass(_name));
		}

		public void AddInputControl(string inputName, InputType type, string name)
		{
			InputClass input = inputs.Find(x => x.name == inputName);
			if (input.inputs.Contains(new InputControl(type, name)))
			{
				// input.inputs.Find(x => x == new InputControl(type, name)).name ;
			}
			else
			{
				input.inputs.Add(new InputControl(type, name));
			}
		}
	}

	public struct InputClass
	{
		public InputClass(string _name)
		{
			name = _name;
			inputs = new List<InputControl>();
		}

		public string name;
		public List<InputControl> inputs;
	}

	public struct InputControl
	{
		public InputControl(InputType _type, string _name)
		{
			type = _type;
			name = _name;
		}

		public InputType type;
		public string name;
	}

	public enum InputType
	{
		Key,
		MouseButton
	}
}