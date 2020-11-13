#if UNITY_EDITOR
#pragma warning disable 649
using UnityEngine;
using System.Diagnostics;
using UnityEditor;

namespace Ciber_Turtle.Tools
{
	public class OpenProjectInCode
	{
		[MenuItem("Tools/Open Project In Code")]
		static void Open()
		{
			// > -r to reuse open window (defualt) <
			// Process.Start("code", $"\"{Application.dataPath}/../\"".Replace("/", "\\"));
			ProcessStartInfo p = new ProcessStartInfo();
			p.FileName = "code";
			p.Arguments = $"{Ciber_Turtle.Internal.Settings.settings.openInCodeArgs} \"{$"{Application.dataPath}/../\"".Replace("/", "\\")}";
			p.WindowStyle = ProcessWindowStyle.Hidden;
			Process.Start(p);
		}
	}
}
#endif